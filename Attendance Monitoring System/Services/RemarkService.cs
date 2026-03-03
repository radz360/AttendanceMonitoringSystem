using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Attendance_Monitoring_System.Models;

namespace Attendance_Monitoring_System.Services
{
    public class RemarkService
    {
        // ── Get Remarks by Session ID ────────────────────────────
        public List<Remark> GetRemarksBySession(int sessionId)
        {
            List<Remark> remarks = new List<Remark>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_GetRemarksBySession", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_session_id", sessionId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            remarks.Add(MapRemark(reader));
                        }
                    }
                }
            }

            return remarks;
        }

        // ── Get All Remarks (Admin/Registrar) ────────────────────
        public List<Remark> GetAllRemarks()
        {
            List<Remark> remarks = new List<Remark>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_GetAllRemarks", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            remarks.Add(MapRemark(reader));
                        }
                    }
                }
            }

            return remarks;
        }

        // ── Get Remarks by Student ID (Student role) ─────────────
        public List<Remark> GetRemarksByStudentId(int studentId)
        {
            List<Remark> remarks = new List<Remark>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_GetRemarksByStudentId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_student_id", studentId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            remarks.Add(MapRemark(reader));
                        }
                    }
                }
            }

            return remarks;
        }

        // ── Search Remarks ───────────────────────────────────────
        public List<Remark> SearchRemarks(string keyword)
        {
            List<Remark> remarks = new List<Remark>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_SearchRemarks", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_keyword", keyword);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            remarks.Add(MapRemark(reader));
                        }
                    }
                }
            }

            return remarks;
        }

        // ── Add Remark ───────────────────────────────────────────
        public void AddRemark(Remark remark)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_AddRemark", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_session_id", remark.SessionId);
                    cmd.Parameters.AddWithValue("p_student_id", remark.StudentId);
                    cmd.Parameters.AddWithValue("p_teacher_id", remark.TeacherId);
                    cmd.Parameters.AddWithValue("p_category_id",
                        remark.CategoryId.HasValue ? (object)remark.CategoryId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("p_remark_text", remark.RemarkText);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Update Remark ────────────────────────────────────────
        public void UpdateRemark(Remark remark)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_UpdateRemark", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_remark_id", remark.RemarkId);
                    cmd.Parameters.AddWithValue("p_category_id",
                        remark.CategoryId.HasValue ? (object)remark.CategoryId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("p_remark_text", remark.RemarkText);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Delete Remark ────────────────────────────────────────
        public void DeleteRemark(int remarkId)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_DeleteRemark", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_remark_id", remarkId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Get All Remark Categories ────────────────────────────
        public List<RemarkCategory> GetAllCategories()
        {
            List<RemarkCategory> categories = new List<RemarkCategory>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_GetAllRemarkCategories", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new RemarkCategory
                            {
                                CategoryId = reader.GetInt32("category_id"),
                                CategoryName = reader.GetString("category_name")
                            });
                        }
                    }
                }
            }

            return categories;
        }

        // ── Get Students in a Session (who have attendance records) ──
        public List<SessionStudentItem> GetStudentsBySession(int sessionId)
        {
            List<SessionStudentItem> students = new List<SessionStudentItem>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_GetStudentsBySession", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_session_id", sessionId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new SessionStudentItem
                            {
                                StudentId = reader.GetInt32("student_id"),
                                RegistrationNo = reader.GetString("registration_no"),
                                StudentName = reader.GetString("student_name")
                            });
                        }
                    }
                }
            }

            return students;
        }

        // ── Helper: Map DataReader row to Remark object ──────────
        private Remark MapRemark(MySqlDataReader reader)
        {
            return new Remark
            {
                RemarkId = reader.GetInt32("remark_id"),
                SessionId = reader.GetInt32("session_id"),
                StudentId = reader.GetInt32("student_id"),
                StudentName = reader.GetString("student_name"),
                TeacherId = reader.GetInt32("teacher_id"),
                TeacherName = reader.GetString("teacher_name"),
                CategoryId = reader.IsDBNull(reader.GetOrdinal("category_id"))
                    ? (int?)null
                    : reader.GetInt32("category_id"),
                CategoryName = reader.IsDBNull(reader.GetOrdinal("category_name"))
                    ? "(None)"
                    : reader.GetString("category_name"),
                RemarkText = reader.GetString("remark_text"),
                RemarkDate = reader.GetDateTime("remark_date")
            };
        }
    }
}
