using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class DashboardAdmin
    {
        private (Label valueLabel, Label detailLabel) CreateSessionKpiCard(Control parent, string title, string value, string detail, int x)
        {
            Panel card = new Panel
            {
                BackColor = Color.FromArgb(45, 45, 45),
                Location = new Point(x, 0),
                Size = new Size(205, 84)
            };
            parent.Controls.Add(card);

            Label titleLabel = new Label { Text = title, ForeColor = Color.Gainsboro, AutoSize = true, Location = new Point(12, 10) };
            Label valueLabel = new Label { Text = value, ForeColor = Color.WhiteSmoke, AutoSize = true, Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold), Location = new Point(12, 28) };
            Label detailLabel = new Label { Text = detail, ForeColor = Color.Silver, AutoSize = true, Location = new Point(12, 60) };
            card.Controls.Add(titleLabel);
            card.Controls.Add(valueLabel);
            card.Controls.Add(detailLabel);
            return (valueLabel, detailLabel);
        }

        private static int ToInt(object value)
        {
            if (value == DBNull.Value || value == null)
            {
                return 0;
            }

            return Convert.ToInt32(value);
        }

        private static double ToDouble(object value)
        {
            if (value == DBNull.Value || value == null)
            {
                return 0;
            }

            return Convert.ToDouble(value);
        }

        private class SessionInfo
        {
            public int SessionId { get; set; }
            public int DeviceId { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime? EndTime { get; set; }
            public int TotalGestures { get; set; }
            public double AverageConfidence { get; set; }
            public string Location { get; set; } = string.Empty;
        }

        private class DeviceInfo
        {
            public int DeviceId { get; set; }
            public string DeviceName { get; set; } = string.Empty;
            public string SerialPort { get; set; } = string.Empty;
            public int BaudRate { get; set; }
            public string Status { get; set; } = "Offline";
            public DateTime? LastConnected { get; set; }
            public DateTime? CreatedAt { get; set; }
        }

        private class AccountInfo
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Role { get; set; } = "User";
        }
    }
}
