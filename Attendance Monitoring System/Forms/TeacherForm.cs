using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Attendance_Monitoring_System.Models;
using Attendance_Monitoring_System.Services;

namespace Attendance_Monitoring_System.Forms
{
    public partial class TeacherForm : BaseForm
    {
        private readonly User _currentUser;
        private readonly TeacherService _teacherService = new TeacherService();
        private int _selectedTeacherId = -1;

        public TeacherForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            // --- Event Handlers ---
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            txtSearch.TextChanged += txtSearch_TextChanged;
            dgvTeachers.CellClick += dgvTeachers_CellClick;
        }

        // ── Form Load ───────────────────────────────────────────
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Style delete button red
            btnDelete.BackColor = Color.FromArgb(192, 57, 43);

            // Load data
            LoadTeachers();
        }

        // ── Load Teachers into DataGridView ──────────────────────
        private void LoadTeachers()
        {
            try
            {
                List<Teacher> teachers = _teacherService.GetAllTeachers();
                PopulateGrid(teachers);
            }
            catch (Exception ex)
            {
                ShowError("Failed to load teachers:\n\n" + ex.Message);
            }
        }

        // ── Populate DataGridView ────────────────────────────────
        private void PopulateGrid(List<Teacher> teachers)
        {
            dgvTeachers.DataSource = null;
            dgvTeachers.DataSource = teachers;

            if (dgvTeachers.Columns.Count > 0)
            {
                dgvTeachers.Columns["TeacherId"].Visible = false;
                dgvTeachers.Columns["FullName"].Visible = false;

                dgvTeachers.Columns["FirstName"].HeaderText = "First Name";
                dgvTeachers.Columns["LastName"].HeaderText = "Last Name";
                dgvTeachers.Columns["Email"].HeaderText = "Email";
                dgvTeachers.Columns["Designation"].HeaderText = "Designation";
            }
        }

        // ── DataGridView Row Click — Populate Fields ─────────────
        private void dgvTeachers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvTeachers.Rows[e.RowIndex];

            _selectedTeacherId = Convert.ToInt32(row.Cells["TeacherId"].Value);
            txtFirstName.Text = row.Cells["FirstName"].Value.ToString();
            txtLastName.Text = row.Cells["LastName"].Value.ToString();
            txtEmail.Text = row.Cells["Email"].Value.ToString();
            txtDesignation.Text = row.Cells["Designation"].Value.ToString();
        }

        // ── Add Teacher ──────────────────────────────────────────
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                Teacher teacher = new Teacher
                {
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Designation = txtDesignation.Text.Trim()
                };

                _teacherService.AddTeacher(teacher);
                ShowSuccess("Teacher added successfully.");
                ClearFields();
                LoadTeachers();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    ShowWarning("A teacher with this email already exists.");
                }
                else
                {
                    ShowError("Database error:\n\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to add teacher:\n\n" + ex.Message);
            }
        }

        // ── Update Teacher ───────────────────────────────────────
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedTeacherId < 0)
            {
                ShowWarning("Please select a teacher from the list to update.");
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                Teacher teacher = new Teacher
                {
                    TeacherId = _selectedTeacherId,
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Designation = txtDesignation.Text.Trim()
                };

                _teacherService.UpdateTeacher(teacher);
                ShowSuccess("Teacher updated successfully.");
                ClearFields();
                LoadTeachers();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    ShowWarning("A teacher with this email already exists.");
                }
                else
                {
                    ShowError("Database error:\n\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to update teacher:\n\n" + ex.Message);
            }
        }

        // ── Delete Teacher ───────────────────────────────────────
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedTeacherId < 0)
            {
                ShowWarning("Please select a teacher from the list to delete.");
                return;
            }

            if (!ConfirmDelete("this teacher")) return;

            try
            {
                _teacherService.DeleteTeacher(_selectedTeacherId);
                ShowSuccess("Teacher deleted successfully.");
                ClearFields();
                LoadTeachers();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Number == 1451)
                {
                    ShowError("Cannot delete this teacher because they have related records (classes, attendance, etc.).\n\nRemove those records first.");
                }
                else
                {
                    ShowError("Database error:\n\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to delete teacher:\n\n" + ex.Message);
            }
        }

        // ── Clear Fields ─────────────────────────────────────────
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            _selectedTeacherId = -1;
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            txtDesignation.Clear();
            txtFirstName.Focus();

            dgvTeachers.ClearSelection();
        }

        // ── Search / Filter ──────────────────────────────────────
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            try
            {
                List<Teacher> teachers;

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    teachers = _teacherService.GetAllTeachers();
                }
                else
                {
                    teachers = _teacherService.SearchTeachers(keyword);
                }

                PopulateGrid(teachers);
            }
            catch (Exception ex)
            {
                ShowError("Search failed:\n\n" + ex.Message);
            }
        }

        // ── Input Validation ─────────────────────────────────────
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                ShowWarning("Please enter a first name.");
                txtFirstName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                ShowWarning("Please enter a last name.");
                txtLastName.Focus();
                return false;
            }

            // Email is optional but if provided, must be valid format
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!Regex.IsMatch(txtEmail.Text.Trim(), emailPattern))
                {
                    ShowWarning("Please enter a valid email address.\n\nExample: teacher@school.edu");
                    txtEmail.Focus();
                    return false;
                }
            }

            return true;
        }
    }
}