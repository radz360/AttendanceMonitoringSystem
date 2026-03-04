using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Attendance_Monitoring_System.Models;
using Attendance_Monitoring_System.Services;

namespace Attendance_Monitoring_System.Forms
{
    public partial class ScheduleForm : BaseForm
    {
        private readonly User _currentUser;
        private readonly ScheduleService _scheduleService = new ScheduleService();
        private readonly ClassService _classService = new ClassService();
        private int _selectedScheduleId = -1;

        public ScheduleForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            // --- Event Handlers ---
            cmbClass.SelectedIndexChanged += cmbClass_SelectedIndexChanged;
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            dgvSchedules.CellClick += dgvSchedules_CellClick;
        }

        // ── Form Load ───────────────────────────────────────────
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Style delete button red
            btnDelete.BackColor = Color.FromArgb(192, 57, 43);

            // Populate day dropdown
            cmbDay.Items.AddRange(new string[]
            {
                "Monday", "Tuesday", "Wednesday", "Thursday",
                "Friday", "Saturday", "Sunday"
            });
            cmbDay.SelectedIndex = 0;

            // Populate time dropdowns (7:00 AM to 9:00 PM, 30-min intervals)
            PopulateTimeComboBoxes();

            // Apply role-based restrictions
            ApplyRolePermissions();

