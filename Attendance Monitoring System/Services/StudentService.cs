using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Attendance_Monitoring_System.Models;

namespace Attendance_Monitoring_System.Services
{
    public class StudentService
    {
        // ── Get All Students (Admin / Registrar) ─────────────────
        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"SELECT student_id, registration_no, first_name, last_name, gender, date_of_birth
                FROM students ORDER BY last_name, first_name";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(MapStudent(reader));
                        }
                    }
                }
            }

            return students;
        }

        // ── Get Students by Teacher ID (Teacher role) ────────────
        public List<Student> GetStudentsByTeacherId(int teacherId)
        {
            List<Student> students = new List<Student>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"SELECT DISTINCT s.student_id, s.registration_no, s.first_name, s.last_name,
                       s.gender, s.date_of_birth
                FROM students s
                INNER JOIN enrollments e ON s.student_id = e.student_id
                INNER JOIN classes c ON e.class_id = c.class_id
                WHERE c.teacher_id = @p_teacher_id
                ORDER BY s.last_name, s.first_name";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_teacher_id", teacherId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(MapStudent(reader));
                        }
                    }
                }
            }

            return students;
        }

        // ── Get Student by ID ────────────────────────────────────
        public Student GetStudentById(int studentId)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"SELECT student_id, registration_no, first_name, last_name, gender, date_of_birth
                FROM students WHERE student_id = @p_student_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_student_id", studentId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapStudent(reader);
                        }
                    }
                }
            }

            return null;
        }

        // ── Add Student ──────────────────────────────────────────
        public void AddStudent(Student student)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"INSERT INTO students (registration_no, first_name, last_name, gender, date_of_birth)
                VALUES (@p_registration_no, @p_first_name, @p_last_name, @p_gender, @p_date_of_birth)";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_registration_no", student.RegistrationNo);
                    cmd.Parameters.AddWithValue("@p_first_name", student.FirstName);
                    cmd.Parameters.AddWithValue("@p_last_name", student.LastName);
                    cmd.Parameters.AddWithValue("@p_gender", student.Gender);
                    cmd.Parameters.AddWithValue("@p_date_of_birth", student.DateOfBirth);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Update Student ───────────────────────────────────────
        public void UpdateStudent(Student student)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"UPDATE students
                SET registration_no = @p_registration_no,
                    first_name = @p_first_name,
                    last_name = @p_last_name,
                    gender = @p_gender,
                    date_of_birth = @p_date_of_birth
                WHERE student_id = @p_student_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_student_id", student.StudentId);
                    cmd.Parameters.AddWithValue("@p_registration_no", student.RegistrationNo);
                    cmd.Parameters.AddWithValue("@p_first_name", student.FirstName);
                    cmd.Parameters.AddWithValue("@p_last_name", student.LastName);
                    cmd.Parameters.AddWithValue("@p_gender", student.Gender);
                    cmd.Parameters.AddWithValue("@p_date_of_birth", student.DateOfBirth);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Delete Student ───────────────────────────────────────
        public void DeleteStudent(int studentId)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = "DELETE FROM students WHERE student_id = @p_student_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_student_id", studentId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Search Students ──────────────────────────────────────
        public List<Student> SearchStudents(string keyword)
        {
            List<Student> students = new List<Student>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"SELECT student_id, registration_no, first_name, last_name, gender, date_of_birth
                FROM students
                WHERE registration_no LIKE @kw
                   OR first_name LIKE @kw
                   OR last_name LIKE @kw
                ORDER BY last_name, first_name";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(MapStudent(reader));
                        }
                    }
                }
            }

            return students;
        }

        // ── Helper: Map DataReader row to Student object ─────────
        private Student MapStudent(MySqlDataReader reader)
        {
            return new Student
            {
                StudentId = reader.GetInt32("student_id"),
                RegistrationNo = reader.GetString("registration_no"),
                FirstName = reader.GetString("first_name"),
                LastName = reader.GetString("last_name"),
                Gender = reader.GetString("gender"),
                DateOfBirth = reader.GetDateTime("date_of_birth")
            };
        }
    }
}
