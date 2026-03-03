using System;
using System.Collections.Generic;
using System.Data;
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

                using (MySqlCommand cmd = new MySqlCommand("sp_GetEnrolledStudents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_class_id", classId);

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

                using (MySqlCommand cmd = new MySqlCommand("sp_GetAvailableStudents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_class_id", classId);

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

                using (MySqlCommand cmd = new MySqlCommand("sp_EnrollStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_class_id", classId);
                    cmd.Parameters.AddWithValue("p_student_id", studentId);

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
