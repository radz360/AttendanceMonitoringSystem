using MySql.Data.MySqlClient;

namespace Attendance_Monitoring_System.Services
{
    public static class DatabaseHelper
    {
        private static readonly string _connectionString =
            "Server=localhost;" +
            "Port=3306;" +
            "Database=attendance_db;" +
            "Uid=root;" +
            "Pwd=root;";

        /// <summary>
        /// Returns a new MySqlConnection. 
        /// The caller is responsible for opening and disposing it.
        /// </summary>
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        /// <summary>
        /// Tests whether the database connection can be opened successfully.
        /// Returns true if the connection succeeds, false otherwise.
        /// </summary>
        public static bool TestConnection(out string errorMessage)
        {
            errorMessage = null;
            try
            {
                using (MySqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (MySqlException ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}