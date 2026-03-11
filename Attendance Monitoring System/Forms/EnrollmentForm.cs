using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Attendance_Monitoring_System.Models;
using Attendance_Monitoring_System.Services;

namespace Attendance_Monitoring_System.Forms
{
    public partial class EnrollmentForm : BaseForm
    {
        private readonly User _currentUser;
        private readonly EnrollmentService _enrollmentService = new EnrollmentService();
        private readonly ClassService _classService = new ClassService();

        public EnrollmentForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            // --- Event Handlers ---
            cmbClass.SelectedIndexChanged += cmbClass_SelectedIndexChanged;
            btnEnroll.Click += btnEnroll_Click;
            btnRemove.Click += btnRemove_Click;
        }

        // ── Form Load ───────────────────────────────────────────
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Style the Remove button red
            btnRemove.BackColor = Color.FromArgb(192, 57, 43);

            // Load classes into ComboBox
            LoadClassesComboBox();
        }

        // ── Load Classes into ComboBox ───────────────────────────
        private void LoadClassesComboBox()
        {
            try
            {
                List<ClassInfo> classes = _classService.GetAllClasses();

                List<ClassDisplayItem> displayItems = new List<ClassDisplayItem>();
                foreach (ClassInfo c in classes)
                {
                    displayItems.Add(new ClassDisplayItem
                    {
                        ClassId = c.ClassId,
                        DisplayText = c.SubjectCode + " - " + c.SubjectName +
                                      " (" + c.TeacherName + ") [" +
                                      c.AcademicYear + " Sem " + c.Semester + " Sec " + c.Section + "]"
                    });
                }

                cmbClass.DataSource = null;
                cmbClass.DisplayMember = "DisplayText";
                cmbClass.ValueMember = "ClassId";
                cmbClass.DataSource = displayItems;

                if (cmbClass.Items.Count > 0)
                    cmbClass.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ShowError("Failed to load classes:\n\n" + ex.Message);
            }
        }

        // ── Class Selection Changed — Refresh Both Grids ─────────
        private void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbClass.SelectedValue == null) return;
            RefreshGrids();
        }

        // ── Refresh Both DataGridViews ───────────────────────────
        private void RefreshGrids()
        {
            try
            {
                int classId = (int)cmbClass.SelectedValue;

                // Load Available Students (LEFT grid)
                List<Student> available = _enrollmentService.GetAvailableStudents(classId);
                dgvAvailable.DataSource = null;
                dgvAvailable.DataSource = available;
                FormatStudentGrid(dgvAvailable);

                // Load Enrolled Students (RIGHT grid)
                List<Student> enrolled = _enrollmentService.GetEnrolledStudents(classId);
                dgvEnrolled.DataSource = null;
                dgvEnrolled.DataSource = enrolled;
                FormatStudentGrid(dgvEnrolled);
            }
            catch (Exception ex)
            {
                ShowError("Failed to load enrollment data:\n\n" + ex.Message);
            }
        }

        // ── Format Student Grid Columns ──────────────────────────
        private void FormatStudentGrid(DataGridView grid)
        {
            if (grid.Columns.Count > 0)
            {
                grid.Columns["StudentId"].Visible = false;
                grid.Columns["Gender"].Visible = false;
                grid.Columns["DateOfBirth"].Visible = false;
                grid.Columns["FullName"].Visible = false;

                grid.Columns["RegistrationNo"].HeaderText = "Student ID";
                grid.Columns["FirstName"].HeaderText = "First Name";
                grid.Columns["LastName"].HeaderText = "Last Name";
            }
        }

        // ── Enroll Selected Students (Available → Enrolled) ──────
        private void btnEnroll_Click(object sender, EventArgs e)
        {
            if (cmbClass.SelectedValue == null)
            {
                ShowWarning("Please select a class first.");
                return;
            }

            if (dgvAvailable.SelectedRows.Count == 0)
            {
                ShowWarning("Please select one or more students from the Available list to enroll.");
                return;
            }

            try
            {
                int classId = (int)cmbClass.SelectedValue;
                int enrolledCount = 0;

                foreach (DataGridViewRow row in dgvAvailable.SelectedRows)
                {
                    int studentId = Convert.ToInt32(row.Cells["StudentId"].Value);
                    _enrollmentService.EnrollStudent(classId, studentId);
                    enrolledCount++;
                }

                ShowSuccess(enrolledCount + " student(s) enrolled successfully.");
                RefreshGrids();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    ShowWarning("One or more students are already enrolled in this class.");
                }
                else
                {
                    ShowError("Database error:\n\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to enroll students:\n\n" + ex.Message);
            }
        }

        // ── Remove Selected Students (Enrolled → Available) ──────
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (cmbClass.SelectedValue == null)
            {
                ShowWarning("Please select a class first.");
                return;
            }

            if (dgvEnrolled.SelectedRows.Count == 0)
            {
                ShowWarning("Please select one or more students from the Enrolled list to remove.");
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to remove " + dgvEnrolled.SelectedRows.Count + " student(s) from this class?",
                "Confirm Removal",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                int classId = (int)cmbClass.SelectedValue;
                int removedCount = 0;

                foreach (DataGridViewRow row in dgvEnrolled.SelectedRows)
                {
                    int studentId = Convert.ToInt32(row.Cells["StudentId"].Value);
                    _enrollmentService.UnenrollStudent(classId, studentId);
                    removedCount++;
                }

                ShowSuccess(removedCount + " student(s) removed from class.");
                RefreshGrids();
            }
            catch (Exception ex)
            {
                ShowError("Failed to remove students:\n\n" + ex.Message);
            }
        }
    }
}
