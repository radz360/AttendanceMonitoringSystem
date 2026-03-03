namespace Attendance_Monitoring_System.Models
{
    public class SessionStudentItem
    {
        public int StudentId { get; set; }
        public string RegistrationNo { get; set; }
        public string StudentName { get; set; }

        public override string ToString()
        {
            return RegistrationNo + " - " + StudentName;
        }
    }
}
