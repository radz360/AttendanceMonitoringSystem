using System;

namespace Attendance_Monitoring_System.Models
{
    public class AttendanceSession
    {
        public int SessionId { get; set; }
        public int ClassId { get; set; }
        public string SessionName { get; set; }       // User-editable session label
        public DateTime SessionDate { get; set; }
        public int CreatedByTeacherId { get; set; }
        public string TeacherName { get; set; }       // From JOIN

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(SessionName))
                return SessionName + " (" + SessionDate.ToString("yyyy-MM-dd") + ")";
            return SessionDate.ToString("yyyy-MM-dd") + " (Session #" + SessionId + ")";
        }
    }
}