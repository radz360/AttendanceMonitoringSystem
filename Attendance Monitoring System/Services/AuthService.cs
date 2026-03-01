using System;
using MySql.Data.MySqlClient;
using Attendance_Monitoring_System.Models;

namespace Attendance_Monitoring_System.Services
{
    public class AuthService
    {
        /// <summary>
        /// Authenticates a user by username and password.
        /// Returns the User object if successful, or null if failed.
        /// </summary>
        public User Login(string username, string password)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_GetUserByUsername", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_username", username);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bool isActive = reader.GetBoolean("is_active");

                            if (!isActive)
                            {
                                return null;
                            }

                            string storedHash = reader.GetString("password_hash");

                            if (BCrypt.Net.BCrypt.Verify(password, storedHash))
                            {
                                User user = new User
                                {
                                    UserId = reader.GetInt32("user_id"),
                                    Username = reader.GetString("username"),
                                    PasswordHash = storedHash,
                                    Role = reader.GetString("role"),
                                    TeacherId = reader.IsDBNull(reader.GetOrdinal("teacher_id"))
                                                ? (int?)null
                                                : reader.GetInt32("teacher_id"),
                                    StudentId = reader.IsDBNull(reader.GetOrdinal("student_id"))
                                                ? (int?)null
                                                : reader.GetInt32("student_id"),
                                    IsActive = isActive
                                };

                                return user;
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Creates a new user account. Only called by Admin from UserManagementForm.
        /// Hashes the password with BCrypt before storing.
        /// </summary>
        public void CreateUser(string username, string plainPassword, string role, int? teacherId, int? studentId)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_CreateUser", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_username", username);
                    cmd.Parameters.AddWithValue("p_password_hash", hashedPassword);
                    cmd.Parameters.AddWithValue("p_role", role);
                    cmd.Parameters.AddWithValue("p_teacher_id",
                        teacherId.HasValue ? (object)teacherId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("p_student_id",
                        studentId.HasValue ? (object)studentId.Value : DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}