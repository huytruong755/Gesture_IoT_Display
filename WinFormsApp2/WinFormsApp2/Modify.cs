using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace WinFormsApp2
{
    internal class Modify
    {
        public Modify() {
        }

        SqlCommand sqlCommand;//truy vấn các câu lệnh insert/update/delete/select
        SqlDataReader dataReader;//đọc dữ liệu trả về sau khi thực hiện câu lệnh truy vấn
        public List<TaiKhoan> TaiKhoans(string query)
        {
            List<TaiKhoan> TaiKhoans = new List<TaiKhoan>();
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                dataReader = sqlCommand.ExecuteReader();
                while(dataReader.Read()) {
                    TaiKhoans.Add(new TaiKhoan(dataReader.GetString(0), dataReader.GetString(1)));
                }

                sqlConnection.Close();
            }
                return TaiKhoans;
        }

        public List<GestureHistory> GestureHistories(string query)
        {
            List<GestureHistory> list = new List<GestureHistory>();
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    list.Add(new GestureHistory(
                        dataReader.GetInt32(0),                           
                        dataReader.GetString(1),                         
                        dataReader.GetInt32(2),                           
                        dataReader.GetDateTime(3).ToString("HH:mm:ss dd/MM/yyyy") 
                    ));
                }
                sqlConnection.Close();
            }
            return list;
        }

        //dùng đăng kí tài khoản
        public void Command(string query)
        {
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
        } 
    }
}
