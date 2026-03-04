using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Attendance_Monitoring_System.Models;
using Attendance_Monitoring_System.Services;

namespace Attendance_Monitoring_System.Forms
{
    public partial class StudentForm : BaseForm
    {
        private readonly User _currentUser;
        private readonly StudentService _studentService = new StudentService();
        private int _selectedStudentId = -1;

        public StudentForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            // --- Event Handlers ---
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            txtSearch.TextChanged += txtSearch_TextChanged;
            dgvStudents.CellClick += dgvStudents_CellClick;
        }

        // ── Form Load ───────────────────────────────────────────
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Populate gender dropdown
            cmbGender.Items.AddRange(new string[] { "Male", "Female", "Other" });
            cmbGender.SelectedIndex = 0;

            // Style delete button red
            btnDelete.BackColor = Color.FromArgb(192, 57, 43);

            // Apply role-based restrictions
            ApplyRolePermissions();

            // Load data
            LoadStudents();
        }

        // ── Role-Based Permissions ───────────────────────────────
        private void ApplyRolePermissions()
        {
            string role = _currentUser.Role;

            if (string.Equals(role, "Teacher", StringComparison.OrdinalIgnoreCase))
            {
                // Teacher: View-only — hide input panel entirely
                pnlInput.Visible = false;

                // Shift search and grid up to fill the space
                lblSearch.Location = new Point(20, 60);
                txtSearch.Location = new Point(80, 57);
                dgvStudents.Location = new Point(20, 90);
                dgvStudents.Height = 460;
            }
            // Admin and Registrar get full CRUD — no changes needed
        }

        // ── Load Students into DataGridView ──────────────────────
        private void LoadStudents()
        {
            try
            {
                List<Student> students;

                if (string.Equals(_currentUser.Role, "Teacher", StringComparison.OrdinalIgnoreCase))
                {
                    // Teacher sees only their own students
                    if (_currentUser.TeacherId.HasValue)
                    {
                        students = _studentService.GetStudentsByTeacherId(_currentUser.TeacherId.Value);
                    }
                    else
                    {
                        students = new List<Student>();
                    }
                }
                else
                {
                    // Admin and Registrar see all students
                    students = _studentService.GetAllStudents();
                }

                PopulateGrid(students);
            }
            catch (Exception ex)
            {
                ShowError("Failed to load students:\n\n" + ex.Message);
            }
        }

        // ── Populate DataGridView ────────────────────────────────
        private void PopulateGrid(List<Student> students)
        {
            dgvStudents.DataSource = null;
            dgvStudents.DataSource = students;

            if (dgvStudents.Columns.Count > 0)
            {
                // Hide the StudentId column — internal use only
                dgvStudents.Columns["StudentId"].Visible = false;

                // Set friendly header names
                dgvStudents.Columns["RegistrationNo"].HeaderText = "Registration No";
                dgvStudents.Columns["FirstName"].HeaderText = "First Name";
                dgvStudents.Columns["LastName"].HeaderText = "Last Name";
                dgvStudents.Columns["Gender"].HeaderText = "Gender";
                dgvStudents.Columns["DateOfBirth"].HeaderText = "Date of Birth";
                dgvStudents.Columns["DateOfBirth"].DefaultCellStyle.Format = "MM-dd-yyyy";
                dgvStudents.Columns["FullName"].Visible = false;
            }
        }

        // ── DataGridView Row Click — Populate Fields ─────────────
        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvStudents.Rows[e.RowIndex];

            _selectedStudentId = Convert.ToInt32(row.Cells["StudentId"].Value);
            txtRegNo.Text = row.Cells["RegistrationNo"].Value.ToString();
            txtFirstName.Text = row.Cells["FirstName"].Value.ToString();
            txtLastName.Text = row.Cells["LastName"].Value.ToString();
            cmbGender.SelectedItem = row.Cells["Gender"].Value.ToString();
            dtpDateOfBirth.Value = Convert.ToDateTime(row.Cells["DateOfBirth"].Value);
        }

        // ── Add Student ──────────────────────────────────────────
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                Student student = new Student
                {
                    RegistrationNo = txtRegNo.Text.Trim(),
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    Gender = cmbGender.SelectedItem.ToString(),
                    DateOfBirth = dtpDateOfBirth.Value.Date
                };

                _studentService.AddStudent(student);
                ShowSuccess("Student added successfully.");
                ClearFields();
                LoadStudents();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Number == 1062) // Duplicate entry
                {
                    ShowWarning("A student with this registration number already exists.");
                }
                else
                {
                    ShowError("Database error:\n\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to add student:\n\n" + ex.Message);
            }
        }

        // ── Update Student ───────────────────────────────────────
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedStudentId < 0)
            {
                ShowWarning("Please select a student from the list to update.");
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                Student student = new Student
                {
                    StudentId = _selectedStudentId,
                    RegistrationNo = txtRegNo.Text.Trim(),
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    Gender = cmbGender.SelectedItem.ToString(),
                    DateOfBirth = dtpDateOfBirth.Value.Date
                };

                _studentService.UpdateStudent(student);
                ShowSuccess("Student updated successfully.");
                ClearFields();
                LoadStudents();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    ShowWarning("A student with this registration number already exists.");
                }
                else
                {
                    ShowError("Database error:\n\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to update student:\n\n" + ex.Message);
            }
        }

        // ── Delete Student ───────────────────────────────────────
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedStudentId < 0)
            {
                ShowWarning("Please select a student from the list to delete.");
                return;
            }

            if (!ConfirmDelete("this student")) return;

            try
            {
                _studentService.DeleteStudent(_selectedStudentId);
                ShowSuccess("Student deleted successfully.");
                ClearFields();
                LoadStudents();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Number == 1451) // Foreign key constraint
                {
                    ShowError("Cannot delete this student because they have related records (enrollments, attendance, etc.).\n\nRemove those records first.");
                }
                else
                {
                    ShowError("Database error:\n\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to delete student:\n\n" + ex.Message);
            }
        }

        // ── Clear Fields ─────────────────────────────────────────
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            _selectedStudentId = -1;
            txtRegNo.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            cmbGender.SelectedIndex = 0;
            dtpDateOfBirth.Value = DateTime.Today;
            txtRegNo.Focus();

            // Deselect any row in the grid
            dgvStudents.ClearSelection();
        }

        // ── Search / Filter ──────────────────────────────────────
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            try
            {
                List<Student> students;

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    // Empty search — reload all based on role
                    if (string.Equals(_currentUser.Role, "Teacher", StringComparison.OrdinalIgnoreCase)
                        && _currentUser.TeacherId.HasValue)
                    {
                        students = _studentService.GetStudentsByTeacherId(_currentUser.TeacherId.Value);
                    }
                    else
                    {
                        students = _studentService.GetAllStudents();
                    }
                }
                else
                {
                    students = _studentService.SearchStudents(keyword);

                    // If Teacher, filter search results to only their students
                    if (string.Equals(_currentUser.Role, "Teacher", StringComparison.OrdinalIgnoreCase)
                        && _currentUser.TeacherId.HasValue)
                    {
                        List<Student> myStudents = _studentService.GetStudentsByTeacherId(_currentUser.TeacherId.Value);
                        HashSet<int> myStudentIds = new HashSet<int>();

                        foreach (Student s in myStudents)
                        {
                            myStudentIds.Add(s.StudentId);
                        }

                        List<Student> filtered = new List<Student>();
                        foreach (Student s in students)
                        {
                            if (myStudentIds.Contains(s.StudentId))
                            {
                                filtered.Add(s);
                            }
                        }

                        students = filtered;
                    }
                }

                PopulateGrid(students);
            }
            catch (Exception ex)
            {
                ShowError("Search failed:\n\n" + ex.Message);
            }
        }

        // ── Input Validation ─────────────────────────────────────
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtRegNo.Text))
            {
                ShowWarning("Please enter a registration number.");
                txtRegNo.Focus();
                return false;
            }

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

            if (cmbGender.SelectedIndex < 0)
            {
                ShowWarning("Please select a gender.");
                cmbGender.Focus();
                return false;
            }

            if (dtpDateOfBirth.Value.Date >= DateTime.Today)
            {
                ShowWarning("Date of birth must be in the past.");
                dtpDateOfBirth.Focus();
                return false;
            }

            return true;
        }
    }
}