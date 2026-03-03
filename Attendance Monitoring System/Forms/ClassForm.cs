using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Attendance_Monitoring_System.Models;
using Attendance_Monitoring_System.Services;

namespace Attendance_Monitoring_System.Forms
{
    public partial class ClassForm : BaseForm
    {
        private readonly User _currentUser;
        private readonly ClassService _classService = new ClassService();
        private readonly SubjectService _subjectService = new SubjectService();
        private readonly TeacherService _teacherService = new TeacherService();
        private int _selectedClassId = -1;

        public ClassForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            // --- Event Handlers ---
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            txtSearch.TextChanged += txtSearch_TextChanged;
            dgvClasses.CellClick += dgvClasses_CellClick;
        }

        // ── Form Load ───────────────────────────────────────────
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Style delete button red
            btnDelete.BackColor = Color.FromArgb(192, 57, 43);

            // Load ComboBox data
            LoadSubjectsComboBox();
            LoadTeachersComboBox();

            // Load grid data
            LoadClasses();
        }

        // ── Load Subjects into ComboBox ──────────────────────────
        private void LoadSubjectsComboBox()
        {
            try
            {
                List<Subject> subjects = _subjectService.GetAllSubjects();

                cmbSubject.DataSource = null;
                cmbSubject.DataSource = subjects;
                cmbSubject.DisplayMember = "SubjectCode";
                cmbSubject.ValueMember = "SubjectId";

                if (cmbSubject.Items.Count > 0)
                    cmbSubject.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ShowError("Failed to load subjects:\n\n" + ex.Message);
            }
        }

        // ── Load Teachers into ComboBox ──────────────────────────
        private void LoadTeachersComboBox()
        {
            try
            {
                List<Teacher> teachers = _teacherService.GetAllTeachers();

                cmbTeacher.DataSource = null;
                cmbTeacher.DataSource = teachers;
                cmbTeacher.DisplayMember = "FullName";
                cmbTeacher.ValueMember = "TeacherId";

                if (cmbTeacher.Items.Count > 0)
                    cmbTeacher.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ShowError("Failed to load teachers:\n\n" + ex.Message);
            }
        }

        // ── Load Classes into DataGridView ───────────────────────
        private void LoadClasses()
        {
            try
            {
                List<ClassInfo> classes = _classService.GetAllClasses();
                PopulateGrid(classes);
            }
            catch (Exception ex)
            {
                ShowError("Failed to load classes:\n\n" + ex.Message);
            }
        }

        // ── Populate DataGridView ────────────────────────────────
        private void PopulateGrid(List<ClassInfo> classes)
        {
            dgvClasses.DataSource = null;
            dgvClasses.DataSource = classes;

            if (dgvClasses.Columns.Count > 0)
            {
                dgvClasses.Columns["ClassId"].Visible = false;
                dgvClasses.Columns["SubjectId"].Visible = false;
                dgvClasses.Columns["TeacherId"].Visible = false;

                dgvClasses.Columns["SubjectCode"].HeaderText = "Subject Code";
                dgvClasses.Columns["SubjectName"].HeaderText = "Subject Name";
                dgvClasses.Columns["TeacherName"].HeaderText = "Teacher";
                dgvClasses.Columns["AcademicYear"].HeaderText = "Academic Year";
                dgvClasses.Columns["Semester"].HeaderText = "Semester";
                dgvClasses.Columns["Section"].HeaderText = "Section";
            }
        }

        // ── DataGridView Row Click — Populate Fields ─────────────
        private void dgvClasses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvClasses.Rows[e.RowIndex];

            _selectedClassId = Convert.ToInt32(row.Cells["ClassId"].Value);

            // Set ComboBox selections by value
            cmbSubject.SelectedValue = Convert.ToInt32(row.Cells["SubjectId"].Value);
            cmbTeacher.SelectedValue = Convert.ToInt32(row.Cells["TeacherId"].Value);

            txtAcademicYear.Text = row.Cells["AcademicYear"].Value.ToString();
            nudSemester.Value = Convert.ToInt32(row.Cells["Semester"].Value);
            txtSection.Text = row.Cells["Section"].Value.ToString();
        }

        // ── Add Class ────────────────────────────────────────────
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                ClassInfo classInfo = new ClassInfo
                {
                    SubjectId = (int)cmbSubject.SelectedValue,
                    TeacherId = (int)cmbTeacher.SelectedValue,
                    AcademicYear = txtAcademicYear.Text.Trim(),
                    Semester = (int)nudSemester.Value,
                    Section = txtSection.Text.Trim().ToUpper()
                };

                _classService.AddClass(classInfo);
                ShowSuccess("Class added successfully.");
                ClearFields();
                LoadClasses();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    ShowWarning("This class already exists (duplicate subject, teacher, year, semester, and section).");
                }
                else
                {
                    ShowError("Database error:\n\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to add class:\n\n" + ex.Message);
            }
        }

        // ── Update Class ─────────────────────────────────────────
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedClassId < 0)
            {
                ShowWarning("Please select a class from the list to update.");
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                ClassInfo classInfo = new ClassInfo
                {
                    ClassId = _selectedClassId,
                    SubjectId = (int)cmbSubject.SelectedValue,
                    TeacherId = (int)cmbTeacher.SelectedValue,
                    AcademicYear = txtAcademicYear.Text.Trim(),
                    Semester = (int)nudSemester.Value,
                    Section = txtSection.Text.Trim().ToUpper()
                };

                _classService.UpdateClass(classInfo);
                ShowSuccess("Class updated successfully.");
                ClearFields();
                LoadClasses();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    ShowWarning("This class already exists (duplicate subject, teacher, year, semester, and section).");
                }
                else
                {
                    ShowError("Database error:\n\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to update class:\n\n" + ex.Message);
            }
        }

        // ── Delete Class ─────────────────────────────────────────
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedClassId < 0)
            {
                ShowWarning("Please select a class from the list to delete.");
                return;
            }

            if (!ConfirmDelete("this class")) return;

            try
            {
                _classService.DeleteClass(_selectedClassId);
                ShowSuccess("Class deleted successfully.");
                ClearFields();
                LoadClasses();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Number == 1451)
                {
                    ShowError("Cannot delete this class because it has related records (schedules, enrollments, attendance, etc.).\n\nRemove those records first.");
                }
                else
                {
                    ShowError("Database error:\n\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to delete class:\n\n" + ex.Message);
            }
        }

        // ── Clear Fields ─────────────────────────────────────────
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            _selectedClassId = -1;

            if (cmbSubject.Items.Count > 0)
                cmbSubject.SelectedIndex = 0;
            if (cmbTeacher.Items.Count > 0)
                cmbTeacher.SelectedIndex = 0;

            txtAcademicYear.Clear();
            nudSemester.Value = 1;
            txtSection.Clear();
            txtAcademicYear.Focus();

            dgvClasses.ClearSelection();
        }

        // ── Search / Filter ──────────────────────────────────────
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            try
            {
                List<ClassInfo> classes;

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    classes = _classService.GetAllClasses();
                }
                else
                {
                    classes = _classService.SearchClasses(keyword);
                }

                PopulateGrid(classes);
            }
            catch (Exception ex)
            {
                ShowError("Search failed:\n\n" + ex.Message);
            }
        }

        // ── Input Validation ─────────────────────────────────────
        private bool ValidateInput()
        {
            if (cmbSubject.SelectedIndex < 0)
            {
                ShowWarning("Please select a subject.");
                cmbSubject.Focus();
                return false;
            }

            if (cmbTeacher.SelectedIndex < 0)
            {
                ShowWarning("Please select a teacher.");
                cmbTeacher.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAcademicYear.Text))
            {
                ShowWarning("Please enter an academic year.");
                txtAcademicYear.Focus();
                return false;
            }

            // Validate academic year format: YYYY-YYYY
            string yearPattern = @"^\d{4}-\d{4}$";
            if (!Regex.IsMatch(txtAcademicYear.Text.Trim(), yearPattern))
            {
                ShowWarning("Academic year must be in the format YYYY-YYYY.\n\nExample: 2025-2026");
                txtAcademicYear.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSection.Text))
            {
                ShowWarning("Please enter a section.");
                txtSection.Focus();
                return false;
            }

            return true;
        }
    }
}
