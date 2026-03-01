using System;

namespace Attendance_Monitoring_System.Models
{
    public class AttendanceSession
    {
        public int SessionId { get; set; }
        public int ClassId { get; set; }
        public int? ScheduleId { get; set; }         // Nullable
        public DateTime SessionDate { get; set; }
        public int CreatedByTeacherId { get; set; }
        public string TeacherName { get; set; }       // From JOIN

        public override string ToString()
        {
            return "Session #" + SessionId + " (" + SessionDate.ToShortDateString() + ")";
        }
    }
}