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

            if (!DatabaseHelper.TestConnection(out string dbError))
            {
                MessageBox.Show(
                    "Cannot connect to the database. Please check your connection settings.\n\n" + dbError,
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Application.Run(new LoginForm());
        }
    }
}