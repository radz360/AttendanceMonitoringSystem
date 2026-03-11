using System;
using System.Collections.Generic;
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

                string sql = @"SELECT schedule_id, class_id, day_of_week, start_time, end_time, room
                FROM class_schedule
                WHERE class_id = @p_class_id
                ORDER BY day_of_week, start_time";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_class_id", classId);

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

                string sql = @"SELECT cs.schedule_id, cs.class_id, cs.day_of_week,
                       cs.start_time, cs.end_time, cs.room,
                       s.subject_code, s.subject_name,
                       c.section
                FROM class_schedule cs
                INNER JOIN classes c ON cs.class_id = c.class_id
                INNER JOIN subjects s ON c.subject_id = s.subject_id
                WHERE c.teacher_id = @p_teacher_id
                ORDER BY cs.day_of_week, cs.start_time";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_teacher_id", teacherId);

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

                string sql = @"SELECT cs.schedule_id, cs.class_id, cs.day_of_week,
                       cs.start_time, cs.end_time, cs.room,
                       s.subject_code, s.subject_name,
                       c.section
                FROM class_schedule cs
                INNER JOIN classes c ON cs.class_id = c.class_id
                INNER JOIN subjects s ON c.subject_id = s.subject_id
                INNER JOIN enrollments e ON c.class_id = e.class_id
                WHERE e.student_id = @p_student_id
                ORDER BY cs.day_of_week, cs.start_time";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_student_id", studentId);

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

                string sql = @"INSERT INTO class_schedule (class_id, day_of_week, start_time, end_time, room)
                VALUES (@p_class_id, @p_day_of_week, @p_start_time, @p_end_time, @p_room)";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_class_id", schedule.ClassId);
                    cmd.Parameters.AddWithValue("@p_day_of_week", schedule.DayOfWeek);
                    cmd.Parameters.AddWithValue("@p_start_time", schedule.StartTime);
                    cmd.Parameters.AddWithValue("@p_end_time", schedule.EndTime);
                    cmd.Parameters.AddWithValue("@p_room", schedule.Room);

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

                string sql = @"UPDATE class_schedule
                SET day_of_week = @p_day_of_week,
                    start_time = @p_start_time,
                    end_time = @p_end_time,
                    room = @p_room
                WHERE schedule_id = @p_schedule_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_schedule_id", schedule.ScheduleId);
                    cmd.Parameters.AddWithValue("@p_day_of_week", schedule.DayOfWeek);
                    cmd.Parameters.AddWithValue("@p_start_time", schedule.StartTime);
                    cmd.Parameters.AddWithValue("@p_end_time", schedule.EndTime);
                    cmd.Parameters.AddWithValue("@p_room", schedule.Room);

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

                string sql = "DELETE FROM class_schedule WHERE schedule_id = @p_schedule_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_schedule_id", scheduleId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Helper: Map basic schedule (class schedules query) ──
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

        // ── Helper: Map schedule with subject info (Teacher/Student queries) ──
        private ClassSchedule MapScheduleWithSubject(MySqlDataReader reader)
        {
            return new ClassSchedule
            {
                ScheduleId = reader.GetInt32("schedule_id"),
                ClassId = reader.GetInt32("class_id"),
                DayOfWeek = reader.GetInt32("day_of_week"),
                StartTime = reader.GetString("start_time"),
                EndTime = reader.GetString("end_time"),
                Room = reader.GetString("room"),
                SubjectCode = reader.GetString("subject_code"),
                SubjectName = reader.GetString("subject_name"),
                Section = reader.GetString("section")
            };
        }
    }
}
