using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace WinFormsApp2
{
    public partial class DashboardAdmin
    {
        private void EnsureDeviceUi()
        {
            if (_deviceRoot != null) return;

            _deviceRoot = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(35, 35, 35),
                Visible = false,
                Padding = new Padding(18, 16, 18, 16)
            };
            panelContent.Controls.Add(_deviceRoot);
            _deviceRoot.BringToFront();

            Label title = new Label
            {
                Text = "Devices",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.WhiteSmoke,
                Location = new Point(12, 10)
            };
            _deviceRoot.Controls.Add(title);

            Button refreshBtn = new Button
            {
                Text = "Refresh status",
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.WhiteSmoke,
                BackColor = Color.FromArgb(50, 50, 50),
                Size = new Size(130, 34),
                Location = new Point(560, 8)
            };
            refreshBtn.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);
            refreshBtn.Click += (_, __) => LoadDeviceScreenData();
            _deviceRoot.Controls.Add(refreshBtn);

            Button addBtn = new Button
            {
                Text = "+ Thêm thiết bị",
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.WhiteSmoke,
                BackColor = Color.FromArgb(50, 50, 50),
                Size = new Size(120, 34),
                Location = new Point(700, 8)
            };
            addBtn.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);
            addBtn.Click += (_, __) => MessageBox.Show("Bạn có thể thêm form tạo thiết bị ở bước tiếp theo.", "Thông báo");
            _deviceRoot.Controls.Add(addBtn);

            Panel kpiRow = new Panel { Location = new Point(12, 52), Size = new Size(900, 86) };
            _deviceRoot.Controls.Add(kpiRow);

            (_deviceTotalLabel, _) = CreateSessionKpiCard(kpiRow, "Tổng thiết bị", "0", "Arduino x0", 0);
            (_deviceOnlineLabel, _) = CreateSessionKpiCard(kpiRow, "Online", "0", "Tất cả offline", 220);
            (_deviceBaudLabel, _) = CreateSessionKpiCard(kpiRow, "Baud Rate", "0", "Cấu hình chung", 440);
            (_deviceLastSeenLabel, _) = CreateSessionKpiCard(kpiRow, "Last Seen", "--", "Chưa có", 660);

            Panel alertPanel = new Panel { Location = new Point(12, 150), Size = new Size(900, 52), BackColor = Color.FromArgb(74, 48, 48) };
            _deviceAlertLabel = new Label { AutoSize = true, ForeColor = Color.FromArgb(255, 222, 222), Location = new Point(12, 16), Text = "Đang tải trạng thái..." };
            alertPanel.Controls.Add(_deviceAlertLabel);
            _deviceRoot.Controls.Add(alertPanel);

            Panel conflictPanel = new Panel { Location = new Point(12, 210), Size = new Size(900, 52), BackColor = Color.FromArgb(78, 62, 33) };
            _deviceConflictLabel = new Label { AutoSize = true, ForeColor = Color.FromArgb(255, 235, 185), Location = new Point(12, 16), Text = "Kiểm tra xung đột serial..." };
            conflictPanel.Controls.Add(_deviceConflictLabel);
            _deviceRoot.Controls.Add(conflictPanel);

            _deviceCardsFlow = new FlowLayoutPanel
            {
                Location = new Point(12, 272),
                Size = new Size(900, 352),
                AutoScroll = true
            };
            _deviceRoot.Controls.Add(_deviceCardsFlow);
        }

        private void LoadDeviceScreenData()
        {
            if (_deviceCardsFlow == null || _deviceTotalLabel == null || _deviceOnlineLabel == null || _deviceBaudLabel == null || _deviceLastSeenLabel == null || _deviceAlertLabel == null || _deviceConflictLabel == null)
            {
                return;
            }

            _devices.Clear();
            try
            {
                using SqlConnection conn = Connection.GetSqlConnection();
                conn.Open();
                const string sql = @"
SELECT DeviceId, COALESCE(DeviceName,N'Unknown') AS DeviceName, COALESCE(SerialPort,'-') AS SerialPort,
       COALESCE(BaudRate,0) AS BaudRate, COALESCE(Status,'Offline') AS Status, LastConnected, CreatedAt
FROM dbo.Device
ORDER BY DeviceId ASC;";
                using SqlCommand cmd = new SqlCommand(sql, conn);
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    _devices.Add(new DeviceInfo
                    {
                        DeviceId = ToInt(reader["DeviceId"]),
                        DeviceName = reader["DeviceName"]?.ToString() ?? "Unknown",
                        SerialPort = reader["SerialPort"]?.ToString() ?? "-",
                        BaudRate = ToInt(reader["BaudRate"]),
                        Status = reader["Status"]?.ToString() ?? "Offline",
                        LastConnected = reader["LastConnected"] == DBNull.Value ? null : Convert.ToDateTime(reader["LastConnected"]),
                        CreatedAt = reader["CreatedAt"] == DBNull.Value ? null : Convert.ToDateTime(reader["CreatedAt"])
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không tải được devices: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            int total = _devices.Count;
            int online = _devices.Count(d => d.Status.Equals("Online", StringComparison.OrdinalIgnoreCase));
            int commonBaud = _devices.GroupBy(d => d.BaudRate).OrderByDescending(g => g.Count()).Select(g => g.Key).FirstOrDefault();
            DateTime? latest = _devices.Where(d => d.LastConnected.HasValue).Select(d => d.LastConnected!.Value).DefaultIfEmpty().Max();

            _deviceTotalLabel.Text = total.ToString();
            _deviceOnlineLabel.Text = online.ToString();
            _deviceBaudLabel.Text = commonBaud > 0 ? commonBaud.ToString() : "--";
            _deviceLastSeenLabel.Text = latest.HasValue ? latest.Value.ToString("dd/MM HH:mm") : "--";

            UpdateDeviceKpiDetails(total, online, latest);
            UpdateDeviceAlerts(total, online);

            _deviceCardsFlow.Controls.Clear();
            foreach (DeviceInfo device in _devices)
            {
                _deviceCardsFlow.Controls.Add(CreateDeviceDetailCard(device));
            }
        }

        private void UpdateDeviceKpiDetails(int total, int online, DateTime? latest)
        {
            Control? row = _deviceTotalLabel?.Parent?.Parent;
            if (row == null || row.Controls.Count < 4) return;
            Label? totalDetail = row.Controls[0].Controls.OfType<Label>().LastOrDefault();
            Label? onlineDetail = row.Controls[1].Controls.OfType<Label>().LastOrDefault();
            Label? baudDetail = row.Controls[2].Controls.OfType<Label>().LastOrDefault();
            Label? lastSeenDetail = row.Controls[3].Controls.OfType<Label>().LastOrDefault();
            if (totalDetail != null) totalDetail.Text = $"Arduino x{total}";
            if (onlineDetail != null) onlineDetail.Text = online == 0 ? "Tất cả offline" : $"{online} thiết bị online";
            if (baudDetail != null) baudDetail.Text = total > 0 ? "Cả 2 thiết bị" : "Chưa có thiết bị";
            if (lastSeenDetail != null) lastSeenDetail.Text = latest.HasValue ? "Cả 2 cùng lúc" : "Chưa có dữ liệu";
        }

        private void UpdateDeviceAlerts(int total, int online)
        {
            if (_deviceAlertLabel == null || _deviceConflictLabel == null) return;

            if (total == 0)
            {
                _deviceAlertLabel.Text = "Chưa có thiết bị trong hệ thống.";
            }
            else if (online == 0)
            {
                _deviceAlertLabel.Text = $"Cả {total} thiết bị đang Offline - kiểm tra cổng COM và nguồn điện Arduino.";
            }
            else
            {
                _deviceAlertLabel.Text = $"{online}/{total} thiết bị đang hoạt động bình thường.";
            }

            var conflictPort = _devices
                .Where(d => !string.IsNullOrWhiteSpace(d.SerialPort) && d.SerialPort != "-")
                .GroupBy(d => d.SerialPort, StringComparer.OrdinalIgnoreCase)
                .FirstOrDefault(g => g.Count() > 1);

            if (conflictPort != null)
            {
                _deviceConflictLabel.Text = $"Conflict Serial Port: có {conflictPort.Count()} thiết bị dùng {conflictPort.Key}.";
            }
            else
            {
                _deviceConflictLabel.Text = "Không phát hiện xung đột serial port.";
            }
        }

        private Control CreateDeviceDetailCard(DeviceInfo device)
        {
            Panel card = new Panel
            {
                BackColor = Color.FromArgb(42, 42, 42),
                Width = 435,
                Height = 260,
                Margin = new Padding(0, 0, 12, 12),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label name = new Label { Text = device.DeviceName, ForeColor = Color.WhiteSmoke, AutoSize = true, Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold), Location = new Point(18, 14) };
            Label id = new Label { Text = $"Device ID: {device.DeviceId}", ForeColor = Color.Gainsboro, AutoSize = true, Location = new Point(20, 42) };
            Label status = new Label
            {
                Text = device.Status,
                AutoSize = true,
                BackColor = device.Status.Equals("Online", StringComparison.OrdinalIgnoreCase) ? Color.FromArgb(52, 106, 84) : Color.FromArgb(103, 67, 58),
                ForeColor = Color.WhiteSmoke,
                Padding = new Padding(10, 2, 10, 2),
                Location = new Point(330, 18)
            };
            card.Controls.Add(name);
            card.Controls.Add(id);
            card.Controls.Add(status);

            card.Controls.Add(CreateDeviceStatBox("SerialPort", device.SerialPort, 18, 74, IsSerialConflict(device.SerialPort) ? Color.IndianRed : Color.WhiteSmoke));
            card.Controls.Add(CreateDeviceStatBox("BaudRate", device.BaudRate.ToString(), 155, 74, Color.WhiteSmoke));
            card.Controls.Add(CreateDeviceStatBox("Status", device.Status, 18, 128, device.Status.Equals("Online", StringComparison.OrdinalIgnoreCase) ? Color.MediumSeaGreen : Color.IndianRed));
            card.Controls.Add(CreateDeviceStatBox("Created At", device.CreatedAt?.ToString("dd/MM HH:mm") ?? "--", 155, 128, Color.WhiteSmoke));
            card.Controls.Add(CreateDeviceStatBox("Last Connected", device.LastConnected?.ToString("dd/MM/yyyy HH:mm:ss.fff") ?? "--", 18, 182, Color.WhiteSmoke, 274));

            return card;
        }

        private Control CreateDeviceStatBox(string title, string value, int x, int y, Color valueColor, int width = 120)
        {
            Panel box = new Panel { BackColor = Color.FromArgb(36, 36, 36), Location = new Point(x, y), Size = new Size(width, 48) };
            Label t = new Label { Text = title, AutoSize = true, ForeColor = Color.Silver, Location = new Point(8, 5) };
            Label v = new Label { Text = value, AutoSize = true, ForeColor = valueColor, Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold), Location = new Point(8, 22) };
            box.Controls.Add(t);
            box.Controls.Add(v);
            return box;
        }

        private bool IsSerialConflict(string serialPort)
        {
            if (string.IsNullOrWhiteSpace(serialPort) || serialPort == "-") return false;
            return _devices.Count(d => d.SerialPort.Equals(serialPort, StringComparison.OrdinalIgnoreCase)) > 1;
        }
    }
}
