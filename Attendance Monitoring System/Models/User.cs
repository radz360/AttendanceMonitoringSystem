namespace Attendance_Monitoring_System.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }           // "Admin", "Registrar", "Teacher", or "Student"
        public int? TeacherId { get; set; }         // Nullable — only for Teacher role
        public int? StudentId { get; set; }         // Nullable — only for Student role
        public string TeacherName { get; set; }     // For display purposes (from JOIN)
        public string StudentName { get; set; }     // For display purposes (from JOIN)
        public bool IsActive { get; set; }

        public override string ToString()
        {
            return Username + " (" + Role + ")";
        }
    }
}