using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class DangNhap : Form
    {
        public DangNhap()
        {
            InitializeComponent();
        }
        private void linkLabel_QuenMatKhau_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            QuenMatKhau quenMatKhau = new QuenMatKhau();
            quenMatKhau.ShowDialog();
        }

        private void linkLabel_DangKy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DangKi dangKi = new DangKi();
            dangKi.ShowDialog();
        }

        Modify modify = new Modify();

        private void button1_Click(object sender, EventArgs e)
        {
            string tentk = textBox_TenTaiKhoan.Text;
            string matkhau = textBox_MatKhau.Text;
            if (tentk.Trim()=="") { MessageBox.Show("Vui lòng nhập tên tài khoản!"); return; }
            else if(matkhau.Trim() == "") { MessageBox.Show("Vui lòng nhập mật khẩu!"); return; }
            else
            {
                string query = $"SELECT * FROM dbo.Account WHERE TenTaiKhoan = '{tentk}' AND MatKhau = '{matkhau}'";
                if(modify.TaiKhoans(query).Count > 0)
                {
                    MessageBox.Show("Đăng nhập thành công!","Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Mở dashboard
                    Dashboard dashboard = new Dashboard();
                    dashboard.ShowDialog();

                    this.Hide();

                    dashboard.FormClosed += (s, args) => this.Show();
                }
                else
                {
                    MessageBox.Show("Tên tài khoản hoặc mật khẩu không đúng!");
                }
            }
        }
    }
}