            // Load class dropdown
            LoadClassesComboBox();
        }

        // ── Populate Time ComboBoxes ─────────────────────────────
        private void PopulateTimeComboBoxes()
        {
            List<string> times = new List<string>();

            for (int hour = 7; hour <= 21; hour++)
            {
                for (int min = 0; min < 60; min += 30)
                {
                    int displayHour = hour > 12 ? hour - 12 : hour;
                    string amPm = hour >= 12 ? "PM" : "AM";
                    if (hour == 0) displayHour = 12;

                    string time = displayHour.ToString() + ":" + min.ToString("D2") + " " + amPm;
                    times.Add(time);
                }
            }

            cmbStartTime.Items.AddRange(times.ToArray());
            cmbEndTime.Items.AddRange(times.ToArray());

            if (cmbStartTime.Items.Count > 0) cmbStartTime.SelectedIndex = 0;
            if (cmbEndTime.Items.Count > 1) cmbEndTime.SelectedIndex = 1;
        }

        // ── Role-Based Permissions ───────────────────────────────
        private void ApplyRolePermissions()
        {
            string role = _currentUser.Role;

            if (string.Equals(role, "Teacher", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(role, "Student", StringComparison.OrdinalIgnoreCase))
            {
                // View-only: hide class selector and input panel
                lblSelectClass.Visible = false;
                cmbClass.Visible = false;
                pnlInput.Visible = false;

                // Shift grid up
                dgvSchedules.Location = new Point(20, 60);
                dgvSchedules.Height = 490;
            }
        }

        // ── Load Classes into ComboBox ───────────────────────────
        private void LoadClassesComboBox()
        {
            string role = _currentUser.Role;

            if (string.Equals(role, "Teacher", StringComparison.OrdinalIgnoreCase))
            {
                // Teacher: load only their schedules directly
                LoadTeacherSchedules();
                return;
            }
            else if (string.Equals(role, "Student", StringComparison.OrdinalIgnoreCase))
            {
                // Student: load only their schedules directly
                LoadStudentSchedules();
                return;
            }

            // Admin / Registrar: load all classes into ComboBox
            try
            {
                List<ClassInfo> classes = _classService.GetAllClasses();

                // Build display list with formatted text
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

        // ── Load Teacher Schedules (no class selector) ───────────
        private void LoadTeacherSchedules()
        {
            try
            {
                if (_currentUser.TeacherId.HasValue)
                {
                    List<ClassSchedule> schedules =
                        _scheduleService.GetSchedulesByTeacherId(_currentUser.TeacherId.Value);
                    PopulateGrid(schedules);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to load schedules:\n\n" + ex.Message);
            }
        }

        // ── Load Student Schedules (no class selector) ───────────
        private void LoadStudentSchedules()
        {
            try
            {
                if (_currentUser.StudentId.HasValue)
                {
                    List<ClassSchedule> schedules =
                        _scheduleService.GetSchedulesByStudentId(_currentUser.StudentId.Value);
                    PopulateGrid(schedules);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to load schedules:\n\n" + ex.Message);
            }
        }

        // ── Class ComboBox Changed — Load Schedules ──────────────
        private void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbClass.SelectedValue == null) return;

            try
            {
                int classId = (int)cmbClass.SelectedValue;
                List<ClassSchedule> schedules = _scheduleService.GetSchedulesByClassId(classId);
                PopulateGrid(schedules);
                ClearInputFields();
            }
            catch (Exception ex)
            {
                ShowError("Failed to load schedules:\n\n" + ex.Message);
            }
        }

        // ── Populate DataGridView ────────────────────────────────
        private void PopulateGrid(List<ClassSchedule> schedules)
        {
            dgvSchedules.DataSource = null;
            dgvSchedules.DataSource = schedules;

            if (dgvSchedules.Columns.Count > 0)
            {
                dgvSchedules.Columns["ScheduleId"].Visible = false;
                dgvSchedules.Columns["ClassId"].Visible = false;
                dgvSchedules.Columns["DayOfWeek"].Visible = false;

                dgvSchedules.Columns["DayName"].HeaderText = "Day";
                dgvSchedules.Columns["StartTime"].HeaderText = "Start Time";
                dgvSchedules.Columns["EndTime"].HeaderText = "End Time";
                dgvSchedules.Columns["Room"].HeaderText = "Room";

                // Show subject/section columns for Teacher/Student views
                if (dgvSchedules.Columns.Contains("SubjectCode"))
                {
                    bool hasSubjectData = schedules.Count > 0 && schedules[0].SubjectCode != null;
                    dgvSchedules.Columns["SubjectCode"].Visible = hasSubjectData;
                    dgvSchedules.Columns["SubjectCode"].HeaderText = "Subject Code";
                    dgvSchedules.Columns["SubjectCode"].DisplayIndex = 0;
                }
                if (dgvSchedules.Columns.Contains("SubjectName"))
                {
                    bool hasSubjectData = schedules.Count > 0 && schedules[0].SubjectName != null;
                    dgvSchedules.Columns["SubjectName"].Visible = hasSubjectData;
                    dgvSchedules.Columns["SubjectName"].HeaderText = "Subject";
                    dgvSchedules.Columns["SubjectName"].DisplayIndex = 1;
                }
                if (dgvSchedules.Columns.Contains("Section"))
                {
                    bool hasSectionData = schedules.Count > 0 && schedules[0].Section != null;
                    dgvSchedules.Columns["Section"].Visible = hasSectionData;
                    dgvSchedules.Columns["Section"].HeaderText = "Section";
                    dgvSchedules.Columns["Section"].DisplayIndex = 2;
                }
            }
        }

        // ── DataGridView Row Click — Populate Fields ─────────────
        private void dgvSchedules_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvSchedules.Rows[e.RowIndex];

            _selectedScheduleId = Convert.ToInt32(row.Cells["ScheduleId"].Value);

            // Set day ComboBox (DayOfWeek is 1-based, ComboBox is 0-based)
            int dayValue = Convert.ToInt32(row.Cells["DayOfWeek"].Value);
            cmbDay.SelectedIndex = dayValue - 1;

            // Set time ComboBoxes
            string startTime = row.Cells["StartTime"].Value.ToString();
            string endTime = row.Cells["EndTime"].Value.ToString();

            int startIdx = cmbStartTime.Items.IndexOf(startTime);
            int endIdx = cmbEndTime.Items.IndexOf(endTime);

            if (startIdx >= 0) cmbStartTime.SelectedIndex = startIdx;
            if (endIdx >= 0) cmbEndTime.SelectedIndex = endIdx;

            txtRoom.Text = row.Cells["Room"].Value.ToString();
        }

        // ── Add Schedule ─────────────────────────────────────────
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                ClassSchedule schedule = new ClassSchedule
                {
                    ClassId = (int)cmbClass.SelectedValue,
                    DayOfWeek = cmbDay.SelectedIndex + 1,
                    StartTime = cmbStartTime.SelectedItem.ToString(),
                    EndTime = cmbEndTime.SelectedItem.ToString(),
                    Room = txtRoom.Text.Trim()
                };

                _scheduleService.AddSchedule(schedule);
                ShowSuccess("Schedule added successfully.");
                ClearInputFields();
                RefreshGrid();
            }
            catch (Exception ex)
            {
                ShowError("Failed to add schedule:\n\n" + ex.Message);
            }
        }

        // ── Update Schedule ──────────────────────────────────────
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedScheduleId < 0)
            {
                ShowWarning("Please select a schedule from the list to update.");
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                ClassSchedule schedule = new ClassSchedule
                {
                    ScheduleId = _selectedScheduleId,
                    DayOfWeek = cmbDay.SelectedIndex + 1,
                    StartTime = cmbStartTime.SelectedItem.ToString(),
                    EndTime = cmbEndTime.SelectedItem.ToString(),
                    Room = txtRoom.Text.Trim()
                };

                _scheduleService.UpdateSchedule(schedule);
                ShowSuccess("Schedule updated successfully.");
                ClearInputFields();
                RefreshGrid();
            }
            catch (Exception ex)
            {
                ShowError("Failed to update schedule:\n\n" + ex.Message);
            }
        }

        // ── Delete Schedule ──────────────────────────────────────
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedScheduleId < 0)
            {
                ShowWarning("Please select a schedule from the list to delete.");
                return;
            }

            if (!ConfirmDelete("this schedule entry")) return;

            try
            {
                _scheduleService.DeleteSchedule(_selectedScheduleId);
                ShowSuccess("Schedule deleted successfully.");
                ClearInputFields();
                RefreshGrid();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Number == 1451)
                {
                    ShowError("Cannot delete this schedule because it has related attendance sessions.\n\nRemove those records first.");
                }
                else
                {
                    ShowError("Database error:\n\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to delete schedule:\n\n" + ex.Message);
            }
        }

        // ── Clear Input Fields ───────────────────────────────────
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputFields();
        }

        private void ClearInputFields()
        {
            _selectedScheduleId = -1;
            cmbDay.SelectedIndex = 0;
            if (cmbStartTime.Items.Count > 0) cmbStartTime.SelectedIndex = 0;
            if (cmbEndTime.Items.Count > 1) cmbEndTime.SelectedIndex = 1;
            txtRoom.Clear();
            txtRoom.Focus();

            dgvSchedules.ClearSelection();
        }

        // ── Refresh Grid ─────────────────────────────────────────
        private void RefreshGrid()
        {
            if (cmbClass.SelectedValue != null)
            {
                int classId = (int)cmbClass.SelectedValue;
                List<ClassSchedule> schedules = _scheduleService.GetSchedulesByClassId(classId);
                PopulateGrid(schedules);
            }
        }

        // ── Input Validation ─────────────────────────────────────
        private bool ValidateInput()
        {
            if (cmbClass.SelectedValue == null)
            {
                ShowWarning("Please select a class first.");
                cmbClass.Focus();
                return false;
            }

            if (cmbDay.SelectedIndex < 0)
            {
                ShowWarning("Please select a day of the week.");
                cmbDay.Focus();
                return false;
            }

            if (cmbStartTime.SelectedIndex < 0)
            {
                ShowWarning("Please select a start time.");
                cmbStartTime.Focus();
                return false;
            }

            if (cmbEndTime.SelectedIndex < 0)
            {
                ShowWarning("Please select an end time.");
                cmbEndTime.Focus();
                return false;
            }

            // End time must be after start time
            if (cmbEndTime.SelectedIndex <= cmbStartTime.SelectedIndex)
            {
                ShowWarning("End time must be after start time.");
                cmbEndTime.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtRoom.Text))
            {
                ShowWarning("Please enter a room.");
                txtRoom.Focus();
                return false;
            }

            return true;
        }
    }

    // ── Helper class for Class ComboBox display ──────────────
    internal class ClassDisplayItem
    {
        public int ClassId { get; set; }
        public string DisplayText { get; set; }
    }
}
