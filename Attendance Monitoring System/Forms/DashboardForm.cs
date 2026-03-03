using Attendance_Monitoring_System.Models;
using System;
using System.Drawing;
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

            // Load sidebar icons and setup buttons
            InitializeSidebar();

            // --- Event Handlers ---
            btnNavDashboard.Click += btnNavDashboard_Click;
            btnNavStudents.Click += btnNavStudents_Click;
            btnNavTeachers.Click += btnNavTeachers_Click;
            btnNavSubjects.Click += btnNavSubjects_Click;
            btnNavClasses.Click += btnNavClasses_Click;
            btnNavSchedule.Click += btnNavSchedule_Click;
            btnNavEnrollment.Click += btnNavEnrollment_Click;
            btnNavAttendance.Click += btnNavAttendance_Click;
            btnNavRemarks.Click += btnNavRemarks_Click;
            btnNavUsers.Click += btnNavUsers_Click;
            btnNavLogout.Click += btnNavLogout_Click;
        }

        // ── Form Load ───────────────────────────────────────────
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Display user info
            lblUserInfo.Text = "Welcome, " + _currentUser.Username + " (" + _currentUser.Role + ")";

            // Apply role-based sidebar visibility
            ApplyRolePermissions();

            // Show dashboard home by default
            SetActiveButton(btnNavDashboard);
            ShowWelcomePanel();
        }

        // ── Role-Based Sidebar Visibility ────────────────────────
        private void ApplyRolePermissions()
        {
            string role = _currentUser.Role;

            // Admin sees everything — no changes needed
            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
                return;

            if (string.Equals(role, "Registrar", StringComparison.OrdinalIgnoreCase))
            {
                // Registrar: hide Attendance, Remarks, User Management
                btnNavAttendance.Visible = false;
                btnNavRemarks.Visible = false;
                btnNavUsers.Visible = false;
            }
            else if (string.Equals(role, "Teacher", StringComparison.OrdinalIgnoreCase))
            {
                // Teacher: hide Teachers, Subjects, Classes, Enrollment, Users
                btnNavTeachers.Visible = false;
                btnNavSubjects.Visible = false;
                btnNavClasses.Visible = false;
                btnNavEnrollment.Visible = false;
                btnNavUsers.Visible = false;
            }
            else if (string.Equals(role, "Student", StringComparison.OrdinalIgnoreCase))
            {
                // Student: only Dashboard, Schedule, Attendance, Remarks
                btnNavStudents.Visible = false;
                btnNavTeachers.Visible = false;
                btnNavSubjects.Visible = false;
                btnNavClasses.Visible = false;
                btnNavEnrollment.Visible = false;
                btnNavUsers.Visible = false;
            }

            RepositionSidebarButtons();
        }

        // ── Reposition Sidebar Buttons (remove gaps) ─────────────
        private void RepositionSidebarButtons()
        {
            int yPosition = 95;
            int buttonHeight = 45;

            Button[] navButtons = new Button[]
            {
                btnNavDashboard,
                btnNavStudents,
                btnNavTeachers,
                btnNavSubjects,
                btnNavClasses,
                btnNavSchedule,
                btnNavEnrollment,
                btnNavAttendance,
                btnNavRemarks,
                btnNavUsers
            };

            foreach (Button btn in navButtons)
            {
                if (btn.Visible)
                {
                    btn.Location = new Point(0, yPosition);
                    yPosition += buttonHeight;
                }
            }

            // Logout button always stays at the bottom
            btnNavLogout.Location = new Point(0, 560);
        }

        // ── Sidebar Initialization ──────────────────────────────
        private void InitializeSidebar()
        {
            ConfigureNavButton(btnNavDashboard, "dashboard");
            ConfigureNavButton(btnNavStudents, "students");
            ConfigureNavButton(btnNavTeachers, "teachers");
            ConfigureNavButton(btnNavSubjects, "subjects");
            ConfigureNavButton(btnNavClasses, "classes");
            ConfigureNavButton(btnNavSchedule, "schedule");
            ConfigureNavButton(btnNavEnrollment, "enrollment");
            ConfigureNavButton(btnNavAttendance, "attendance");
            ConfigureNavButton(btnNavRemarks, "remarks");
            ConfigureNavButton(btnNavUsers, "users");
            ConfigureNavButton(btnNavLogout, "logout");
        }

        private void ConfigureNavButton(Button btn, string keyName)
        {
            btn.ImageList = imageListSidebar;
            btn.ImageKey = keyName;
            btn.ImageAlign = ContentAlignment.MiddleLeft;
            btn.TextImageRelation = TextImageRelation.ImageBeforeText;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(10, 0, 0, 0);
            btn.Text = "  " + btn.Text;
        }

        // ── Navigation Helper: Load a Form into Content Panel ───
        private void LoadFormInPanel(Form childForm)
        {
            // Dispose previous child forms to prevent memory leaks
            foreach (Control ctrl in pnlContent.Controls)
            {
                if (ctrl is Form oldForm)
                {
                    oldForm.Close();
                    oldForm.Dispose();
                }
            }

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
            if (_activeButton != null)
            {
                _activeButton.BackColor = Color.FromArgb(44, 62, 80);
            }

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

        // ── Navigation Click Handlers ───────────────────────────

        private void btnNavDashboard_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavDashboard);
            ShowWelcomePanel();
        }

        private void btnNavStudents_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavStudents);
            LoadFormInPanel(new StudentForm(_currentUser));
        }

        private void btnNavTeachers_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavTeachers);
            LoadFormInPanel(new TeacherForm(_currentUser));
        }

        private void btnNavSubjects_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavSubjects);
            LoadFormInPanel(new SubjectForm(_currentUser));
        }

        private void btnNavClasses_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavClasses);
            LoadFormInPanel(new ClassForm(_currentUser));
        }

        private void btnNavSchedule_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavSchedule);
            // TODO: LoadFormInPanel(new ScheduleForm(_currentUser));
        }

        private void btnNavEnrollment_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavEnrollment);
            // TODO: LoadFormInPanel(new EnrollmentForm(_currentUser));
        }

        private void btnNavAttendance_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavAttendance);
            // TODO: LoadFormInPanel(new AttendanceForm(_currentUser));
        }

        private void btnNavRemarks_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavRemarks);
            // TODO: LoadFormInPanel(new RemarkForm(_currentUser));
        }

        private void btnNavUsers_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnNavUsers);
            // TODO: LoadFormInPanel(new UserManagementForm(_currentUser));
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

































