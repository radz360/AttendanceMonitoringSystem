using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Attendance_Monitoring_System.Models;
using Attendance_Monitoring_System.Services;

namespace Attendance_Monitoring_System.Forms
{
    public partial class RemarkForm : BaseForm
    {
        private readonly User _currentUser;
        private readonly RemarkService _remarkService = new RemarkService();
        private readonly ClassService _classService = new ClassService();
        private readonly AttendanceService _attendanceService = new AttendanceService();
        private int _selectedRemarkId = -1;

        public RemarkForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            // --- Event Handlers ---
            cmbClass.SelectedIndexChanged += cmbClass_SelectedIndexChanged;
            cmbSession.SelectedIndexChanged += cmbSession_SelectedIndexChanged;
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            txtSearch.TextChanged += txtSearch_TextChanged;
            dgvRemarks.CellClick += dgvRemarks_CellClick;
        }

        // ── Form Load ───────────────────────────────────────────
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Style delete button red
            btnDelete.BackColor = Color.FromArgb(192, 57, 43);

            // Apply role-based restrictions
            ApplyRolePermissions();

            // Load category dropdown
            LoadCategoriesComboBox();

            // Load class dropdown (or load data directly for Student role)
            LoadClassesComboBox();
        }

        // ── Role-Based Permissions ───────────────────────────────
        private void ApplyRolePermissions()
        {
            string role = _currentUser.Role;

            if (string.Equals(role, "Student", StringComparison.OrdinalIgnoreCase))
            {
                // Student: read-only — hide input panel, show only their remarks
                pnlInput.Visible = false;
                dgvRemarks.Location = new Point(20, 90);
                dgvRemarks.Height = 490;
                lblSearch.Location = new Point(20, 52);
                txtSearch.Location = new Point(80, 49);
            }
            else if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(role, "Registrar", StringComparison.OrdinalIgnoreCase))
            {
                // Admin/Registrar: can view all and delete, but cannot add/update
                btnAdd.Enabled = false;
                btnUpdate.Enabled = false;
            }
            // Teacher: full CRUD — no restrictions
        }

        // ── Load Categories into ComboBox ────────────────────────
        private void LoadCategoriesComboBox()
        {
            try
            {
                List<RemarkCategory> categories = _remarkService.GetAllCategories();

                // Add a "(None)" option at the top
                RemarkCategory noneItem = new RemarkCategory
                {
                    CategoryId = 0,
                    CategoryName = "(None)"
                };

                List<RemarkCategory> allItems = new List<RemarkCategory>();
                allItems.Add(noneItem);
                allItems.AddRange(categories);

                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryId";
                cmbCategory.DataSource = allItems;

                cmbCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ShowError("Failed to load categories:\n\n" + ex.Message);
            }
        }

        // ── Load Classes into ComboBox ───────────────────────────
        private void LoadClassesComboBox()
        {
            string role = _currentUser.Role;

            if (string.Equals(role, "Student", StringComparison.OrdinalIgnoreCase))
            {
                // Student: load their remarks directly
                LoadStudentRemarks();
                return;
            }

            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(role, "Registrar", StringComparison.OrdinalIgnoreCase))
            {
                // Admin/Registrar: load all remarks directly, but still load class combo
                LoadAllRemarks();
            }

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

        // ── Load Student Remarks (Student role — read-only) ──────
        private void LoadStudentRemarks()
        {
            try
            {
                if (_currentUser.StudentId.HasValue)
                {
                    List<Remark> remarks = _remarkService.GetRemarksByStudentId(_currentUser.StudentId.Value);
                    PopulateGrid(remarks);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to load remarks:\n\n" + ex.Message);
            }
        }

        // ── Load All Remarks (Admin/Registrar) ───────────────────
        private void LoadAllRemarks()
        {
            try
            {
                List<Remark> remarks = _remarkService.GetAllRemarks();
                PopulateGrid(remarks);
            }
            catch (Exception ex)
            {
                ShowError("Failed to load remarks:\n\n" + ex.Message);
            }
        }

        // ── Class ComboBox Changed — Load Sessions ───────────────
        private void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbClass.SelectedValue == null || !(cmbClass.SelectedValue is int))
                return;

            try
            {
                int classId = (int)cmbClass.SelectedValue;
                List<AttendanceSession> sessions = _attendanceService.GetSessionsByClassId(classId);

                cmbSession.DisplayMember = "DisplayText";
                cmbSession.ValueMember = "SessionId";
                cmbSession.DataSource = null;

                if (sessions.Count == 0)
                {
                    cmbStudent.DataSource = null;
                    return;
                }

                // Build display items for sessions
                List<SessionDisplayItem> sessionItems = new List<SessionDisplayItem>();
                foreach (AttendanceSession s in sessions)
                {
                    sessionItems.Add(new SessionDisplayItem
                    {
                        SessionId = s.SessionId,
                        DisplayText = s.ToString() + " (by " + s.TeacherName + ")"
                    });
                }

                cmbSession.DisplayMember = "DisplayText";
                cmbSession.ValueMember = "SessionId";
                cmbSession.DataSource = sessionItems;

                if (cmbSession.Items.Count > 0)
                    cmbSession.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ShowError("Failed to load sessions:\n\n" + ex.Message);
            }
        }

        // ── Session ComboBox Changed — Load Students + Remarks ───
        private void cmbSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSession.SelectedValue == null || !(cmbSession.SelectedValue is int))
                return;

            try
            {
                int sessionId = (int)cmbSession.SelectedValue;

                // Load students who have attendance in this session
                List<SessionStudentItem> students = _remarkService.GetStudentsBySession(sessionId);

                cmbStudent.DisplayMember = "DisplayText";
                cmbStudent.ValueMember = "StudentId";
                cmbStudent.DataSource = null;

                if (students.Count > 0)
                {
                    List<StudentDisplayItem> studentItems = new List<StudentDisplayItem>();
                    foreach (SessionStudentItem s in students)
                    {
                        studentItems.Add(new StudentDisplayItem
                        {
                            StudentId = s.StudentId,
                            DisplayText = s.RegistrationNo + " - " + s.StudentName
                        });
                    }

                    cmbStudent.DisplayMember = "DisplayText";
                    cmbStudent.ValueMember = "StudentId";
                    cmbStudent.DataSource = studentItems;

                    if (cmbStudent.Items.Count > 0)
                        cmbStudent.SelectedIndex = 0;
                }

                // Load remarks for this session
                List<Remark> remarks = _remarkService.GetRemarksBySession(sessionId);
                PopulateGrid(remarks);
            }
            catch (Exception ex)
            {
                ShowError("Failed to load session data:\n\n" + ex.Message);
            }
        }

        // ── Populate DataGridView ────────────────────────────────
        private void PopulateGrid(List<Remark> remarks)
        {
            dgvRemarks.DataSource = null;
            dgvRemarks.DataSource = remarks;

            if (dgvRemarks.Columns.Count > 0)
            {
                dgvRemarks.Columns["RemarkId"].Visible = false;
                dgvRemarks.Columns["SessionId"].Visible = false;
                dgvRemarks.Columns["StudentId"].Visible = false;
                dgvRemarks.Columns["TeacherId"].Visible = false;
                dgvRemarks.Columns["CategoryId"].Visible = false;

                dgvRemarks.Columns["StudentName"].HeaderText = "Student";
                dgvRemarks.Columns["CategoryName"].HeaderText = "Category";
                dgvRemarks.Columns["RemarkText"].HeaderText = "Remark";
                dgvRemarks.Columns["TeacherName"].HeaderText = "Teacher";
                dgvRemarks.Columns["RemarkDate"].HeaderText = "Date";
            }
        }

        // ── DataGridView Row Click — Populate Fields ─────────────
        private void dgvRemarks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvRemarks.Rows[e.RowIndex];

            _selectedRemarkId = Convert.ToInt32(row.Cells["RemarkId"].Value);

            // Set category ComboBox
            object categoryValue = row.Cells["CategoryId"].Value;
            if (categoryValue != null && categoryValue != DBNull.Value)
            {
                cmbCategory.SelectedValue = Convert.ToInt32(categoryValue);
            }
            else
            {
                cmbCategory.SelectedIndex = 0; // (None)
            }

            txtRemarkText.Text = row.Cells["RemarkText"].Value.ToString();
        }

        // ── Add Remark ───────────────────────────────────────────
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            if (!_currentUser.TeacherId.HasValue || _currentUser.TeacherId.Value == 0)
            {
                ShowError("Cannot add remarks — your account is not linked to a teacher record.");
                return;
            }

            try
            {
                int categoryId = (int)cmbCategory.SelectedValue;

                Remark remark = new Remark
                {
                    SessionId = (int)cmbSession.SelectedValue,
                    StudentId = (int)cmbStudent.SelectedValue,
                    TeacherId = _currentUser.TeacherId.Value,
                    CategoryId = categoryId == 0 ? (int?)null : categoryId,
                    RemarkText = txtRemarkText.Text.Trim()
                };

                _remarkService.AddRemark(remark);
                ShowSuccess("Remark added successfully.");
                ClearFields();
                RefreshGrid();
            }
            catch (Exception ex)
            {
                ShowError("Failed to add remark:\n\n" + ex.Message);
            }
        }

        // ── Update Remark ────────────────────────────────────────
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedRemarkId < 0)
            {
                ShowWarning("Please select a remark from the list to update.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtRemarkText.Text))
            {
                ShowWarning("Please enter remark text.");
                txtRemarkText.Focus();
                return;
            }

            try
            {
                int categoryId = (int)cmbCategory.SelectedValue;

                Remark remark = new Remark
                {
                    RemarkId = _selectedRemarkId,
                    CategoryId = categoryId == 0 ? (int?)null : categoryId,
                    RemarkText = txtRemarkText.Text.Trim()
                };

                _remarkService.UpdateRemark(remark);
                ShowSuccess("Remark updated successfully.");
                ClearFields();
                RefreshGrid();
            }
            catch (Exception ex)
            {
                ShowError("Failed to update remark:\n\n" + ex.Message);
            }
        }

        // ── Delete Remark ────────────────────────────────────────
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedRemarkId < 0)
            {
                ShowWarning("Please select a remark from the list to delete.");
                return;
            }

            if (!ConfirmDelete("this remark")) return;

            try
            {
                _remarkService.DeleteRemark(_selectedRemarkId);
                ShowSuccess("Remark deleted successfully.");
                ClearFields();
                RefreshGrid();
            }
            catch (Exception ex)
            {
                ShowError("Failed to delete remark:\n\n" + ex.Message);
            }
        }

        // ── Clear Fields ─────────────────────────────────────────
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            _selectedRemarkId = -1;
            cmbCategory.SelectedIndex = 0;
            txtRemarkText.Clear();
            txtRemarkText.Focus();
            dgvRemarks.ClearSelection();
        }

        // ── Refresh Grid ─────────────────────────────────────────
        private void RefreshGrid()
        {
            string role = _currentUser.Role;

            if (string.Equals(role, "Student", StringComparison.OrdinalIgnoreCase))
            {
                LoadStudentRemarks();
                return;
            }

            if (string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(role, "Registrar", StringComparison.OrdinalIgnoreCase))
            {
                // If a session is selected, show that session's remarks; otherwise all
                if (cmbSession.SelectedValue != null && cmbSession.SelectedValue is int)
                {
                    int sessionId = (int)cmbSession.SelectedValue;
                    List<Remark> remarks = _remarkService.GetRemarksBySession(sessionId);
                    PopulateGrid(remarks);
                }
                else
                {
                    LoadAllRemarks();
                }
                return;
            }

            // Teacher: refresh by session
            if (cmbSession.SelectedValue != null && cmbSession.SelectedValue is int)
            {
                int sessionId = (int)cmbSession.SelectedValue;
                List<Remark> remarks = _remarkService.GetRemarksBySession(sessionId);
                PopulateGrid(remarks);
            }
        }

        // ── Search / Filter ──────────────────────────────────────
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            try
            {
                List<Remark> remarks;

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    // Restore default view based on role
                    RefreshGrid();
                    return;
                }
                else
                {
                    remarks = _remarkService.SearchRemarks(keyword);
                }

                PopulateGrid(remarks);
            }
            catch (Exception ex)
            {
                ShowError("Search failed:\n\n" + ex.Message);
            }
        }

        // ── Input Validation ─────────────────────────────────────
        private bool ValidateInput()
        {
            if (cmbClass.SelectedValue == null || !(cmbClass.SelectedValue is int))
            {
                ShowWarning("Please select a class.");
                cmbClass.Focus();
                return false;
            }

            if (cmbSession.SelectedValue == null || !(cmbSession.SelectedValue is int))
            {
                ShowWarning("Please select an attendance session.\n\nIf no sessions exist, create one in the Attendance form first.");
                cmbSession.Focus();
                return false;
            }

            if (cmbStudent.SelectedValue == null || !(cmbStudent.SelectedValue is int))
            {
                ShowWarning("Please select a student.");
                cmbStudent.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtRemarkText.Text))
            {
                ShowWarning("Please enter remark text.");
                txtRemarkText.Focus();
                return false;
            }

            return true;
        }
    }

    // ── Helper class for Session ComboBox display ────────────
    internal class SessionDisplayItem
    {
        public int SessionId { get; set; }
        public string DisplayText { get; set; }
    }

    // ── Helper class for Student ComboBox display ────────────
    internal class StudentDisplayItem
    {
        public int StudentId { get; set; }
        public string DisplayText { get; set; }
    }
}
