using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class QuenMatKhau : Form
    {
        public QuenMatKhau()
        {
            InitializeComponent();
            label2.Text = "";
        }
        Modify modify = new Modify();
        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text;
            if (email.Trim() == "") { MessageBox.Show("Vui lòng nhập email đăng kí!"); }
            else
            {
                string query = "SELECT * FROM dbo.Account WHERE Email = '" + email + "'";
                if(modify.TaiKhoans(query).Count > 0)
                {
                    label1.ForeColor = Color.Blue;
                    label2.Text = "Mật khẩu của bạn là: " + modify.TaiKhoans(query)[0].MatKhau;
                }
                else
                {
                    label1.ForeColor = Color.Red;
                    MessageBox.Show("Email không tồn tại!");
                }
            }
        }


    }
}
