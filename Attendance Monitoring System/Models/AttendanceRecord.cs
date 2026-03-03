using System;

namespace Attendance_Monitoring_System.Models
{
    public class AttendanceRecord
    {
        public int RecordId { get; set; }
        public int SessionId { get; set; }
        public int StudentId { get; set; }
        public string RegistrationNo { get; set; }   // From JOIN
        public string StudentName { get; set; }       // From JOIN
        public string Status { get; set; }            // e.g. "Present", "Absent", "Late"
        public DateTime? TimeIn { get; set; }         // Nullable time-in value

        public override string ToString()
        {
            return StudentName + " - " + Status;
        }
    }
}