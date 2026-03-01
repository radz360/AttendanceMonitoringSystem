using Attendance_Monitoring_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Attendance_Monitoring_System.Forms
{
    public partial class DashboardForm : BaseForm
    {
        private readonly User _currentUser;
        private Button _activeButton;

        public DashboardForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            // --- Event Handlers ---
            btnNavDashboard.Click += new EventHandler(btnNavDashboard_Click);
            btnNavStudents.Click += new EventHandler(btnNavStudents_Click);
            btnNavTeachers.Click += new EventHandler(btnNavTeachers_Click);
            btnNavSubjects.Click += new EventHandler(btnNavSubjects_Click);
            btnNavClasses.Click += new EventHandler(btnNavClasses_Click);
            btnNavSchedule.Click += new EventHandler(btnNavSchedule_Click);
            btnNavEnrollment.Click += new EventHandler(btnNavEnrollment_Click);
            btnNavAttendance.Click += new EventHandler(btnNavAttendance_Click);
            btnNavRemarks.Click += new EventHandler(btnNavRemarks_Click);
            btnNavUsers.Click += new EventHandler(btnNavUsers_Click);
            btnNavLogout.Click += new EventHandler(btnNavLogout_Click);
        }

        // ── Form Load ───────────────────────────────────────────
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Display user info
            lblUserInfo.Text = "Welcome, " + _currentUser.Username + " (" + _currentUser.Role + ")";

            // Hide User Management button if not Admin
            if (_currentUser.Role != "Admin")
            {
                btnNavUsers.Visible = false;
            }

            // Show dashboard home by default
            SetActiveButton(btnNavDashboard);
            ShowWelcomePanel();
        }

        // ── Navigation Helper: Load a Form into Content Panel ───
        private void LoadFormInPanel(Form childForm)
        {
            pnlContent.Controls.Clear();

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pnlContent.Controls.Add(childForm);
            childForm.Show();
        }

        // ── Navigation Helper: Highlight Active Button ──────────
        private void SetActiveButton(Button button)
        {
            // Reset previous active button
            if (_activeButton != null)
            {
                _activeButton.BackColor = Color.FromArgb(44, 62, 80);
            }

            // Highlight new active button
            _activeButton = button;
            _activeButton.BackColor = Color.FromArgb(52, 73, 94);
        }

        // ── Welcome Panel (Default Content) ─────────────────────
        private void ShowWelcomePanel()
        {
            pnlContent.Controls.Clear();

            Label lblWelcome = new Label
            {
                Text = "Welcome to the Attendance Monitoring System",
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(50, 50)
            };

            Label lblInstruction = new Label
            {
                Text = "Use the sidebar to navigate between modules.",
                Font = new Font("Segoe UI", 11f),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(50, 95)
            };

            Label lblRole = new Label
            {
                Text = "Logged in as: " + _currentUser.Username + "  |  Role: " + _currentUser.Role,
                Font = new Font("Segoe UI", 10f),
                ForeColor = Color.FromArgb(100, 100, 100),
                AutoSize = true,
                Location = new Point(50, 135)
            };

            pnlContent.Controls.Add(lblWelcome);
            pnlContent.Controls.Add(lblInstruction);
            pnlContent.Controls.Add(lblRole);
        }

        // ── Navigation Click Handlers ──────────────────��────────

        private void btnNavDashboard_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavDashboard);
            ShowWelcomePanel();
        }

        private void btnNavStudents_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavStudents);
            // TODO: LoadFormInPanel(new StudentForm());
        }

        private void btnNavTeachers_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavTeachers);
            // TODO: LoadFormInPanel(new TeacherForm());
        }

        private void btnNavSubjects_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavSubjects);
            // TODO: LoadFormInPanel(new SubjectForm());
        }

        private void btnNavClasses_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavClasses);
            // TODO: LoadFormInPanel(new ClassForm());
        }

        private void btnNavSchedule_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavSchedule);
            // TODO: LoadFormInPanel(new ScheduleForm());
        }

        private void btnNavEnrollment_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavEnrollment);
            // TODO: LoadFormInPanel(new EnrollmentForm());
        }

        private void btnNavAttendance_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavAttendance);
            // TODO: LoadFormInPanel(new AttendanceForm());
        }

        private void btnNavRemarks_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavRemarks);
            // TODO: LoadFormInPanel(new RemarkForm());
        }

        private void btnNavUsers_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavUsers);
            // TODO: LoadFormInPanel(new UserManagementForm());
        }

        private void btnNavLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
