using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Attendance_Monitoring_System.Models;

namespace Attendance_Monitoring_System.Services
{
    public class UserService
    {
        // ── Get All Users ────────────────────────────────────────
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_GetAllUsers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(MapUser(reader));
                        }
                    }
                }
            }

            return users;
        }

        // ── Update User ──────────────────────────────────────────
        public void UpdateUser(User user)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_UpdateUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_user_id", user.UserId);
                    cmd.Parameters.AddWithValue("p_username", user.Username);
                    cmd.Parameters.AddWithValue("p_role", user.Role);
                    cmd.Parameters.AddWithValue("p_teacher_id",
                        user.TeacherId.HasValue ? (object)user.TeacherId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("p_student_id",
                        user.StudentId.HasValue ? (object)user.StudentId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("p_is_active", user.IsActive ? 1 : 0);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Reset User Password ──────────────────────────────────
        public void ResetPassword(int userId, string newPlainPassword)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPlainPassword);

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_ResetUserPassword", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_user_id", userId);
                    cmd.Parameters.AddWithValue("p_password_hash", hashedPassword);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Helper: Map DataReader row to User object ────────────
        private User MapUser(MySqlDataReader reader)
        {
            return new User
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
                TeacherName = reader.IsDBNull(reader.GetOrdinal("teacher_name"))
                    ? ""
                    : reader.GetString("teacher_name"),
                StudentName = reader.IsDBNull(reader.GetOrdinal("student_name"))
                    ? ""
                    : reader.GetString("student_name"),
                IsActive = reader.GetInt32("is_active") == 1
            };
        }
    }
}
