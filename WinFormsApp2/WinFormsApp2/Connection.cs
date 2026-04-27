using Microsoft.Data.SqlClient;

namespace WinFormsApp2
{
    internal class Connection
    {
        private static string stringConnection =@"Server=localhost\SQLEXPRESS;Database=Database_Gesture;Integrated Security=True;TrustServerCertificate=True;";
        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(stringConnection);
        }
    }
}