using System;

namespace Attendance_Monitoring_System.Models
{
    public class AttendanceRecord
    {
        public int AttendanceRecordId { get; set; }
        public int SessionId { get; set; }
        public int StudentId { get; set; }
        public string RegistrationNo { get; set; }   // From JOIN
        public string StudentName { get; set; }       // From JOIN
        public int StatusId { get; set; }
        public string StatusName { get; set; }        // From JOIN
        public DateTime MarkedAt { get; set; }

        public override string ToString()
        {
            return StudentName + " - " + StatusName;
        }
    }
}