using System;
using System.Collections.Generic;
using System.Data;
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

                using (MySqlCommand cmd = new MySqlCommand("sp_GetAllStudents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

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

                using (MySqlCommand cmd = new MySqlCommand("sp_GetStudentsByTeacherId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_teacher_id", teacherId);

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

                using (MySqlCommand cmd = new MySqlCommand("sp_GetStudentById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_student_id", studentId);

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

                using (MySqlCommand cmd = new MySqlCommand("sp_AddStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_registration_no", student.RegistrationNo);
                    cmd.Parameters.AddWithValue("p_first_name", student.FirstName);
                    cmd.Parameters.AddWithValue("p_last_name", student.LastName);
                    cmd.Parameters.AddWithValue("p_gender", student.Gender);
                    cmd.Parameters.AddWithValue("p_date_of_birth", student.DateOfBirth);

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

                using (MySqlCommand cmd = new MySqlCommand("sp_UpdateStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_student_id", student.StudentId);
                    cmd.Parameters.AddWithValue("p_registration_no", student.RegistrationNo);
                    cmd.Parameters.AddWithValue("p_first_name", student.FirstName);
                    cmd.Parameters.AddWithValue("p_last_name", student.LastName);
                    cmd.Parameters.AddWithValue("p_gender", student.Gender);
                    cmd.Parameters.AddWithValue("p_date_of_birth", student.DateOfBirth);

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

                using (MySqlCommand cmd = new MySqlCommand("sp_DeleteStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_student_id", studentId);

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

                using (MySqlCommand cmd = new MySqlCommand("sp_SearchStudents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_keyword", keyword);

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
