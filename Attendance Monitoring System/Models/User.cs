using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Monitoring_System.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }           // "Admin" or "Teacher"
        public int? TeacherId { get; set; }         // Nullable — Admins may not be linked
        public string TeacherName { get; set; }     // For display purposes (from JOIN)
        public bool IsActive { get; set; }
    }
}
