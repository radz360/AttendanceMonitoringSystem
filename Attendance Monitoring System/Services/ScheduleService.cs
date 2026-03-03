using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using Attendance_Monitoring_System.Models;

namespace Attendance_Monitoring_System.Services
{
    public class ScheduleService
    {
        // ── Get Schedules by Class ID ────────────────────────────
        public List<ClassSchedule> GetSchedulesByClassId(int classId)
        {
            List<ClassSchedule> schedules = new List<ClassSchedule>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_GetSchedulesByClassId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_class_id", classId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            schedules.Add(MapSchedule(reader));
                        }
                    }
                }
            }

            return schedules;
        }

        // ── Get Schedules by Teacher ID (Teacher role) ───────────
        public List<ClassSchedule> GetSchedulesByTeacherId(int teacherId)
        {
            List<ClassSchedule> schedules = new List<ClassSchedule>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_GetSchedulesByTeacherId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_teacher_id", teacherId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            schedules.Add(MapScheduleWithSubject(reader));
                        }
                    }
                }
            }

            return schedules;
        }

        // ── Get Schedules by Student ID (Student role) ───────────
        public List<ClassSchedule> GetSchedulesByStudentId(int studentId)
        {
            List<ClassSchedule> schedules = new List<ClassSchedule>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_GetSchedulesByStudentId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_student_id", studentId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            schedules.Add(MapScheduleWithSubject(reader));
                        }
                    }
                }
            }

            return schedules;
        }

        // ── Add Schedule ─────────────────────────────────────────
        public void AddSchedule(ClassSchedule schedule)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_AddSchedule", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_class_id", schedule.ClassId);
                    cmd.Parameters.AddWithValue("p_day_of_week", schedule.DayOfWeek);
                    cmd.Parameters.AddWithValue("p_start_time", schedule.StartTime);
                    cmd.Parameters.AddWithValue("p_end_time", schedule.EndTime);
                    cmd.Parameters.AddWithValue("p_room", schedule.Room);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Update Schedule ──────────────────────────────────────
        public void UpdateSchedule(ClassSchedule schedule)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_UpdateSchedule", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_schedule_id", schedule.ScheduleId);
                    cmd.Parameters.AddWithValue("p_day_of_week", schedule.DayOfWeek);
                    cmd.Parameters.AddWithValue("p_start_time", schedule.StartTime);
                    cmd.Parameters.AddWithValue("p_end_time", schedule.EndTime);
                    cmd.Parameters.AddWithValue("p_room", schedule.Room);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Delete Schedule ──────────────────────────────────────
        public void DeleteSchedule(int scheduleId)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("sp_DeleteSchedule", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_schedule_id", scheduleId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Helper: Map basic schedule (from sp_GetSchedulesByClassId) ──
        private ClassSchedule MapSchedule(MySqlDataReader reader)
        {
            return new ClassSchedule
            {
                ScheduleId = reader.GetInt32("schedule_id"),
                ClassId = reader.GetInt32("class_id"),
                DayOfWeek = reader.GetInt32("day_of_week"),
                StartTime = reader.GetString("start_time"),
                EndTime = reader.GetString("end_time"),
                Room = reader.GetString("room")
            };
        }

        // ── Helper: Map schedule with subject info (Teacher/Student SPs) ──
        private ClassSchedule MapScheduleWithSubject(MySqlDataReader reader)
        {
            return new ClassSchedule
            {
                ScheduleId = reader.GetInt32("schedule_id"),
                ClassId = reader.GetInt32("class_id"),
                DayOfWeek = reader.GetInt32("day_of_week"),
                StartTime = reader.GetString("start_time"),
                EndTime = reader.GetString("end_time"),
                Room = reader.GetString("room")
            };
        }
    }
}
