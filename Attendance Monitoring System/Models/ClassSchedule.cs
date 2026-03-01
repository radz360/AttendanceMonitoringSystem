namespace Attendance_Monitoring_System.Models
{
    public class ClassSchedule
    {
        private static readonly string[] DayNames =
            { "Unknown", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

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
                if (DayOfWeek >= 1 && DayOfWeek <= 7)
                    return DayNames[DayOfWeek];

                return DayNames[0];
            }
        }

        public override string ToString()
        {
            return DayName + " " + StartTime + "-" + EndTime + " (" + Room + ")";
        }
    }
}