using Attendance_Monitoring_System.Forms;
using Attendance_Monitoring_System.Models;
using Attendance_Monitoring_System.Services;
using System;
using System.Windows.Forms;

namespace Attendance_Monitoring_System
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ─── DATABASE CONNECTION CHECK ──────────────────────────
            if (!DatabaseHelper.TestConnection(out string dbError))
            {
                MessageBox.Show(
                    "Cannot connect to the database. Please check your connection settings.\n\n" + dbError,
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // ─── DEVELOPMENT / TEST MODE ────────────────────────────
            // Switch the Role value to test each sidebar configuration

            // Test as Admin — sees ALL 10 nav buttons
            //User testUser = new User
            //{
            //    UserId = 1,
            //    Username = "DevAdmin",
            //    Role = "Admin",
            //    TeacherId = null,
            //    StudentId = null
            //};

            // Test as Registrar — hides Attendance, Remarks, Users (7 buttons)
            // User testUser = new User
            // {
            //     UserId = 2,
            //     Username = "DevRegistrar",
            //     Role = "Registrar",
            //     TeacherId = null,
            //     StudentId = null
            // };

            // Test as Teacher — sees Dashboard, Students, Schedule, Attendance, Remarks (5 buttons)
            //User testUser = new User
            //{
            //    UserId = 3,
            //    Username = "DevTeacher",
            //    Role = "Teacher",
            //    TeacherId = 1,
            //    StudentId = null
            //};

            // Test as Student — sees Dashboard, Schedule, Attendance, Remarks (4 buttons)
            // User testUser = new User
            // {
            //     UserId = 4,
            //     Username = "DevStudent",
            //     Role = "Student",
            //     TeacherId = null,
            //     StudentId = 1
            // };

            //Application.Run(new DashboardForm(testUser));

            // ─── PRODUCTION MODE ────────────────────────────────────
            Application.Run(new LoginForm());
        }
    }
}