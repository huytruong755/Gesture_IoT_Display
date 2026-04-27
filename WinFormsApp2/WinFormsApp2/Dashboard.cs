using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class Dashboard : Form
    {
        // ──────────────────────────────────────────────
        // Fields
        // ──────────────────────────────────────────────
        private SerialPort? _serialPort;
        private bool _isRunning = false;
        private CancellationTokenSource? _cts;
        private GestureService? _gestureService;

        private readonly Modify modify = new Modify();

        // ──────────────────────────────────────────────
        // Constructor
        // ──────────────────────────────────────────────
        public Dashboard()
        {
            InitializeComponent();
            InitializeGrid();
            LoadAvailablePorts();
            LoadHistory(); // Load lịch sử từ DB khi mở Dashboard
        }

        // ──────────────────────────────────────────────
        // Khởi tạo bổ sung
        // ──────────────────────────────────────────────

        /// <summary>Đặt tên cột DataGridView cho thân thiện.</summary>
        private void InitializeGrid()
        {
            colGesture.HeaderText = "Gesture";
            colConfidence.HeaderText = "Confidence (%)";
            colTime.HeaderText = "Thời gian";

            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>Tự động liệt kê các COM port hiện có vào textBox_COMPort.</summary>
        private void LoadAvailablePorts()
        {
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length > 0)
                textBox_COMPort.Text = ports[0];

            textBox_BaudRate.Text = "9600";
            textBox_APIServer.Text = "http://localhost:5000";
        }

        // ══════════════════════════════════════════════
        // TAB 1 – Camera
        // ══════════════════════════════════════════════

        private void button_start_Click(object sender, EventArgs e)
        {
            if (_isRunning) return;
            _ = StartCameraAsync();
        }

        private async Task StartCameraAsync()
        {
            _isRunning = true;
            _cts = new CancellationTokenSource();
            button_start.Enabled = false;
            button_stop.Enabled = true;

            // Initialize GestureService with API URL
            string apiUrl = textBox_APIServer.Text.Trim();
            if (string.IsNullOrWhiteSpace(apiUrl))
                apiUrl = "http://127.0.0.1:5000";

            _gestureService = new GestureService(apiUrl);

            // Check if Python API is running
            bool isHealthy = await _gestureService.HealthCheckAsync();
            if (!isHealthy)
            {
                MessageBox.Show(
                    "Python API server không khả dụng tại " + apiUrl + "\n\n" +
                    "Vui lòng chạy gesture_api.py trước:\n" +
                    "python gesture_api.py",
                    "Lỗi kết nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                _isRunning = false;
                button_start.Enabled = true;
                button_stop.Enabled = false;
                return;
            }

            label_gestureRate.Text = "Gesture Rate: Đang khởi động...";
            label_LEDPerformance.Text = "LED Performance: Dừng hiển thị";

            // ⭐ Đóng COM port Tab 2 nếu đang mở (để tránh xung đột)
            if (_serialPort != null && _serialPort.IsOpen)
            {
                try
                {
                    _serialPort.Close();
                    _serialPort.Dispose();
                    _serialPort = null;
                    button_connect.Text = "Connect";
                    label_performance.Text = "Performance: Đã ngắt kết nối (cho phép camera sử dụng port)";
                }
                catch { }
            }

            // Get COM port settings
            string comPort = textBox_COMPort.Text.Trim();
            if (string.IsNullOrWhiteSpace(comPort))
                comPort = "COM3";

            if (!int.TryParse(textBox_BaudRate.Text, out int baudrate))
                baudrate = 9600;

            // Start camera
            bool started = await _gestureService.StartAsync(comPort, baudrate);
            if (!started)
            {
                MessageBox.Show("Không thể khởi động camera.", "Lỗi");
                _isRunning = false;
                button_start.Enabled = true;
                button_stop.Enabled = false;
                return;
            }

            Task.Run(() => CameraFrameLoop(_cts.Token));
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            StopCamera();
        }

        private async void StopCamera()
        {
            if (!_isRunning) return;
            _isRunning = false;
            _cts?.Cancel();

            if (_gestureService != null)
                await _gestureService.StopAsync();

            this.Invoke(() =>
            {
                button_start.Enabled = true;
                button_stop.Enabled = false;
                label_gestureRate.Text = "Gesture Rate:";
                label_LEDPerformance.Text = "LED Performance:";
                pictureBox_Camera.Image = null;
            });
        }

        /// <summary>
        /// Main camera loop - fetches frames from Python API
        /// </summary>
        private async Task CameraFrameLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _isRunning)
            {
                try
                {
                    // Get frame from Python API
                    var frameData = await _gestureService.GetFrameAsync();
                    if (frameData == null)
                    {
                        await Task.Delay(100, token);
                        continue;
                    }

                    // Update UI on main thread
                    this.Invoke(() =>
                    {
                        try
                        {
                            // Display frame
                            if (!string.IsNullOrEmpty(frameData.Frame))
                            {
                                byte[] imageBytes = Convert.FromBase64String(frameData.Frame);
                                using (var ms = new System.IO.MemoryStream(imageBytes))
                                {
                                    var image = Image.FromStream(ms);
                                    if (pictureBox_Camera.Image != null)
                                        pictureBox_Camera.Image.Dispose();
                                    pictureBox_Camera.Image = image;
                                }
                            }

                            // Display gesture and confidence
                            if (!string.IsNullOrEmpty(frameData.Gesture))
                            {
                                label_gestureRate.Text =
                                    $"Gesture Rate: {frameData.Gesture} ({frameData.Confidence:F1}%)";

                                // Save to DB
                                SaveGestureToDB(frameData.Gesture, (int)frameData.Confidence);

                                // Add to history grid
                                string timeStamp = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
                                dataGridView1.Rows.Insert(0, frameData.Gesture,
                                    frameData.Confidence.ToString("F1"), timeStamp);
                            }
                            else
                            {
                                label_gestureRate.Text = "Gesture Rate: Chờ cử chỉ...";
                            }

                            // Display LED status
                            label_LEDPerformance.Text = frameData.LedStatus
                                ? "LED Performance: Đang hiển thị"
                                : "LED Performance: Dừng hiển thị";
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"UI update error: {ex.Message}");
                        }
                    });

                    // Limit frame rate to ~30 FPS
                    await Task.Delay(33, token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Camera loop error: {ex.Message}");
                    await Task.Delay(500, token);
                }
            }
        }

        // ══════════════════════════════════════════════
        // TAB 2 – COM Port
        // ══════════════════════════════════════════════

        private void button_Renew_Click(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length > 0)
            {
                textBox_COMPort.Text = ports[0];
                label_performance.Text = $"Performance: Tìm thấy {ports.Length} cổng";
            }
            else
            {
                textBox_COMPort.Text = string.Empty;
                label_performance.Text = "Performance: Không tìm thấy cổng COM";
            }
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
                _serialPort.Dispose();
                _serialPort = null;

                button_connect.Text = "Connect";
                label_performance.Text = "Performance: Đã ngắt kết nối";
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox_COMPort.Text))
            {
                MessageBox.Show("Vui lòng nhập cổng COM.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBox_BaudRate.Text, out int baud))
            {
                MessageBox.Show("Baud Rate phải là số.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _serialPort = new SerialPort(textBox_COMPort.Text.Trim(), baud);
                _serialPort.Open();

                button_connect.Text = "Disconnect";
                label_performance.Text = $"Performance: Đã kết nối {textBox_COMPort.Text}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kết nối thất bại: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                label_performance.Text = "Performance: Lỗi kết nối";
            }
        }

        // ══════════════════════════════════════════════
        // TAB 3 – History (Database)
        // ══════════════════════════════════════════════

        /// <summary>Load toàn bộ lịch sử từ DB lên Grid.</summary>
        private void LoadHistory()
        {
            try
            {
                string query = "SELECT Id, Gesture, Confidence, CreatedAt " +
                               "FROM dbo.GestureHistory ORDER BY CreatedAt DESC";

                var list = modify.GestureHistories(query);
                dataGridView1.Rows.Clear();

                foreach (var r in list)
                    dataGridView1.Rows.Add(r.Gesture, r.Confidence.ToString(), r.CreatedAt);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải lịch sử: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Lưu 1 gesture vào DB.</summary>
        private void SaveGestureToDB(string gesture, int confidence)
        {
            try
            {
                string query = $"INSERT INTO dbo.GestureHistory (Gesture, Confidence) " +
                               $"VALUES (N'{gesture}', {confidence})";
                modify.Command(query);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi, không hiện MessageBox trong vòng lặp
                Console.WriteLine($"[SaveGestureToDB] Lỗi: {ex.Message}");
            }
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xóa toàn bộ lịch sử?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    modify.Command("DELETE FROM dbo.GestureHistory");
                    dataGridView1.Rows.Clear();
                    MessageBox.Show("Đã xóa toàn bộ lịch sử.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Xóa thất bại: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Xóa dòng đã chọn?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            try
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    if (row.IsNewRow) continue;

                    string gesture = row.Cells[0].Value?.ToString() ?? "";
                    string confidence = row.Cells[1].Value?.ToString() ?? "0";
                    string time = row.Cells[2].Value?.ToString() ?? "";

                    // Xóa đúng 1 bản ghi khớp gesture + confidence + thời gian
                    modify.Command(
                        $"DELETE TOP(1) FROM dbo.GestureHistory " +
                        $"WHERE Gesture = N'{gesture}' " +
                        $"AND Confidence = {confidence} " +
                        $"AND CONVERT(VARCHAR, CreatedAt, 108) + ' ' + CONVERT(VARCHAR, CreatedAt, 103) = '{time}'");

                    dataGridView1.Rows.Remove(row);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Xóa thất bại: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            using var dlg = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                FileName = $"gesture_history_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("Gesture,Confidence (%),Thời gian");

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;
                    sb.AppendLine(
                        $"{row.Cells[0].Value},{row.Cells[1].Value},{row.Cells[2].Value}");
                }

                System.IO.File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);
                MessageBox.Show("Lưu file thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lưu thất bại: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_print_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tính năng in: kết nối PrintDocument tại đây.", "In",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_logout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn đăng xuất không?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                StopCamera();
                _serialPort?.Close();

                DangNhap dangNhap = new DangNhap();
                dangNhap.Show();
                this.Close();
            }
        }

        // ══════════════════════════════════════════════
        // Gửi dữ liệu lên API Server
        // ══════════════════════════════════════════════

        private static readonly HttpClient _http = new();

        private async Task SendToApiAsync(string gesture, int confidence, string time)
        {
            string apiUrl = string.Empty;
            this.Invoke(() => apiUrl = textBox_APIServer.Text.Trim());
            if (string.IsNullOrWhiteSpace(apiUrl)) return;

            try
            {
                var payload = new StringContent(
                    $"{{\"gesture\":\"{gesture}\",\"confidence\":{confidence},\"time\":\"{time}\"}}",
                    Encoding.UTF8, "application/json");

                await _http.PostAsync(apiUrl + "/gesture", payload);
            }
            catch
            {
                // Bỏ qua lỗi mạng trong vòng lặp
            }
        }

        // ══════════════════════════════════════════════
        // Form closing – dọn tài nguyên
        // ══════════════════════════════════════════════

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            StopCamera();
            _serialPort?.Close();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}