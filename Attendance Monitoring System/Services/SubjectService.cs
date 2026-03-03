using System;
using System.Collections.Generic;
using System.Data;
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

                using (MySqlCommand cmd = new MySqlCommand("sp_GetAllSubjects", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

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

                using (MySqlCommand cmd = new MySqlCommand("sp_GetSubjectById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_subject_id", subjectId);

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

                using (MySqlCommand cmd = new MySqlCommand("sp_AddSubject", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_subject_code", subject.SubjectCode);
                    cmd.Parameters.AddWithValue("p_subject_name", subject.SubjectName);

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

                using (MySqlCommand cmd = new MySqlCommand("sp_UpdateSubject", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_subject_id", subject.SubjectId);
                    cmd.Parameters.AddWithValue("p_subject_code", subject.SubjectCode);
                    cmd.Parameters.AddWithValue("p_subject_name", subject.SubjectName);

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

                using (MySqlCommand cmd = new MySqlCommand("sp_DeleteSubject", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_subject_id", subjectId);

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

                using (MySqlCommand cmd = new MySqlCommand("sp_SearchSubjects", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_keyword", keyword);

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
