using System;
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
        private void EnsureAccountUi()
        {
            if (_accountRoot != null) return;

            _accountRoot = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(35, 35, 35),
                Visible = false,
                Padding = new Padding(18, 16, 18, 16)
            };
            panelContent.Controls.Add(_accountRoot);
            _accountRoot.BringToFront();

            Label title = new Label { Text = "Accounts", AutoSize = true, Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold), ForeColor = Color.WhiteSmoke, Location = new Point(12, 10) };
            _accountRoot.Controls.Add(title);

            Button exportBtn = new Button { Text = "Export", FlatStyle = FlatStyle.Flat, ForeColor = Color.WhiteSmoke, BackColor = Color.FromArgb(50, 50, 50), Size = new Size(90, 34), Location = new Point(620, 8) };
            exportBtn.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);
            exportBtn.Click += (_, __) => ExportAccountsCsv();
            _accountRoot.Controls.Add(exportBtn);

            Button addBtn = new Button { Text = "+ Tạo tài khoản", FlatStyle = FlatStyle.Flat, ForeColor = Color.WhiteSmoke, BackColor = Color.FromArgb(83, 74, 183), Size = new Size(130, 34), Location = new Point(720, 8) };
            addBtn.FlatAppearance.BorderColor = Color.FromArgb(140, 130, 220);
            addBtn.Click += (_, __) => MessageBox.Show("Bạn có thể nối form tạo tài khoản ở bước tiếp theo.", "Thông báo");
            _accountRoot.Controls.Add(addBtn);

            Panel kpiRow = new Panel { Location = new Point(12, 52), Size = new Size(900, 86) };
            _accountRoot.Controls.Add(kpiRow);
            (_accountTotalKpi, _) = CreateSessionKpiCard(kpiRow, "Tổng tài khoản", "0", "Tất cả active", 0);
            (_accountAdminKpi, _) = CreateSessionKpiCard(kpiRow, "Admin", "0", "--", 220);
            (_accountUserKpi, _) = CreateSessionKpiCard(kpiRow, "User thường", "0", "--", 440);
            (_accountSecurityKpi, _) = CreateSessionKpiCard(kpiRow, "Bảo mật", "Yếu", "Plaintext password", 660);

            Panel alert1 = new Panel { Location = new Point(12, 148), Size = new Size(900, 52), BackColor = Color.FromArgb(78, 45, 45) };
            alert1.Controls.Add(new Label { AutoSize = true, ForeColor = Color.FromArgb(255, 220, 220), Location = new Point(12, 16), Text = "Mật khẩu đang lưu plaintext. Cần hash bằng bcrypt/Argon2 trước production." });
            _accountRoot.Controls.Add(alert1);

            Panel alert2 = new Panel { Location = new Point(12, 206), Size = new Size(900, 52), BackColor = Color.FromArgb(79, 62, 35) };
            alert2.Controls.Add(new Label { AutoSize = true, ForeColor = Color.FromArgb(255, 236, 190), Location = new Point(12, 16), Text = "Role nên chuẩn hóa admin/user để phân quyền rõ ràng." });
            _accountRoot.Controls.Add(alert2);

            Panel left = new Panel { Location = new Point(12, 268), Size = new Size(590, 356), BackColor = Color.FromArgb(40, 40, 40) };
            _accountRoot.Controls.Add(left);
            Panel right = new Panel { Location = new Point(614, 268), Size = new Size(298, 356), BackColor = Color.FromArgb(40, 40, 40) };
            _accountRoot.Controls.Add(right);

            _accountSearchBox = new TextBox { Location = new Point(10, 10), Size = new Size(300, 30), Font = new Font("Segoe UI", 10.5F), PlaceholderText = "Tìm tài khoản..." };
            _accountSearchBox.TextChanged += (_, __) => RenderAccountList();
            left.Controls.Add(_accountSearchBox);
            left.Controls.Add(CreateAccountFilterButton("Tất cả", 320, "All"));
            left.Controls.Add(CreateAccountFilterButton("Admin", 390, "Admin"));
            left.Controls.Add(CreateAccountFilterButton("User", 465, "User"));

            _accountListFlow = new FlowLayoutPanel { Location = new Point(10, 48), Size = new Size(570, 298), AutoScroll = true };
            left.Controls.Add(_accountListFlow);

            _accDetailName = new Label { Text = "Chưa chọn", ForeColor = Color.WhiteSmoke, AutoSize = true, Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold), Location = new Point(14, 12) };
            _accDetailRoleBadge = new Label { Text = "User", ForeColor = Color.WhiteSmoke, AutoSize = true, BackColor = Color.FromArgb(62, 69, 106), Padding = new Padding(8, 2, 8, 2), Location = new Point(14, 38) };
            right.Controls.Add(_accDetailName);
            right.Controls.Add(_accDetailRoleBadge);

            _accDetailUsername = CreateDetailTextBox(right, "Tên tài khoản", 14, 74);
            _accDetailEmail = CreateDetailTextBox(right, "Email", 14, 130);
            _accDetailPassword = CreateDetailTextBox(right, "Mật khẩu", 14, 186);
            _accDetailPassword.UseSystemPasswordChar = true;
            _accPassWarn = new Label { AutoSize = true, ForeColor = Color.IndianRed, Location = new Point(14, 238), Text = "⚠ Plaintext — chưa hash" };
            right.Controls.Add(_accPassWarn);

            right.Controls.Add(new Label { Text = "Độ mạnh mật khẩu", AutoSize = true, ForeColor = Color.Gainsboro, Location = new Point(14, 262) });
            Panel scoreBack = new Panel { BackColor = Color.FromArgb(58, 58, 58), Location = new Point(14, 282), Size = new Size(180, 8) };
            _accStrengthFill = new Panel { BackColor = Color.IndianRed, Location = new Point(0, 0), Size = new Size(30, 8) };
            scoreBack.Controls.Add(_accStrengthFill);
            right.Controls.Add(scoreBack);
            _accStrengthLabel = new Label { Text = "Yếu", AutoSize = true, ForeColor = Color.IndianRed, Location = new Point(202, 277) };
            right.Controls.Add(_accStrengthLabel);
        }

        private TextBox CreateDetailTextBox(Control parent, string label, int x, int y)
        {
            parent.Controls.Add(new Label { Text = label, AutoSize = true, ForeColor = Color.Silver, Location = new Point(x, y) });
            TextBox tb = new TextBox { Location = new Point(x, y + 18), Size = new Size(260, 28), ReadOnly = true, BackColor = Color.FromArgb(48, 48, 48), ForeColor = Color.WhiteSmoke, BorderStyle = BorderStyle.FixedSingle };
            parent.Controls.Add(tb);
            return tb;
        }

        private Button CreateAccountFilterButton(string text, int x, string filter)
        {
            Button btn = new Button { Text = text, FlatStyle = FlatStyle.Flat, ForeColor = Color.WhiteSmoke, BackColor = Color.FromArgb(50, 50, 50), Size = new Size(66, 30), Location = new Point(x, 10) };
            btn.FlatAppearance.BorderColor = Color.FromArgb(85, 85, 85);
            btn.Click += (_, __) => { _accountFilter = filter; RenderAccountList(); };
            return btn;
        }

        private void LoadAccountData()
        {
            _accounts.Clear();
            try
            {
                using SqlConnection conn = Connection.GetSqlConnection();
                conn.Open();
                const string sql = "SELECT TenTaiKhoan, MatKhau, COALESCE(Email,'') AS Email, COALESCE(Role,'User') AS Role FROM dbo.Account ORDER BY TenTaiKhoan ASC;";
                using SqlCommand cmd = new SqlCommand(sql, conn);
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    _accounts.Add(new AccountInfo
                    {
                        Username = reader["TenTaiKhoan"]?.ToString() ?? "",
                        Password = reader["MatKhau"]?.ToString() ?? "",
                        Email = reader["Email"]?.ToString() ?? "",
                        Role = NormalizeRole(reader["Role"]?.ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không tải được accounts: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            RenderAccountKpi();
            RenderAccountList();
            if (_accounts.Count > 0)
            {
                SelectAccount(_accounts[0]);
            }
        }

        private void RenderAccountKpi()
        {
            if (_accountTotalKpi == null || _accountAdminKpi == null || _accountUserKpi == null || _accountSecurityKpi == null) return;
            int total = _accounts.Count;
            int admin = _accounts.Count(a => a.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase));
            int user = total - admin;
            int weak = _accounts.Count(a => GetPasswordScore(a.Password) < 50);
            _accountTotalKpi.Text = total.ToString();
            _accountAdminKpi.Text = admin.ToString();
            _accountUserKpi.Text = user.ToString();
            _accountSecurityKpi.Text = weak > 0 ? "Yếu" : "Ổn";

            Control? row = _accountTotalKpi.Parent?.Parent;
            if (row != null && row.Controls.Count >= 4)
            {
                Label? d1 = row.Controls[0].Controls.OfType<Label>().LastOrDefault();
                Label? d2 = row.Controls[1].Controls.OfType<Label>().LastOrDefault();
                Label? d3 = row.Controls[2].Controls.OfType<Label>().LastOrDefault();
                Label? d4 = row.Controls[3].Controls.OfType<Label>().LastOrDefault();
                if (d1 != null) d1.Text = "Tất cả active";
                if (d2 != null) d2.Text = _accounts.FirstOrDefault(a => a.Role == "Admin")?.Username ?? "--";
                if (d3 != null) d3.Text = string.Join(", ", _accounts.Where(a => a.Role != "Admin").Take(2).Select(a => a.Username));
                if (d4 != null) d4.Text = weak > 0 ? "Plaintext password" : "Không phát hiện yếu";
            }
        }

        private void RenderAccountList()
        {
            if (_accountListFlow == null) return;
            _accountListFlow.Controls.Clear();
            var filtered = _accounts.AsEnumerable();
            string keyword = _accountSearchBox?.Text.Trim().ToLowerInvariant() ?? "";
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filtered = filtered.Where(a => a.Username.ToLowerInvariant().Contains(keyword) || a.Email.ToLowerInvariant().Contains(keyword));
            }
            if (_accountFilter == "Admin")
            {
                filtered = filtered.Where(a => a.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase));
            }
            else if (_accountFilter == "User")
            {
                filtered = filtered.Where(a => !a.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase));
            }

            foreach (AccountInfo account in filtered)
            {
                _accountListFlow.Controls.Add(CreateAccountRow(account));
            }
        }

        private Control CreateAccountRow(AccountInfo account)
        {
            Panel row = new Panel { Width = 550, Height = 66, BackColor = Color.FromArgb(44, 44, 44), Margin = new Padding(0, 0, 0, 8), Cursor = Cursors.Hand };
            Label avatar = new Label
            {
                Text = GetInitials(account.Username),
                AutoSize = false,
                Size = new Size(36, 36),
                Location = new Point(10, 15),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = account.Role == "Admin" ? Color.FromArgb(60, 52, 137) : Color.FromArgb(12, 68, 124),
                BackColor = account.Role == "Admin" ? Color.FromArgb(238, 237, 254) : Color.FromArgb(230, 241, 251)
            };
            Label name = new Label { Text = account.Username, AutoSize = true, ForeColor = Color.WhiteSmoke, Font = new Font("Segoe UI Semibold", 11.5F, FontStyle.Bold), Location = new Point(56, 12) };
            Label email = new Label { Text = account.Email, AutoSize = true, ForeColor = Color.Silver, Location = new Point(56, 36) };
            Label role = new Label
            {
                Text = account.Role,
                AutoSize = true,
                BackColor = account.Role == "Admin" ? Color.FromArgb(62, 52, 137) : Color.FromArgb(12, 68, 124),
                ForeColor = Color.WhiteSmoke,
                Padding = new Padding(8, 2, 8, 2),
                Location = new Point(470, 21)
            };
            row.Controls.Add(avatar);
            row.Controls.Add(name);
            row.Controls.Add(email);
            row.Controls.Add(role);
            row.Click += (_, __) => SelectAccount(account);
            foreach (Control child in row.Controls) child.Click += (_, __) => SelectAccount(account);
            return row;
        }

        private void SelectAccount(AccountInfo account)
        {
            _selectedAccount = account;
            if (_accDetailName == null || _accDetailRoleBadge == null || _accDetailUsername == null || _accDetailEmail == null || _accDetailPassword == null || _accPassWarn == null || _accStrengthLabel == null || _accStrengthFill == null)
            {
                return;
            }

            _accDetailName.Text = account.Username;
            _accDetailRoleBadge.Text = account.Role;
            _accDetailRoleBadge.BackColor = account.Role == "Admin" ? Color.FromArgb(62, 52, 137) : Color.FromArgb(12, 68, 124);
            _accDetailUsername.Text = account.Username;
            _accDetailEmail.Text = account.Email;
            _accDetailPassword.Text = account.Password;
            _accPassWarn.Text = "⚠ Plaintext — chưa hash";

            int score = GetPasswordScore(account.Password);
            _accStrengthFill.Width = Math.Max(20, (int)(180 * (score / 100.0)));
            if (score < 40)
            {
                _accStrengthFill.BackColor = Color.IndianRed;
                _accStrengthLabel.Text = "Yếu";
                _accStrengthLabel.ForeColor = Color.IndianRed;
            }
            else if (score < 70)
            {
                _accStrengthFill.BackColor = Color.Goldenrod;
                _accStrengthLabel.Text = "Trung bình";
                _accStrengthLabel.ForeColor = Color.Goldenrod;
            }
            else
            {
                _accStrengthFill.BackColor = Color.MediumSeaGreen;
                _accStrengthLabel.Text = "Mạnh";
                _accStrengthLabel.ForeColor = Color.MediumSeaGreen;
            }
        }

        private static int GetPasswordScore(string password)
        {
            int score = Math.Min(40, password.Length * 5);
            if (password.Any(char.IsUpper)) score += 15;
            if (password.Any(char.IsDigit)) score += 15;
            if (password.Any(ch => !char.IsLetterOrDigit(ch))) score += 20;
            return Math.Min(100, score);
        }

        private static string NormalizeRole(string? role)
        {
            if (string.IsNullOrWhiteSpace(role)) return "User";
            return role.Equals("Admin", StringComparison.OrdinalIgnoreCase) ? "Admin" : "User";
        }

        private static string GetInitials(string username)
        {
            string[] parts = username.Split(new[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2) return $"{char.ToUpper(parts[0][0])}{char.ToUpper(parts[1][0])}";
            if (username.Length >= 2) return username.Substring(0, 2).ToUpperInvariant();
            return username.ToUpperInvariant();
        }

        private void ExportAccountsCsv()
        {
            using SaveFileDialog save = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = $"accounts_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };
            if (save.ShowDialog() != DialogResult.OK) return;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("TenTaiKhoan,Email,Role,MatKhau");
            foreach (AccountInfo a in _accounts)
            {
                sb.AppendLine($"\"{a.Username}\",\"{a.Email}\",\"{a.Role}\",\"{a.Password}\"");
            }
            File.WriteAllText(save.FileName, sb.ToString(), Encoding.UTF8);
            MessageBox.Show("Đã export accounts.", "Thông báo");
        }
    }
}
