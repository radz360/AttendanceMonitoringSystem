using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Attendance_Monitoring_System.Models;

namespace Attendance_Monitoring_System.Services
{
    public class TeacherService
    {
        // ── Get All Teachers ─────────────────────────────────────
        public List<Teacher> GetAllTeachers()
        {
            List<Teacher> teachers = new List<Teacher>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"SELECT teacher_id, first_name, last_name, email, designation
                FROM teachers ORDER BY last_name, first_name";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            teachers.Add(MapTeacher(reader));
                        }
                    }
                }
            }

            return teachers;
        }

        // ── Get Teacher by ID ────────────────────────────────────
        public Teacher GetTeacherById(int teacherId)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"SELECT teacher_id, first_name, last_name, email, designation
                FROM teachers WHERE teacher_id = @p_teacher_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_teacher_id", teacherId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapTeacher(reader);
                        }
                    }
                }
            }

            return null;
        }

        // ── Add Teacher ──────────────────────────────────────────
        public void AddTeacher(Teacher teacher)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"INSERT INTO teachers (first_name, last_name, email, designation)
                VALUES (@p_first_name, @p_last_name, @p_email, @p_designation)";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_first_name", teacher.FirstName);
                    cmd.Parameters.AddWithValue("@p_last_name", teacher.LastName);
                    cmd.Parameters.AddWithValue("@p_email", teacher.Email);
                    cmd.Parameters.AddWithValue("@p_designation", teacher.Designation);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Update Teacher ───────────────────────────────────────
        public void UpdateTeacher(Teacher teacher)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"UPDATE teachers
                SET first_name = @p_first_name,
                    last_name = @p_last_name,
                    email = @p_email,
                    designation = @p_designation
                WHERE teacher_id = @p_teacher_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_teacher_id", teacher.TeacherId);
                    cmd.Parameters.AddWithValue("@p_first_name", teacher.FirstName);
                    cmd.Parameters.AddWithValue("@p_last_name", teacher.LastName);
                    cmd.Parameters.AddWithValue("@p_email", teacher.Email);
                    cmd.Parameters.AddWithValue("@p_designation", teacher.Designation);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Delete Teacher ───────────────────────────────────────
        public void DeleteTeacher(int teacherId)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = "DELETE FROM teachers WHERE teacher_id = @p_teacher_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_teacher_id", teacherId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Search Teachers ──────────────────────────────────────
        public List<Teacher> SearchTeachers(string keyword)
        {
            List<Teacher> teachers = new List<Teacher>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"SELECT teacher_id, first_name, last_name, email, designation
                FROM teachers
                WHERE first_name LIKE @kw
                   OR last_name LIKE @kw
                   OR email LIKE @kw
                   OR designation LIKE @kw
                ORDER BY last_name, first_name";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            teachers.Add(MapTeacher(reader));
                        }
                    }
                }
            }

            return teachers;
        }

        // ── Helper: Map DataReader row to Teacher object ─────────
        private Teacher MapTeacher(MySqlDataReader reader)
        {
            return new Teacher
            {
                TeacherId = reader.GetInt32("teacher_id"),
                FirstName = reader.GetString("first_name"),
                LastName = reader.GetString("last_name"),
                Email = reader.IsDBNull(reader.GetOrdinal("email"))
                        ? "" : reader.GetString("email"),
                Designation = reader.IsDBNull(reader.GetOrdinal("designation"))
                              ? "" : reader.GetString("designation")
            };
        }
    }
}
