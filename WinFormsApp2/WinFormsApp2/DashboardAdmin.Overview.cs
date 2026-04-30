using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace WinFormsApp2
{
    public partial class DashboardAdmin
    {
        private void ConfigureSessionGrid()
        {
            dgvSessions.Columns.Clear();
            dgvSessions.Columns.Add("#", "#");
            dgvSessions.Columns.Add("Location", "Location");
            dgvSessions.Columns.Add("Gestures", "Gestures");
            dgvSessions.Columns.Add("Confidence", "Conf.");

            dgvSessions.Columns[0].FillWeight = 45;
            dgvSessions.Columns[1].FillWeight = 180;
            dgvSessions.Columns[2].FillWeight = 90;
            dgvSessions.Columns[3].FillWeight = 90;

            dgvSessions.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgvSessions.ColumnHeadersDefaultCellStyle.ForeColor = Color.WhiteSmoke;
            dgvSessions.DefaultCellStyle.BackColor = Color.FromArgb(46, 46, 46);
            dgvSessions.DefaultCellStyle.ForeColor = Color.WhiteSmoke;
            dgvSessions.DefaultCellStyle.SelectionBackColor = Color.FromArgb(70, 70, 70);
            dgvSessions.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvSessions.GridColor = Color.FromArgb(60, 60, 60);
        }

        private void LoadOverviewData()
        {
            try
            {
                using SqlConnection conn = Connection.GetSqlConnection();
                conn.Open();

                LoadKpis(conn);
                LoadPopularGesture(conn);
                LoadSessionList(conn);
                LoadDevices(conn);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không tải được dữ liệu overview: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadKpis(SqlConnection conn)
        {
            string kpiSql = @"
SELECT 
    COUNT(*) AS TotalSessions,
    SUM(COALESCE(TotalGestures, 0)) AS TotalGestures,
    SUM(CASE WHEN EndTime IS NULL THEN 1 ELSE 0 END) AS OpenSessions
FROM dbo.GestureSession;

SELECT TOP (1)
    SessionId,
    AverageConfidence,
    COALESCE(Location, N'Chưa rõ') AS Location
FROM dbo.GestureSession
ORDER BY AverageConfidence DESC, SessionId ASC;

SELECT
    SUM(CASE WHEN Status = 'Online' THEN 1 ELSE 0 END) AS OnlineDevices,
    COUNT(*) AS TotalDevices
FROM dbo.Device;";

            using SqlCommand cmd = new SqlCommand(kpiSql, conn);
            using SqlDataReader reader = cmd.ExecuteReader();

            int totalSessions = 0;
            int totalGestures = 0;
            int openSessions = 0;
            if (reader.Read())
            {
                totalSessions = ToInt(reader["TotalSessions"]);
                totalGestures = ToInt(reader["TotalGestures"]);
                openSessions = ToInt(reader["OpenSessions"]);
            }

            int bestSessionId = 0;
            double bestAvgConfidence = 0;
            string bestLocation = "Chưa có dữ liệu";
            if (reader.NextResult() && reader.Read())
            {
                bestSessionId = ToInt(reader["SessionId"]);
                bestAvgConfidence = ToDouble(reader["AverageConfidence"]);
                bestLocation = reader["Location"]?.ToString() ?? "Chưa có dữ liệu";
            }

            int onlineDevices = 0;
            int totalDevices = 0;
            if (reader.NextResult() && reader.Read())
            {
                onlineDevices = ToInt(reader["OnlineDevices"]);
                totalDevices = ToInt(reader["TotalDevices"]);
            }

            lblKpiSessionValue.Text = totalSessions.ToString();
            lblKpiSessionDetail.Text = $"{openSessions} chưa kết thúc";
            lblKpiGesturesValue.Text = totalGestures.ToString();
            lblKpiGesturesDetail.Text = "TotalGestures tổng cộng";
            lblKpiAvgValue.Text = $"{bestAvgConfidence:0.#}%";
            lblKpiAvgDetail.Text = bestSessionId > 0 ? $"session #{bestSessionId} - {bestLocation}" : "session #-- - Chưa có";
            lblKpiDeviceValue.Text = $"{onlineDevices}/{totalDevices}";
            lblKpiDeviceDetail.Text = totalDevices == 0 ? "Chưa có thiết bị" : $"{onlineDevices} thiết bị online";
        }

        private void LoadPopularGesture(SqlConnection conn)
        {
            string popularSql = @"
WITH TotalCte AS
(
    SELECT COUNT(*) AS TotalRows FROM dbo.GestureHistory
)
SELECT TOP (1)
    r.Gesture,
    COUNT(*) AS Cnt,
    MIN(COALESCE(r.Confidence, 0)) AS MinConfidence,
    MAX(COALESCE(r.Confidence, 0)) AS MaxConfidence,
    MIN(COALESCE(r.Quality, 0)) AS MinQuality,
    MAX(COALESCE(r.Quality, 0)) AS MaxQuality,
    t.TotalRows
FROM dbo.GestureHistory r
CROSS JOIN TotalCte t
GROUP BY r.Gesture, t.TotalRows
ORDER BY Cnt DESC, r.Gesture ASC;";

            using SqlCommand cmd = new SqlCommand(popularSql, conn);
            using SqlDataReader reader = cmd.ExecuteReader();

            if (!reader.Read())
            {
                lblPopularGestureName.Text = "Chưa có dữ liệu";
                lblPopularPercent.Text = "0%";
                lblConfidenceRange.Text = "Confidence: chưa có";
                lblQualityRange.Text = "Quality: chưa có";
                return;
            }

            string gesture = reader["Gesture"]?.ToString() ?? "Unknown";
            int cnt = ToInt(reader["Cnt"]);
            int totalRows = Math.Max(1, ToInt(reader["TotalRows"]));
            double percent = cnt * 100.0 / totalRows;

            int minConf = ToInt(reader["MinConfidence"]);
            int maxConf = ToInt(reader["MaxConfidence"]);
            int minQuality = ToInt(reader["MinQuality"]);
            int maxQuality = ToInt(reader["MaxQuality"]);

            lblPopularGestureName.Text = gesture;
            lblPopularPercent.Text = $"{percent:0.#}%";
            lblConfidenceRange.Text = $"Confidence: {minConf}-{maxConf}";
            lblQualityRange.Text = $"Quality: {minQuality}-{maxQuality}";
        }

        private void LoadSessionList(SqlConnection conn)
        {
            dgvSessions.Rows.Clear();

            string sessionSql = @"
SELECT TOP (6)
    SessionId,
    COALESCE(Location, N'Chưa rõ') AS Location,
    COALESCE(TotalGestures, 0) AS TotalGestures,
    COALESCE(AverageConfidence, 0) AS AverageConfidence
FROM dbo.GestureSession
ORDER BY StartTime DESC, SessionId DESC;";

            using SqlCommand cmd = new SqlCommand(sessionSql, conn);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                dgvSessions.Rows.Add(
                    ToInt(reader["SessionId"]),
                    reader["Location"]?.ToString() ?? "Chưa rõ",
                    ToInt(reader["TotalGestures"]),
                    $"{ToDouble(reader["AverageConfidence"]):0.#}"
                );
            }
        }

        private void LoadDevices(SqlConnection conn)
        {
            flowDevices.Controls.Clear();

            string deviceSql = @"
SELECT
    COALESCE(DeviceName, N'Unknown') AS DeviceName,
    COALESCE(SerialPort, '-') AS SerialPort,
    COALESCE(BaudRate, 0) AS BaudRate,
    COALESCE(Status, 'Offline') AS Status,
    LastConnected
FROM dbo.Device
ORDER BY DeviceId ASC;";

            DateTime latestConnected = DateTime.MinValue;
            using SqlCommand cmd = new SqlCommand(deviceSql, conn);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string deviceName = reader["DeviceName"]?.ToString() ?? "Unknown";
                string serialPort = reader["SerialPort"]?.ToString() ?? "-";
                int baudRate = ToInt(reader["BaudRate"]);
                string status = reader["Status"]?.ToString() ?? "Offline";
                DateTime? lastConnected = reader["LastConnected"] == DBNull.Value ? null : Convert.ToDateTime(reader["LastConnected"]);

                if (lastConnected.HasValue && lastConnected.Value > latestConnected)
                {
                    latestConnected = lastConnected.Value;
                }

                flowDevices.Controls.Add(CreateDeviceCard(deviceName, serialPort, baudRate, status));
            }

            if (flowDevices.Controls.Count == 0)
            {
                Label emptyLabel = new Label
                {
                    AutoSize = true,
                    ForeColor = Color.Gainsboro,
                    Text = "Chưa có thiết bị trong hệ thống.",
                    Margin = new Padding(8)
                };
                flowDevices.Controls.Add(emptyLabel);
            }
            else if (latestConnected > DateTime.MinValue)
            {
                labelSectionDevices.Text = $"Trạng thái thiết bị - Last connected: {latestConnected:dd/MM/yyyy HH:mm}";
            }
        }

        private Control CreateDeviceCard(string deviceName, string serialPort, int baudRate, string status)
        {
            Panel card = new Panel
            {
                BackColor = _cardBackColor,
                Width = 270,
                Height = 95,
                Margin = new Padding(8)
            };

            Label name = new Label
            {
                AutoSize = true,
                ForeColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold),
                Location = new Point(12, 10),
                Text = deviceName
            };

            Label detail = new Label
            {
                AutoSize = true,
                ForeColor = Color.Gainsboro,
                Font = new Font("Segoe UI", 9),
                Location = new Point(12, 37),
                Text = $"{serialPort} - {baudRate} baud"
            };

            Label id = new Label
            {
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 8.5F),
                Location = new Point(12, 58),
                Text = status.Equals("Online", StringComparison.OrdinalIgnoreCase) ? "Đang hoạt động" : "Chưa hoạt động"
            };

            Label statusLabel = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Width = 76,
                Height = 26,
                Font = new Font("Segoe UI Semibold", 9, FontStyle.Bold),
                Location = new Point(180, 12),
                Text = status
            };

            bool isOnline = status.Equals("Online", StringComparison.OrdinalIgnoreCase);
            statusLabel.BackColor = isOnline ? Color.FromArgb(48, 112, 86) : Color.FromArgb(100, 70, 58);
            statusLabel.ForeColor = isOnline ? Color.FromArgb(125, 255, 188) : Color.FromArgb(255, 190, 160);

            card.Controls.Add(name);
            card.Controls.Add(detail);
            card.Controls.Add(id);
            card.Controls.Add(statusLabel);
            return card;
        }
    }
}
