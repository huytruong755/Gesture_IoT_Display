using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    internal class TaiKhoan
    {
        private String tenTaiKhoan;
        private String matKhau;
        private String role;

        public TaiKhoan(String tenTaiKhoan, String matKhau, String role = "User")
        {
            this.tenTaiKhoan = tenTaiKhoan;
            this.matKhau = matKhau;
            this.role = role;
        }

        public string TenTaiKhoan { get => tenTaiKhoan; set => tenTaiKhoan = value; }
        public string MatKhau { get => matKhau; set => matKhau = value; }
        public string Role { get => role; set => role = value; }
    }
}
