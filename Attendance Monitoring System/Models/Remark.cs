using System;

namespace Attendance_Monitoring_System.Models
{
    public class Remark
    {
        public int RemarkId { get; set; }
        public int SessionId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }       // From JOIN
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }       // From JOIN
        public int? CategoryId { get; set; }          // Nullable
        public string CategoryName { get; set; }      // From JOIN
        public string RemarkText { get; set; }
        public DateTime RemarkDate { get; set; }

        public override string ToString()
        {
            return StudentName + " - " + RemarkText;
        }
    }
}
