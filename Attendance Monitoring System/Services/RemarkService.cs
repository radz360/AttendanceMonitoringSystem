using System;
using System.Collections.Generic;
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

                string sql = @"SELECT r.remark_id, r.session_id, r.student_id,
                       CONCAT(s.first_name, ' ', s.last_name) AS student_name,
                       r.teacher_id,
                       CONCAT(t.first_name, ' ', t.last_name) AS teacher_name,
                       r.category_id,
                       rc.category_name,
                       r.remark_text, r.remark_date
                FROM remarks r
                INNER JOIN students s ON r.student_id = s.student_id
                INNER JOIN teachers t ON r.teacher_id = t.teacher_id
                LEFT JOIN remark_categories rc ON r.category_id = rc.category_id
                WHERE r.session_id = @p_session_id
                ORDER BY r.remark_date DESC";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_session_id", sessionId);

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

                string sql = @"SELECT r.remark_id, r.session_id, r.student_id,
                       CONCAT(s.first_name, ' ', s.last_name) AS student_name,
                       r.teacher_id,
                       CONCAT(t.first_name, ' ', t.last_name) AS teacher_name,
                       r.category_id,
                       rc.category_name,
                       r.remark_text, r.remark_date
                FROM remarks r
                INNER JOIN students s ON r.student_id = s.student_id
                INNER JOIN teachers t ON r.teacher_id = t.teacher_id
                LEFT JOIN remark_categories rc ON r.category_id = rc.category_id
                ORDER BY r.remark_date DESC";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {

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

                string sql = @"SELECT r.remark_id, r.session_id, r.student_id,
                       CONCAT(s.first_name, ' ', s.last_name) AS student_name,
                       r.teacher_id,
                       CONCAT(t.first_name, ' ', t.last_name) AS teacher_name,
                       r.category_id,
                       rc.category_name,
                       r.remark_text, r.remark_date
                FROM remarks r
                INNER JOIN students s ON r.student_id = s.student_id
                INNER JOIN teachers t ON r.teacher_id = t.teacher_id
                LEFT JOIN remark_categories rc ON r.category_id = rc.category_id
                WHERE r.student_id = @p_student_id
                ORDER BY r.remark_date DESC";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_student_id", studentId);

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

                string sql = @"SELECT r.remark_id, r.session_id, r.student_id,
                       CONCAT(s.first_name, ' ', s.last_name) AS student_name,
                       r.teacher_id,
                       CONCAT(t.first_name, ' ', t.last_name) AS teacher_name,
                       r.category_id,
                       rc.category_name,
                       r.remark_text, r.remark_date
                FROM remarks r
                INNER JOIN students s ON r.student_id = s.student_id
                INNER JOIN teachers t ON r.teacher_id = t.teacher_id
                LEFT JOIN remark_categories rc ON r.category_id = rc.category_id
                WHERE s.first_name LIKE @kw
                   OR s.last_name LIKE @kw
                   OR r.remark_text LIKE @kw
                   OR t.first_name LIKE @kw
                   OR t.last_name LIKE @kw
                   OR rc.category_name LIKE @kw
                ORDER BY r.remark_date DESC";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

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

                string sql = @"INSERT INTO remarks (session_id, student_id, teacher_id, category_id, remark_text)
                VALUES (@p_session_id, @p_student_id, @p_teacher_id, @p_category_id, @p_remark_text)";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_session_id", remark.SessionId);
                    cmd.Parameters.AddWithValue("@p_student_id", remark.StudentId);
                    cmd.Parameters.AddWithValue("@p_teacher_id", remark.TeacherId);
                    cmd.Parameters.AddWithValue("@p_category_id",
                        remark.CategoryId.HasValue ? (object)remark.CategoryId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_remark_text", remark.RemarkText);

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

                string sql = @"UPDATE remarks
                SET category_id = @p_category_id,
                    remark_text = @p_remark_text
                WHERE remark_id = @p_remark_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_remark_id", remark.RemarkId);
                    cmd.Parameters.AddWithValue("@p_category_id",
                        remark.CategoryId.HasValue ? (object)remark.CategoryId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_remark_text", remark.RemarkText);

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

                string sql = "DELETE FROM remarks WHERE remark_id = @p_remark_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_remark_id", remarkId);

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

                string sql = "SELECT category_id, category_name FROM remark_categories ORDER BY category_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {

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

                string sql = @"SELECT DISTINCT ar.student_id, s.registration_no,
                       CONCAT(s.first_name, ' ', s.last_name) AS student_name
                FROM attendance_records ar
                INNER JOIN students s ON ar.student_id = s.student_id
                WHERE ar.session_id = @p_session_id
                ORDER BY student_name";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_session_id", sessionId);

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
