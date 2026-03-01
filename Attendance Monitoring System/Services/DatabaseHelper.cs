using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Monitoring_System.Services
{
    public static class DatabaseHelper
    {
        private static readonly string _connectionString =
            "Server=localhost;" +
            "Port=3306;" +
            "Database=attendance_db;" +
            "Uid=root;" +
            "Pwd=042006;";

        /// <summary>
        /// Returns a new MySqlConnection. 
        /// The caller is responsible for opening and disposing it.
        /// </summary>
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}