using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Attendance_Monitoring_System.Models;

namespace Attendance_Monitoring_System.Services
{
    public class SubjectService
    {
        // ── Get All Subjects ─────────────────────────────────────
        public List<Subject> GetAllSubjects()
        {
            List<Subject> subjects = new List<Subject>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = "SELECT subject_id, subject_code, subject_name FROM subjects ORDER BY subject_code";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subjects.Add(MapSubject(reader));
                        }
                    }
                }
            }

            return subjects;
        }

        // ── Get Subject by ID ────────────────────────────────────
        public Subject GetSubjectById(int subjectId)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = "SELECT subject_id, subject_code, subject_name FROM subjects WHERE subject_id = @p_subject_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_subject_id", subjectId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapSubject(reader);
                        }
                    }
                }
            }

            return null;
        }

        // ── Add Subject ──────────────────────────────────────────
        public void AddSubject(Subject subject)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = "INSERT INTO subjects (subject_code, subject_name) VALUES (@p_subject_code, @p_subject_name)";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_subject_code", subject.SubjectCode);
                    cmd.Parameters.AddWithValue("@p_subject_name", subject.SubjectName);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Update Subject ───────────────────────────────────────
        public void UpdateSubject(Subject subject)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"UPDATE subjects
                SET subject_code = @p_subject_code,
                    subject_name = @p_subject_name
                WHERE subject_id = @p_subject_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_subject_id", subject.SubjectId);
                    cmd.Parameters.AddWithValue("@p_subject_code", subject.SubjectCode);
                    cmd.Parameters.AddWithValue("@p_subject_name", subject.SubjectName);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Delete Subject ───────────────────────────────────────
        public void DeleteSubject(int subjectId)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = "DELETE FROM subjects WHERE subject_id = @p_subject_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_subject_id", subjectId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Search Subjects ──────────────────────────────────────
        public List<Subject> SearchSubjects(string keyword)
        {
            List<Subject> subjects = new List<Subject>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"SELECT subject_id, subject_code, subject_name
                FROM subjects
                WHERE subject_code LIKE @kw
                   OR subject_name LIKE @kw
                ORDER BY subject_code";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subjects.Add(MapSubject(reader));
                        }
                    }
                }
            }

            return subjects;
        }

        // ── Helper: Map DataReader row to Subject object ─────────
        private Subject MapSubject(MySqlDataReader reader)
        {
            return new Subject
            {
                SubjectId = reader.GetInt32("subject_id"),
                SubjectCode = reader.GetString("subject_code"),
                SubjectName = reader.GetString("subject_name")
            };
        }
    }
}
