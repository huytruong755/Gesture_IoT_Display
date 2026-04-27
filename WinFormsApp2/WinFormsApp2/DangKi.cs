using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;  
using System.Text.RegularExpressions;

namespace WinFormsApp2
{
    public partial class DangKi : Form
    {
        public DangKi()
        {
            InitializeComponent();
        }

        public bool CheckAccount(string ac)
        {
            return Regex.IsMatch(ac, @"^[a-zA-Z0-9]{6,24}$");
        }

        public bool CheckEmail(string em)
        {
            return Regex.IsMatch(em, @"^[a-zA-Z0-9_.]{3,20}@gmail.com(.vn|)$");
        }

        Modify modify = new Modify();

        private void button_DangKi_Click(object sender, EventArgs e)
        {
            string TenTaiKhoan = textBox_TenTaiKhoan.Text;
            string MatKhau = textBox_MatKhau.Text;
            string XNMatkhau = textBox_XNMatKhau.Text;
            string Email = textBox_Email.Text;
            if (!CheckAccount(TenTaiKhoan)) { MessageBox.Show("Vui lòng nhập tên tài khoản dài 6-24 ký tự, với các ký tự chữ và số, chữ thường "); return; };
            if (!CheckAccount(MatKhau)) { MessageBox.Show("Vui lòng nhập mật khẩu dài 6-24 ký tự, với các ký tự chữ và số, chữ thường "); return; };
            if (XNMatkhau!=MatKhau) { MessageBox.Show("Vui lòng xác nhận mật khẩu chính xác!"); return; }
            if (!CheckEmail(Email)) { MessageBox.Show("Vui lòng nhập đúng định dạng email!"); return; }
            if(modify.TaiKhoans("SELECT * FROM dbo.Account WHERE Email = '"+ Email +"'").Count != 0) { MessageBox.Show("Email đã tồn tại!"); return; }
            //thực hiện rollback khi xảy ra đăng nhập lỗi
            try
            {
                string query = $"INSERT INTO dbo.Account (TenTaiKhoan, MatKhau, Email) " +
                       $"VALUES('{TenTaiKhoan}', '{MatKhau}', '{Email}')";
                modify.Command(query);  
                if(MessageBox.Show("Đăng kí thành công!","Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    this.Close();
                }
            }
            catch
            {
                MessageBox.Show("Tài khoản đã tồn tại!");
            }
        }
    }
}
