using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Attendance_Monitoring_System.Models;

namespace Attendance_Monitoring_System.Services
{
    public class ClassService
    {
        // ── Get All Classes ──────────────────────────────────────
        public List<ClassInfo> GetAllClasses()
        {
            List<ClassInfo> classes = new List<ClassInfo>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_GetAllClasses", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            classes.Add(MapClassInfo(reader));
                        }
                    }
                }
            }

            return classes;
        }

        // ── Get Class by ID ──────────────────────────────────────
        public ClassInfo GetClassById(int classId)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_GetClassById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_class_id", classId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapClassInfo(reader);
                        }
                    }
                }
            }

            return null;
        }

        // ── Add Class ────────────────────────────────────────────
        public void AddClass(ClassInfo classInfo)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_AddClass", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_subject_id", classInfo.SubjectId);
                    cmd.Parameters.AddWithValue("p_teacher_id", classInfo.TeacherId);
                    cmd.Parameters.AddWithValue("p_academic_year", classInfo.AcademicYear);
                    cmd.Parameters.AddWithValue("p_semester", classInfo.Semester);
                    cmd.Parameters.AddWithValue("p_section", classInfo.Section);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Update Class ─────────────────────────────────────────
        public void UpdateClass(ClassInfo classInfo)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_UpdateClass", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_class_id", classInfo.ClassId);
                    cmd.Parameters.AddWithValue("p_subject_id", classInfo.SubjectId);
                    cmd.Parameters.AddWithValue("p_teacher_id", classInfo.TeacherId);
                    cmd.Parameters.AddWithValue("p_academic_year", classInfo.AcademicYear);
                    cmd.Parameters.AddWithValue("p_semester", classInfo.Semester);
                    cmd.Parameters.AddWithValue("p_section", classInfo.Section);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Delete Class ─────────────────────────────────────────
        public void DeleteClass(int classId)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_DeleteClass", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_class_id", classId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Get Classes by Teacher ID ────────────────────────────
        public List<ClassInfo> GetClassesByTeacherId(int teacherId)
        {
            List<ClassInfo> classes = new List<ClassInfo>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_GetClassesByTeacherId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_teacher_id", teacherId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            classes.Add(MapClassInfo(reader));
                        }
                    }
                }
            }

            return classes;
        }

        // ── Search Classes ───────────────────────────────────────
        public List<ClassInfo> SearchClasses(string keyword)
        {
            List<ClassInfo> classes = new List<ClassInfo>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_SearchClasses", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_keyword", keyword);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            classes.Add(MapClassInfo(reader));
                        }
                    }
                }
            }

            return classes;
        }

        // ── Helper: Map DataReader row to ClassInfo object ───────
        private ClassInfo MapClassInfo(MySqlDataReader reader)
        {
            return new ClassInfo
            {
                ClassId = reader.GetInt32("class_id"),
                SubjectId = reader.GetInt32("subject_id"),
                TeacherId = reader.GetInt32("teacher_id"),
                SubjectCode = reader.GetString("subject_code"),
                SubjectName = reader.GetString("subject_name"),
                TeacherName = reader.GetString("teacher_name"),
                AcademicYear = reader.GetString("academic_year"),
                Semester = reader.GetInt32("semester"),
                Section = reader.GetString("section")
            };
        }
    }
}
