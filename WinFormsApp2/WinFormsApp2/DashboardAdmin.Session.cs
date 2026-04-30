using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace WinFormsApp2
{
    public partial class DashboardAdmin
    {
        private void EnsureSessionUi()
        {
            if (_sessionRoot != null) return;

            _sessionRoot = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(35, 35, 35),
                Visible = false,
                Padding = new Padding(18, 16, 18, 16)
            };
            panelContent.Controls.Add(_sessionRoot);
            _sessionRoot.BringToFront();

            Label title = new Label
            {
                Text = "Sessions",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Color.WhiteSmoke,
                Location = new Point(12, 10)
            };
            _sessionRoot.Controls.Add(title);

            Button exportBtn = new Button
            {
                Text = "Export CSV",
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.WhiteSmoke,
                BackColor = Color.FromArgb(50, 50, 50),
                Size = new Size(110, 34),
                Location = new Point(590, 8)
            };
            exportBtn.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);
            exportBtn.Click += (_, __) => ExportSessionsCsv();
            _sessionRoot.Controls.Add(exportBtn);

            Panel kpiRow = new Panel
            {
                Location = new Point(12, 52),
                Size = new Size(900, 86)
            };
            _sessionRoot.Controls.Add(kpiRow);

            (_sessionCountLabel, _) = CreateSessionKpiCard(kpiRow, "Tổng sessions", "0", "Device #", 0);
            (_sessionOpenLabel, _) = CreateSessionKpiCard(kpiRow, "Đang mở", "0", "Session mở", 220);
            (_sessionGesturesLabel, _) = CreateSessionKpiCard(kpiRow, "Tổng gestures", "0", "Avg/session", 440);
            (_sessionAvgLabel, _) = CreateSessionKpiCard(kpiRow, "Avg confidence", "0", "Tốt nhất", 660);

            _sessionSearchBox = new TextBox
            {
                Location = new Point(12, 152),
                Size = new Size(350, 31),
                Font = new Font("Segoe UI", 11F),
                PlaceholderText = "Tìm location..."
            };
            _sessionSearchBox.TextChanged += (_, __) => RenderSessionSection();
            _sessionRoot.Controls.Add(_sessionSearchBox);

            Button allBtn = CreateFilterButton("Tất cả", 380, "All");
            Button openBtn = CreateFilterButton("Đang mở", 460, "Open");
            Button closedBtn = CreateFilterButton("Đã đóng", 550, "Closed");
            _sessionRoot.Controls.Add(allBtn);
            _sessionRoot.Controls.Add(openBtn);
            _sessionRoot.Controls.Add(closedBtn);

            _sessionCardsFlow = new FlowLayoutPanel
            {
                Location = new Point(12, 194),
                Size = new Size(560, 430),
                AutoScroll = true
            };
            _sessionRoot.Controls.Add(_sessionCardsFlow);

            Panel summaryPanel = new Panel
            {
                Location = new Point(585, 194),
                Size = new Size(327, 430),
                BackColor = Color.FromArgb(45, 45, 45)
            };
            _sessionRoot.Controls.Add(summaryPanel);

            Label locationTitle = CreateSummaryTitle("Gestures theo location", 12, 12);
            _locationSummaryFlow = new FlowLayoutPanel { Location = new Point(12, 40), Size = new Size(300, 112), FlowDirection = FlowDirection.TopDown, WrapContents = false };
            summaryPanel.Controls.Add(locationTitle);
            summaryPanel.Controls.Add(_locationSummaryFlow);

            Label confidenceTitle = CreateSummaryTitle("So sánh sessions", 12, 162);
            _confidenceSummaryFlow = new FlowLayoutPanel { Location = new Point(12, 190), Size = new Size(300, 132), FlowDirection = FlowDirection.TopDown, WrapContents = false };
            summaryPanel.Controls.Add(confidenceTitle);
            summaryPanel.Controls.Add(_confidenceSummaryFlow);

            Label speedTitle = CreateSummaryTitle("Tốc độ gesture/giây", 12, 332);
            _speedSummaryFlow = new FlowLayoutPanel { Location = new Point(12, 360), Size = new Size(300, 60), FlowDirection = FlowDirection.TopDown, WrapContents = false };
            summaryPanel.Controls.Add(speedTitle);
            summaryPanel.Controls.Add(_speedSummaryFlow);
        }

        private Button CreateFilterButton(string text, int x, string filterValue)
        {
            Button btn = new Button
            {
                Text = text,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.WhiteSmoke,
                BackColor = Color.FromArgb(50, 50, 50),
                Size = new Size(72, 31),
                Location = new Point(x, 152)
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(85, 85, 85);
            btn.Click += (_, __) =>
            {
                _sessionFilter = filterValue;
                RenderSessionSection();
            };
            return btn;
        }

        private Label CreateSummaryTitle(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                ForeColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Location = new Point(x, y)
            };
        }

        private void LoadSessionData()
        {
            try
            {
                _sessions.Clear();
                using SqlConnection conn = Connection.GetSqlConnection();
                conn.Open();

                const string sql = @"
SELECT SessionId, DeviceId, StartTime, EndTime, COALESCE(TotalGestures,0) AS TotalGestures,
       COALESCE(AverageConfidence,0) AS AverageConfidence, COALESCE(Location, N'Chưa rõ') AS Location
FROM dbo.GestureSession
ORDER BY StartTime DESC, SessionId DESC;";

                using SqlCommand cmd = new SqlCommand(sql, conn);
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    _sessions.Add(new SessionInfo
                    {
                        SessionId = ToInt(reader["SessionId"]),
                        DeviceId = ToInt(reader["DeviceId"]),
                        StartTime = Convert.ToDateTime(reader["StartTime"]),
                        EndTime = reader["EndTime"] == DBNull.Value ? null : Convert.ToDateTime(reader["EndTime"]),
                        TotalGestures = ToInt(reader["TotalGestures"]),
                        AverageConfidence = ToDouble(reader["AverageConfidence"]),
                        Location = reader["Location"]?.ToString() ?? "Chưa rõ"
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không tải được sessions: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            RenderSessionSection();
        }

        private void RenderSessionSection()
        {
            if (_sessionCardsFlow == null || _locationSummaryFlow == null || _confidenceSummaryFlow == null || _speedSummaryFlow == null ||
                _sessionCountLabel == null || _sessionOpenLabel == null || _sessionGesturesLabel == null || _sessionAvgLabel == null)
            {
                return;
            }

            IEnumerable<SessionInfo> filtered = _sessions;
            string keyword = _sessionSearchBox?.Text.Trim().ToLowerInvariant() ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filtered = filtered.Where(s => s.Location.ToLowerInvariant().Contains(keyword));
            }
            if (_sessionFilter == "Open")
            {
                filtered = filtered.Where(s => !s.EndTime.HasValue);
            }
            else if (_sessionFilter == "Closed")
            {
                filtered = filtered.Where(s => s.EndTime.HasValue);
            }

            List<SessionInfo> list = filtered.ToList();

            int totalSessions = _sessions.Count;
            int openCount = _sessions.Count(s => !s.EndTime.HasValue);
            int totalGestures = _sessions.Sum(s => s.TotalGestures);
            double avgConfidence = _sessions.Count > 0 ? _sessions.Average(s => s.AverageConfidence) : 0;
            double avgGesturesPerSession = _sessions.Count > 0 ? _sessions.Average(s => s.TotalGestures) : 0;
            SessionInfo? best = _sessions.OrderByDescending(s => s.AverageConfidence).FirstOrDefault();

            _sessionCountLabel.Text = totalSessions.ToString();
            _sessionOpenLabel.Text = openCount.ToString();
            _sessionGesturesLabel.Text = totalGestures.ToString();
            _sessionAvgLabel.Text = $"{avgConfidence:0.#}";

            UpdateKpiDetails(openCount, avgGesturesPerSession, best);

            _sessionCardsFlow.Controls.Clear();
            foreach (SessionInfo s in list)
            {
                _sessionCardsFlow.Controls.Add(CreateSessionCard(s));
            }

            _locationSummaryFlow.Controls.Clear();
            foreach (var item in _sessions.GroupBy(x => x.Location).Select(g => new { Location = g.Key, Gestures = g.Sum(x => x.TotalGestures) }).OrderByDescending(x => x.Gestures).Take(4))
            {
                _locationSummaryFlow.Controls.Add(CreateSummaryProgressRow(item.Location, item.Gestures.ToString(), item.Gestures / Math.Max(1d, _sessions.Max(x => x.TotalGestures)), Color.MediumPurple));
            }

            _confidenceSummaryFlow.Controls.Clear();
            foreach (SessionInfo s in _sessions.OrderByDescending(x => x.AverageConfidence).Take(4))
            {
                _confidenceSummaryFlow.Controls.Add(CreateSummaryProgressRow($"#{s.SessionId} - {s.Location}", $"{s.AverageConfidence:0.#}", s.AverageConfidence / 100d, Color.FromArgb(43, 196, 132)));
            }

            _speedSummaryFlow.Controls.Clear();
            foreach (SessionInfo s in _sessions.Take(3))
            {
                double seconds = Math.Max(1, ((s.EndTime ?? DateTime.Now) - s.StartTime).TotalSeconds);
                double rate = s.TotalGestures / seconds;
                _speedSummaryFlow.Controls.Add(CreateSpeedRow($"#{s.SessionId}", $"{rate:0.##} /s"));
            }
        }

        private void UpdateKpiDetails(int openCount, double avgGesturesPerSession, SessionInfo? best)
        {
            Control? row = _sessionCountLabel?.Parent?.Parent;
            if (row == null) return;

            if (row.Controls.Count >= 4)
            {
                Label? sessionDetail = row.Controls[0].Controls.OfType<Label>().LastOrDefault();
                Label? openDetail = row.Controls[1].Controls.OfType<Label>().LastOrDefault();
                Label? gestureDetail = row.Controls[2].Controls.OfType<Label>().LastOrDefault();
                Label? avgDetail = row.Controls[3].Controls.OfType<Label>().LastOrDefault();
                if (sessionDetail != null) sessionDetail.Text = _sessions.Count > 0 ? $"Device #{_sessions[0].DeviceId}" : "Device #--";
                if (openDetail != null) openDetail.Text = openCount > 0 ? $"Session #{_sessions.FirstOrDefault(s => !s.EndTime.HasValue)?.SessionId} - chưa EndTime" : "Không còn session mở";
                if (gestureDetail != null) gestureDetail.Text = $"Avg {avgGesturesPerSession:0.#} / session";
                if (avgDetail != null) avgDetail.Text = best != null ? $"Tốt nhất: {best.AverageConfidence:0.#} (#{best.SessionId})" : "Tốt nhất: --";
            }
        }

        private Control CreateSessionCard(SessionInfo s)
        {
            Panel card = new Panel
            {
                BackColor = Color.FromArgb(42, 42, 42),
                Width = 525,
                Height = 124,
                Margin = new Padding(0, 0, 0, 10),
                Padding = new Padding(12)
            };
            card.BorderStyle = BorderStyle.FixedSingle;

            Label id = new Label { Text = $"#{s.SessionId}", ForeColor = Color.Gainsboro, AutoSize = true, Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold), Location = new Point(10, 10) };
            Label location = new Label { Text = s.Location, ForeColor = Color.WhiteSmoke, AutoSize = true, Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold), Location = new Point(60, 8) };
            Label time = new Label { Text = $"{s.StartTime:dd/MM/yyyy HH:mm:ss}", ForeColor = Color.Silver, AutoSize = true, Location = new Point(62, 40) };
            Label gestures = new Label { Text = $"{s.TotalGestures} gestures", ForeColor = Color.Gainsboro, AutoSize = true, Location = new Point(62, 65) };
            Label conf = new Label { Text = $"{s.AverageConfidence:0.#} confidence", ForeColor = Color.WhiteSmoke, AutoSize = true, Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold), Location = new Point(360, 44) };
            Label status = new Label
            {
                Text = s.EndTime.HasValue ? "Đã đóng" : "Đang mở",
                BackColor = s.EndTime.HasValue ? Color.FromArgb(90, 90, 90) : Color.FromArgb(108, 88, 41),
                ForeColor = Color.WhiteSmoke,
                AutoSize = true,
                Padding = new Padding(8, 2, 8, 2),
                Location = new Point(62, 90)
            };
            card.Controls.Add(id);
            card.Controls.Add(location);
            card.Controls.Add(time);
            card.Controls.Add(gestures);
            card.Controls.Add(conf);
            card.Controls.Add(status);
            return card;
        }

        private Control CreateSummaryProgressRow(string title, string value, double ratio, Color accent)
        {
            Panel row = new Panel { Width = 295, Height = 30 };
            Label titleLabel = new Label { Text = title, ForeColor = Color.Gainsboro, AutoSize = true, Location = new Point(0, 6) };
            ProgressBar bar = new ProgressBar { Location = new Point(130, 7), Size = new Size(120, 15), Value = Math.Max(0, Math.Min(100, (int)(ratio * 100))) };
            Label valueLabel = new Label { Text = value, ForeColor = accent, AutoSize = true, Location = new Point(258, 6) };
            row.Controls.Add(titleLabel);
            row.Controls.Add(bar);
            row.Controls.Add(valueLabel);
            return row;
        }

        private Control CreateSpeedRow(string title, string value)
        {
            Panel row = new Panel { Width = 295, Height = 24 };
            Label left = new Label { Text = title, AutoSize = true, ForeColor = Color.Gainsboro, Location = new Point(0, 4) };
            Label right = new Label { Text = value, AutoSize = true, ForeColor = Color.WhiteSmoke, BackColor = Color.FromArgb(72, 98, 92), Padding = new Padding(7, 2, 7, 2), Location = new Point(210, 1) };
            row.Controls.Add(left);
            row.Controls.Add(right);
            return row;
        }

        private void ExportSessionsCsv()
        {
            using SaveFileDialog save = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = $"sessions_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };
            if (save.ShowDialog() != DialogResult.OK) return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SessionId,DeviceId,Location,StartTime,EndTime,TotalGestures,AverageConfidence");
            foreach (SessionInfo s in _sessions)
            {
                sb.AppendLine($"{s.SessionId},{s.DeviceId},\"{s.Location}\",{s.StartTime:yyyy-MM-dd HH:mm:ss},{s.EndTime:yyyy-MM-dd HH:mm:ss},{s.TotalGestures},{s.AverageConfidence:0.##}");
            }
            File.WriteAllText(save.FileName, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("Đã xuất CSV sessions.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
