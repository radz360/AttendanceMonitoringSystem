using System;
using System.Collections.Generic;
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

                string sql = @"SELECT c.class_id, c.subject_id, c.teacher_id,
                       s.subject_code, s.subject_name,
                       CONCAT(t.first_name, ' ', t.last_name) AS teacher_name,
                       c.academic_year, c.semester, c.section
                FROM classes c
                INNER JOIN subjects s ON c.subject_id = s.subject_id
                INNER JOIN teachers t ON c.teacher_id = t.teacher_id
                ORDER BY c.academic_year DESC, c.semester, s.subject_code";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {

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

                string sql = @"SELECT c.class_id, c.subject_id, c.teacher_id,
                       s.subject_code, s.subject_name,
                       CONCAT(t.first_name, ' ', t.last_name) AS teacher_name,
                       c.academic_year, c.semester, c.section
                FROM classes c
                INNER JOIN subjects s ON c.subject_id = s.subject_id
                INNER JOIN teachers t ON c.teacher_id = t.teacher_id
                WHERE c.class_id = @p_class_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_class_id", classId);

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

                string sql = @"INSERT INTO classes (subject_id, teacher_id, academic_year, semester, section)
                VALUES (@p_subject_id, @p_teacher_id, @p_academic_year, @p_semester, @p_section)";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_subject_id", classInfo.SubjectId);
                    cmd.Parameters.AddWithValue("@p_teacher_id", classInfo.TeacherId);
                    cmd.Parameters.AddWithValue("@p_academic_year", classInfo.AcademicYear);
                    cmd.Parameters.AddWithValue("@p_semester", classInfo.Semester);
                    cmd.Parameters.AddWithValue("@p_section", classInfo.Section);

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

                string sql = @"UPDATE classes
                SET subject_id = @p_subject_id,
                    teacher_id = @p_teacher_id,
                    academic_year = @p_academic_year,
                    semester = @p_semester,
                    section = @p_section
                WHERE class_id = @p_class_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_class_id", classInfo.ClassId);
                    cmd.Parameters.AddWithValue("@p_subject_id", classInfo.SubjectId);
                    cmd.Parameters.AddWithValue("@p_teacher_id", classInfo.TeacherId);
                    cmd.Parameters.AddWithValue("@p_academic_year", classInfo.AcademicYear);
                    cmd.Parameters.AddWithValue("@p_semester", classInfo.Semester);
                    cmd.Parameters.AddWithValue("@p_section", classInfo.Section);

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

                string sql = "DELETE FROM classes WHERE class_id = @p_class_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_class_id", classId);

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

                string sql = @"SELECT c.class_id, c.subject_id, c.teacher_id,
                       s.subject_code, s.subject_name,
                       CONCAT(t.first_name, ' ', t.last_name) AS teacher_name,
                       c.academic_year, c.semester, c.section
                FROM classes c
                INNER JOIN subjects s ON c.subject_id = s.subject_id
                INNER JOIN teachers t ON c.teacher_id = t.teacher_id
                WHERE c.teacher_id = @p_teacher_id
                ORDER BY c.academic_year DESC, c.semester, s.subject_code";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_teacher_id", teacherId);

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

                string sql = @"SELECT c.class_id, c.subject_id, c.teacher_id,
                       s.subject_code, s.subject_name,
                       CONCAT(t.first_name, ' ', t.last_name) AS teacher_name,
                       c.academic_year, c.semester, c.section
                FROM classes c
                INNER JOIN subjects s ON c.subject_id = s.subject_id
                INNER JOIN teachers t ON c.teacher_id = t.teacher_id
                WHERE s.subject_code LIKE @kw
                   OR s.subject_name LIKE @kw
                   OR t.first_name LIKE @kw
                   OR t.last_name LIKE @kw
                   OR c.academic_year LIKE @kw
                   OR c.section LIKE @kw
                ORDER BY c.academic_year DESC, c.semester, s.subject_code";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

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
