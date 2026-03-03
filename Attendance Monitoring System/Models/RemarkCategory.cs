namespace Attendance_Monitoring_System.Models
{
    public class RemarkCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public override string ToString()
        {
            return CategoryName;
        }
    }
}
