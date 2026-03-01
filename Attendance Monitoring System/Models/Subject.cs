namespace Attendance_Monitoring_System.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }

        public override string ToString()
        {
            return SubjectCode + " - " + SubjectName;
        }
    }
}
