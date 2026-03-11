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
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty.", nameof(username));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = "SELECT user_id, username, password_hash, role, teacher_id, student_id, is_active FROM users WHERE username = @p_username";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_username", username);

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
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty.", nameof(username));
            if (string.IsNullOrWhiteSpace(plainPassword))
                throw new ArgumentException("Password cannot be empty.", nameof(plainPassword));
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role cannot be empty.", nameof(role));

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = "INSERT INTO users (username, password_hash, role, teacher_id, student_id) VALUES (@p_username, @p_password_hash, @p_role, @p_teacher_id, @p_student_id)";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_username", username);
                    cmd.Parameters.AddWithValue("@p_password_hash", hashedPassword);
                    cmd.Parameters.AddWithValue("@p_role", role);
                    cmd.Parameters.AddWithValue("@p_teacher_id",
                        teacherId.HasValue ? (object)teacherId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_student_id",
                        studentId.HasValue ? (object)studentId.Value : DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Changes the password for a given user. Verifies the old password before updating.
        /// Returns true if the password was changed successfully.
        /// </summary>
        public bool ChangePassword(int userId, string currentPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(currentPassword))
                throw new ArgumentException("Current password cannot be empty.", nameof(currentPassword));
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("New password cannot be empty.", nameof(newPassword));

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                // Retrieve current hash
                string storedHash;
                using (MySqlCommand cmd = new MySqlCommand(
                    "SELECT password_hash FROM users WHERE user_id = @uid", conn))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);
                    object result = cmd.ExecuteScalar();
                    if (result == null)
                        return false;

                    storedHash = result.ToString();
                }

                // Verify current password
                if (!BCrypt.Net.BCrypt.Verify(currentPassword, storedHash))
                    return false;

                // Update with new hash
                string newHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                using (MySqlCommand cmd = new MySqlCommand(
                    "UPDATE users SET password_hash = @hash WHERE user_id = @uid", conn))
                {
                    cmd.Parameters.AddWithValue("@hash", newHash);
                    cmd.Parameters.AddWithValue("@uid", userId);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
        }
    }
}