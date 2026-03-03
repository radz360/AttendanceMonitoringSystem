using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Attendance_Monitoring_System.Models;
using Attendance_Monitoring_System.Services;

namespace Attendance_Monitoring_System.Forms
{
    public partial class SubjectForm : BaseForm
    {
        private readonly User _currentUser;
        private readonly SubjectService _subjectService = new SubjectService();
        private int _selectedSubjectId = -1;

        public SubjectForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            // --- Event Handlers ---
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            txtSearch.TextChanged += txtSearch_TextChanged;
            dgvSubjects.CellClick += dgvSubjects_CellClick;
        }

        // ── Form Load ───────────────────────────────────────────
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Style delete button red
            btnDelete.BackColor = Color.FromArgb(192, 57, 43);

            // Load data
            LoadSubjects();
        }

        // ── Load Subjects into DataGridView ──────────────────────
        private void LoadSubjects()
        {
            try
            {
                List<Subject> subjects = _subjectService.GetAllSubjects();
                PopulateGrid(subjects);
            }
            catch (Exception ex)
            {
                ShowError("Failed to load subjects:\n\n" + ex.Message);
            }
        }

        // ── Populate DataGridView ────────────────────────────────
        private void PopulateGrid(List<Subject> subjects)
        {
            dgvSubjects.DataSource = null;
            dgvSubjects.DataSource = subjects;

            if (dgvSubjects.Columns.Count > 0)
            {
                dgvSubjects.Columns["SubjectId"].Visible = false;

                dgvSubjects.Columns["SubjectCode"].HeaderText = "Subject Code";
                dgvSubjects.Columns["SubjectName"].HeaderText = "Subject Name";
            }
        }

        // ── DataGridView Row Click — Populate Fields ─────────────
        private void dgvSubjects_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvSubjects.Rows[e.RowIndex];

            _selectedSubjectId = Convert.ToInt32(row.Cells["SubjectId"].Value);
            txtSubjectCode.Text = row.Cells["SubjectCode"].Value.ToString();
            txtSubjectName.Text = row.Cells["SubjectName"].Value.ToString();
        }

        // ── Add Subject ──────────────────────────────────────────
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                Subject subject = new Subject
                {
                    SubjectCode = txtSubjectCode.Text.Trim().ToUpper(),
                    SubjectName = txtSubjectName.Text.Trim()
                };

                _subjectService.AddSubject(subject);
                ShowSuccess("Subject added successfully.");
                ClearFields();
                LoadSubjects();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    ShowWarning("A subject with this code already exists.");
                }
                else
                {
                    ShowError("Database error:\n\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to add subject:\n\n" + ex.Message);
            }
        }

        // ── Update Subject ───────────────────────────────────────
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedSubjectId < 0)
            {
                ShowWarning("Please select a subject from the list to update.");
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                Subject subject = new Subject
                {
                    SubjectId = _selectedSubjectId,
                    SubjectCode = txtSubjectCode.Text.Trim().ToUpper(),
                    SubjectName = txtSubjectName.Text.Trim()
                };

                _subjectService.UpdateSubject(subject);
                ShowSuccess("Subject updated successfully.");
                ClearFields();
                LoadSubjects();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    ShowWarning("A subject with this code already exists.");
                }
                else
                {
                    ShowError("Database error:\n\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to update subject:\n\n" + ex.Message);
            }
        }

        // ── Delete Subject ───────────────────────────────────────
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedSubjectId < 0)
            {
                ShowWarning("Please select a subject from the list to delete.");
                return;
            }

            if (!ConfirmDelete("this subject")) return;

            try
            {
                _subjectService.DeleteSubject(_selectedSubjectId);
                ShowSuccess("Subject deleted successfully.");
                ClearFields();
                LoadSubjects();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Number == 1451)
                {
                    ShowError("Cannot delete this subject because it is assigned to one or more classes.\n\nRemove those class assignments first.");
                }
                else
                {
                    ShowError("Database error:\n\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to delete subject:\n\n" + ex.Message);
            }
        }

        // ── Clear Fields ─────────────────────────────────────────
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            _selectedSubjectId = -1;
            txtSubjectCode.Clear();
            txtSubjectName.Clear();
            txtSubjectCode.Focus();

            dgvSubjects.ClearSelection();
        }

        // ── Search / Filter ──────────────────────────────────────
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            try
            {
                List<Subject> subjects;

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    subjects = _subjectService.GetAllSubjects();
                }
                else
                {
                    subjects = _subjectService.SearchSubjects(keyword);
                }

                PopulateGrid(subjects);
            }
            catch (Exception ex)
            {
                ShowError("Search failed:\n\n" + ex.Message);
            }
        }

        // ── Input Validation ─────────────────────────────────────
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtSubjectCode.Text))
            {
                ShowWarning("Please enter a subject code.");
                txtSubjectCode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSubjectName.Text))
            {
                ShowWarning("Please enter a subject name.");
                txtSubjectName.Focus();
                return false;
            }

            return true;
        }
    }
}
