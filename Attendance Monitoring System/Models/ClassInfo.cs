using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Monitoring_System.Models
{
    public class ClassInfo
    {
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
        public string AcademicYear { get; set; }
        public int Semester { get; set; }
        public string Section { get; set; }
    }
}
