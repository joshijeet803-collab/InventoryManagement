using System.Configuration;
using System.Data.SqlClient;

namespace InventoryManagement.Models
{
    public class DBConnection
    {
        public static SqlConnection GetConnection()
        {
            string cs = ConfigurationManager.ConnectionStrings["InventoryDB"].ConnectionString;

            return new SqlConnection(cs);
        }
    }
}