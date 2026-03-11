using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Attendance_Monitoring_System.Models;

namespace Attendance_Monitoring_System.Services
{
    public class AttendanceService
    {
        // ── Get Sessions by Class ID ─────────────────────────────
        public List<AttendanceSession> GetSessionsByClassId(int classId)
        {
            List<AttendanceSession> sessions = new List<AttendanceSession>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"SELECT a.session_id, a.class_id, a.schedule_id, a.session_date,
                       a.created_by_teacher_id,
                       CONCAT(t.first_name, ' ', t.last_name) AS teacher_name
                FROM attendance_sessions a
                INNER JOIN teachers t ON a.created_by_teacher_id = t.teacher_id
                WHERE a.class_id = @p_class_id
                ORDER BY a.session_date DESC";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_class_id", classId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sessions.Add(MapSession(reader));
                        }
                    }
                }
            }

            return sessions;
        }

        // ── Create Attendance Session — returns new session_id ───
        public int CreateSession(AttendanceSession session)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"INSERT INTO attendance_sessions (class_id, schedule_id, session_date, created_by_teacher_id)
                VALUES (@p_class_id, @p_schedule_id, @p_session_date, @p_created_by_teacher_id);
                SELECT LAST_INSERT_ID() AS session_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_class_id", session.ClassId);
                    cmd.Parameters.AddWithValue("@p_schedule_id",
                        session.ScheduleId.HasValue ? (object)session.ScheduleId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_session_date", session.SessionDate);
                    cmd.Parameters.AddWithValue("@p_created_by_teacher_id", session.CreatedByTeacherId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return reader.GetInt32("session_id");
                    }
                }
            }

            return -1;
        }

        // ── Delete Attendance Session ────────────────────────────
        public void DeleteSession(int sessionId)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"DELETE FROM remarks WHERE session_id = @p_session_id;
                DELETE FROM attendance_records WHERE session_id = @p_session_id;
                DELETE FROM attendance_sessions WHERE session_id = @p_session_id";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_session_id", sessionId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Get Attendance Records by Session ID ─────────────────
        public List<AttendanceRecord> GetAttendanceRecords(int sessionId)
        {
            List<AttendanceRecord> records = new List<AttendanceRecord>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"SELECT ar.attendance_record_id AS record_id,
                       ar.session_id,
                       ar.student_id,
                       s.registration_no,
                       CONCAT(s.first_name, ' ', s.last_name) AS student_name,
                       ast.status_name AS status,
                       ar.time_in
                FROM attendance_records ar
                INNER JOIN students s ON ar.student_id = s.student_id
                INNER JOIN attendance_status ast ON ar.status_id = ast.status_id
                WHERE ar.session_id = @p_session_id
                ORDER BY s.last_name, s.first_name";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_session_id", sessionId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            records.Add(MapRecord(reader));
                        }
                    }
                }
            }

            return records;
        }

        // ── Save (Upsert) Attendance Record ──────────────────────
        public void SaveAttendanceRecord(AttendanceRecord record)
        {
            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"INSERT INTO attendance_records (session_id, student_id, status_id, time_in, marked_at)
                SELECT @p_session_id, @p_student_id, ast.status_id, @p_time_in, CURRENT_TIMESTAMP
                FROM attendance_status ast
                WHERE ast.status_name = @p_status
                ON DUPLICATE KEY UPDATE
                    status_id  = VALUES(status_id),
                    time_in    = VALUES(time_in),
                    marked_at  = CURRENT_TIMESTAMP";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_session_id", record.SessionId);
                    cmd.Parameters.AddWithValue("@p_student_id", record.StudentId);
                    cmd.Parameters.AddWithValue("@p_status", record.Status);
                    cmd.Parameters.AddWithValue("@p_time_in",
                        record.TimeIn.HasValue ? (object)record.TimeIn.Value : DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ── Get Sessions by Teacher ID ───────────────────────────
        public List<AttendanceSession> GetSessionsByTeacherId(int teacherId)
        {
            List<AttendanceSession> sessions = new List<AttendanceSession>();

            using (MySqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string sql = @"SELECT a.session_id, a.class_id, a.schedule_id, a.session_date,
                       a.created_by_teacher_id,
                       CONCAT(t.first_name, ' ', t.last_name) AS teacher_name
                FROM attendance_sessions a
                INNER JOIN teachers t ON a.created_by_teacher_id = t.teacher_id
                WHERE a.created_by_teacher_id = @p_teacher_id
                ORDER BY a.session_date DESC";
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@p_teacher_id", teacherId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sessions.Add(MapSession(reader));
                        }
                    }
                }
            }

            return sessions;
        }

        // ── Helper: Map reader row to AttendanceSession ──────────
        private AttendanceSession MapSession(MySqlDataReader reader)
        {
            return new AttendanceSession
            {
                SessionId = reader.GetInt32("session_id"),
                ClassId = reader.GetInt32("class_id"),
                ScheduleId = reader.IsDBNull(reader.GetOrdinal("schedule_id"))
                    ? (int?)null
                    : reader.GetInt32("schedule_id"),
                SessionDate = reader.GetDateTime("session_date"),
                CreatedByTeacherId = reader.GetInt32("created_by_teacher_id"),
                TeacherName = reader.GetString("teacher_name")
            };
        }

        // ── Helper: Map reader row to AttendanceRecord ───────────
        private AttendanceRecord MapRecord(MySqlDataReader reader)
        {
            return new AttendanceRecord
            {
                RecordId = reader.GetInt32("record_id"),
                SessionId = reader.GetInt32("session_id"),
                StudentId = reader.GetInt32("student_id"),
                RegistrationNo = reader.GetString("registration_no"),
                StudentName = reader.GetString("student_name"),
                Status = reader.GetString("status"),
                TimeIn = reader.IsDBNull(reader.GetOrdinal("time_in"))
                    ? (DateTime?)null
                    : reader.GetDateTime("time_in")
            };
        }
    }
}
