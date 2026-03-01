using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance_Monitoring_System.Models
{
    public class ClassSchedule
    {
        public int ScheduleId { get; set; }
        public int ClassId { get; set; }
        public int DayOfWeek { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Room { get; set; }

        // Convenience property for display
        public string DayName
        {
            get
            {
                switch (DayOfWeek)
                {
                    case 1: return "Monday";
                    case 2: return "Tuesday";
                    case 3: return "Wednesday";
                    case 4: return "Thursday";
                    case 5: return "Friday";
                    case 6: return "Saturday";
                    case 7: return "Sunday";
                    default: return "Unknown";
                }
            }
        }
    }
}