using Attendance_Monitoring_System.Forms;
using Attendance_Monitoring_System.Models;
using System;
using System.Windows.Forms;

namespace Attendance_Monitoring_System
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ─── DEVELOPMENT / TEST MODE ────────────────────────────
            // Create a dummy user so the dashboard has the data it needs to load
            User testUser = new User
            {
                Username = "DevAdmin",
                Role = "Admin" // change this to "Teacher" or "Admin" to test your role restrictions!
            };

            // Run the dashboard directly
            Application.Run(new DashboardForm(testUser));


            // ─── PRODUCTION MODE ────────────────────────────────────
            // When you are ready to test the real login flow, comment out the test mode above 
            // and uncomment the line below:
            // Application.Run(new LoginForm()); 
        }
    }
}