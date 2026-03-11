using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Attendance_Monitoring_System.Models;

namespace Attendance_Monitoring_System.Services
{
    public class EnrollmentService
    {
        // ── Get Enrolled Students by Class ID ────────────────────
        public List<Student> GetEnrolledStudents(int classId)
        {
            List<Student> students = new List<Student>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"SELECT e.enrollment_id, e.class_id, e.enrolled_at,
                       s.student_id, s.registration_no, s.first_name,
                       s.last_name, CONCAT(s.first_name, ' ', s.last_name) AS student_name,
                       s.gender, s.date_of_birth
                FROM enrollments e
                INNER JOIN students s ON e.student_id = s.student_id
                WHERE e.class_id = @p_class_id
                ORDER BY s.last_name, s.first_name";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_class_id", classId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new Student
                            {
                                StudentId = reader.GetInt32("student_id"),
                                RegistrationNo = reader.GetString("registration_no"),
                                FirstName = reader.GetString("first_name"),
                                LastName = reader.GetString("last_name"),
                                Gender = reader.GetString("gender"),
                                DateOfBirth = reader.GetDateTime("date_of_birth")
                            });
                        }
                    }
                }
            }

            return students;
        }

        // ── Get Available (Unenrolled) Students for a Class ──────
        public List<Student> GetAvailableStudents(int classId)
        {
            List<Student> students = new List<Student>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"SELECT s.student_id, s.registration_no, s.first_name,
                       s.last_name, CONCAT(s.first_name, ' ', s.last_name) AS student_name,
                       s.gender, s.date_of_birth
                FROM students s
                WHERE s.student_id NOT IN (
                    SELECT e.student_id
                    FROM enrollments e
                    WHERE e.class_id = @p_class_id
                )
                ORDER BY s.last_name, s.first_name";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_class_id", classId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new Student
                            {
                                StudentId = reader.GetInt32("student_id"),
                                RegistrationNo = reader.GetString("registration_no"),
                                FirstName = reader.GetString("first_name"),
                                LastName = reader.GetString("last_name"),
                                Gender = reader.GetString("gender"),
                                DateOfBirth = reader.GetDateTime("date_of_birth")
                            });
                        }
                    }
                }
            }

            return students;
        }

        // ── Enroll Student in Class ──────────────────────────────
        public void EnrollStudent(int classId, int studentId)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = "INSERT INTO enrollments (class_id, student_id) VALUES (@p_class_id, @p_student_id)";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_class_id", classId);
                    cmd.Parameters.AddWithValue("@p_student_id", studentId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Unenroll Student from Class ──────────────────────────
        public void UnenrollStudent(int classId, int studentId)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                // SP sp_UnenrollStudent takes enrollment_id, so use direct query
                // to delete by class_id + student_id (covered by UNIQUE constraint)
                string sql = "DELETE FROM enrollments WHERE class_id = @class_id AND student_id = @student_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@class_id", classId);
                    cmd.Parameters.AddWithValue("@student_id", studentId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
