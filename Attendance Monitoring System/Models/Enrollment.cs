using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Monitoring_System.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int ClassId { get; set; }
        public int StudentId { get; set; }
        public string RegistrationNo { get; set; }  // From JOIN
        public string StudentName { get; set; }      // From JOIN
        public DateTime EnrolledAt { get; set; }
    }
}