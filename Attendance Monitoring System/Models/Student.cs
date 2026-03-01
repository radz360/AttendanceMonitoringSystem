using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Monitoring_System.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string RegistrationNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }          // "Male", "Female", "Other"
        public DateTime DateOfBirth { get; set; }

        // Convenience property for display
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}
