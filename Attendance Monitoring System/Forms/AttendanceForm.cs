using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Attendance_Monitoring_System.Models;
using Attendance_Monitoring_System.Services;

namespace Attendance_Monitoring_System.Forms
{
    public partial class AttendanceForm : BaseForm
    {
        private readonly User _currentUser;
        private readonly AttendanceService _attendanceService = new AttendanceService();
        private readonly ClassService _classService = new ClassService();
        private readonly EnrollmentService _enrollmentService = new EnrollmentService();

        public AttendanceForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            // --- Event Handlers ---
            cmbClass.SelectedIndexChanged += cmbClass_SelectedIndexChanged;
            cmbSession.SelectedIndexChanged += cmbSession_SelectedIndexChanged;
            btnNewSession.Click += btnNewSession_Click;
            btnRenameSession.Click += btnRenameSession_Click;
            btnDeleteSession.Click += btnDeleteSession_Click;
            btnSave.Click += btnSave_Click;
        }

        // ── Form Load ───────────────────────────────────────────
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Style delete button red, rename button muted
            btnDeleteSession.BackColor = Color.FromArgb(192, 57, 43);
            btnRenameSession.BackColor = Color.FromArgb(127, 140, 141);

            // Save button distinct green
            btnSave.BackColor = Color.FromArgb(39, 174, 96);
            btnSave.Font = new Font(btnSave.Font.FontFamily, 10.5f, FontStyle.Bold);

            // Build the attendance grid columns
            SetupAttendanceGrid();

            // Apply role-based restrictions
            ApplyRolePermissions();

            // Load classes based on role
            LoadClassesComboBox();
        }

        // ── Role-Based Permissions ───────────────────────────────
        private void ApplyRolePermissions()
        {
            string role = _currentUser.Role;

            if (string.Equals(role, "Student", StringComparison.OrdinalIgnoreCase))
            {
                // Student: read-only view
                btnNewSession.Visible = false;
                btnRenameSession.Visible = false;
                btnDeleteSession.Visible = false;
                btnSave.Visible = false;
                lblSummary.Location = new Point(20, 508);
            }
        }

        // ── Setup DataGridView Columns ───────────────────────────
        private void SetupAttendanceGrid()
        {
            dgvAttendance.Columns.Clear();

            // Hidden identity columns
            dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RecordId",
                Visible = false
            });
            dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StudentId",
                Visible = false
            });

            // Student ID (registration_no) — read-only
            DataGridViewTextBoxColumn colStudentId = new DataGridViewTextBoxColumn
            {
                Name = "StudentIdDisplay",
                HeaderText = "Student ID",
                ReadOnly = true,
                FillWeight = 20
            };
            dgvAttendance.Columns.Add(colStudentId);

            // Student Name — read-only
            DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn
            {
                Name = "StudentName",
                HeaderText = "Student Name",
                ReadOnly = true,
                FillWeight = 30
            };
            dgvAttendance.Columns.Add(colName);

            // Status — ComboBox column with constrained width
            DataGridViewComboBoxColumn colStatus = new DataGridViewComboBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                FillWeight = 15,
                FlatStyle = FlatStyle.Flat
            };
            colStatus.Items.AddRange("Present", "Absent", "Late", "Excused");
            dgvAttendance.Columns.Add(colStatus);

            // Time In — editable text box (optional)
            DataGridViewTextBoxColumn colTimeIn = new DataGridViewTextBoxColumn
            {
                Name = "TimeIn",
                HeaderText = "Time In (optional)",
                FillWeight = 15
            };
            dgvAttendance.Columns.Add(colTimeIn);

            // Marked At — read-only timestamp
            DataGridViewTextBoxColumn colMarkedAt = new DataGridViewTextBoxColumn
            {
                Name = "MarkedAt",
                HeaderText = "Last Saved",
                ReadOnly = true,
                FillWeight = 20,
                DefaultCellStyle = { ForeColor = Color.Gray }
            };
            dgvAttendance.Columns.Add(colMarkedAt);
        }

        // ── Load Classes into ComboBox ───────────────────────────
        private void LoadClassesComboBox()
        {
            try
            {
                List<ClassInfo> classes;

                if (string.Equals(_currentUser.Role, "Teacher", StringComparison.OrdinalIgnoreCase)
                    && _currentUser.TeacherId.HasValue)
                {
                    classes = _classService.GetClassesByTeacherId(_currentUser.TeacherId.Value);
                }
                else
                {
                    classes = _classService.GetAllClasses();
                }

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

        // ── Class Selection Changed — Load Sessions ──────────────
        private void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbClass.SelectedValue == null) return;
            LoadSessionsComboBox();
        }

        // ── Load Sessions for Selected Class ────────────────────
        private void LoadSessionsComboBox()
        {
            try
            {
                int classId = (int)cmbClass.SelectedValue;
                List<AttendanceSession> sessions = _attendanceService.GetSessionsByClassId(classId);

                cmbSession.DataSource = null;
                cmbSession.DataSource = sessions;

                if (cmbSession.Items.Count > 0)
                    cmbSession.SelectedIndex = 0;
                else
                {
                    dgvAttendance.Rows.Clear();
                    UpdateSummary();
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to load sessions:\n\n" + ex.Message);
            }
        }

        // ── Session Selection Changed — Load Attendance Records ──
        private void cmbSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            AttendanceSession selected = cmbSession.SelectedItem as AttendanceSession;
            if (selected == null) return;
            LoadAttendanceRecords(selected.SessionId);
        }

        // ── Load Attendance Records into Grid ────────────────────
        private void LoadAttendanceRecords(int sessionId)
        {
            try
            {
                List<AttendanceRecord> records = _attendanceService.GetAttendanceRecords(sessionId);

                dgvAttendance.Rows.Clear();

                foreach (AttendanceRecord rec in records)
                {
                    string timeInText = rec.TimeIn.HasValue
                        ? rec.TimeIn.Value.ToString("HH:mm")
                        : string.Empty;

                    string markedAtText = rec.MarkedAt.ToString("yyyy-MM-dd HH:mm");

                    dgvAttendance.Rows.Add(
                        rec.RecordId,
                        rec.StudentId,
                        rec.RegistrationNo,
                        rec.StudentName,
                        rec.Status,
                        timeInText,
                        markedAtText);
                }

                UpdateSummary();
            }
            catch (Exception ex)
            {
                ShowError("Failed to load attendance records:\n\n" + ex.Message);
            }
        }

        // ── Update Summary Counts ────────────────────────────────
        private void UpdateSummary()
        {
            int present = 0, absent = 0, late = 0, excused = 0;

            foreach (DataGridViewRow row in dgvAttendance.Rows)
            {
                string status = row.Cells["Status"].Value != null
                    ? row.Cells["Status"].Value.ToString()
                    : "";

                switch (status)
                {
                    case "Present": present++; break;
                    case "Absent": absent++; break;
                    case "Late": late++; break;
                    case "Excused": excused++; break;
                }
            }

            int total = dgvAttendance.Rows.Count;
            lblSummary.Text = "Total: " + total +
                "   |   ✔ Present: " + present +
                "   |   ✖ Absent: " + absent +
                "   |   ⏰ Late: " + late +
                "   |   📋 Excused: " + excused;
        }

        // ── New Session Button ───────────────────────────────────
        private void btnNewSession_Click(object sender, EventArgs e)
        {
            if (cmbClass.SelectedValue == null)
            {
                ShowWarning("Please select a class first.");
                return;
            }

            int classId = (int)cmbClass.SelectedValue;

            // Determine teacher id
            int teacherId = _currentUser.TeacherId ?? 0;
            if (teacherId == 0)
            {
                // Admin/Registrar: try to get teacher_id from the selected class
                ClassInfo classInfo = _classService.GetClassById(classId);
                if (classInfo != null)
                    teacherId = classInfo.TeacherId;
            }

            if (teacherId == 0)
            {
                ShowWarning("Cannot determine the teacher for this class.\nMake sure the class has an assigned teacher.");
                return;
            }

            try
            {
                AttendanceSession newSession = new AttendanceSession
                {
                    ClassId = classId,
                    SessionName = null,
                    SessionDate = DateTime.Today,
                    CreatedByTeacherId = teacherId
                };

                int sessionId = _attendanceService.CreateSession(newSession);
                if (sessionId < 0)
                {
                    ShowError("Failed to create session.");
                    return;
                }

                // Auto-populate enrolled students with "Absent"
                List<Student> enrolled = _enrollmentService.GetEnrolledStudents(classId);
                foreach (Student student in enrolled)
                {
                    AttendanceRecord record = new AttendanceRecord
                    {
                        SessionId = sessionId,
                        StudentId = student.StudentId,
                        Status = "Absent",
                        TimeIn = null
                    };
                    _attendanceService.SaveAttendanceRecord(record);
                }

                ShowSuccess("Session created with " + enrolled.Count + " student(s) defaulted to Absent.\nUse 'Save All Changes' after updating statuses.");

                // Reload sessions and select the new one
                LoadSessionsComboBox();

                // Select the newly created session
                for (int i = 0; i < cmbSession.Items.Count; i++)
                {
                    AttendanceSession s = cmbSession.Items[i] as AttendanceSession;
                    if (s != null && s.SessionId == sessionId)
                    {
                        cmbSession.SelectedIndex = i;
                        break;
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    ShowWarning("A session for this class on today's date already exists.");
                }
                else
                {
                    ShowError("Database error:\n\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to create session:\n\n" + ex.Message);
            }
        }

        // ── Rename Session Button ────────────────────────────────
        private void btnRenameSession_Click(object sender, EventArgs e)
        {
            AttendanceSession selected = cmbSession.SelectedItem as AttendanceSession;
            if (selected == null)
            {
                ShowWarning("Please select a session to rename.");
                return;
            }

            string currentName = selected.SessionName ?? "";

            using (Form dialog = new Form())
            {
                dialog.Text = "Rename Session";
                dialog.Size = new Size(420, 170);
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                Label lbl = new Label
                {
                    Text = "Session name (leave blank for default):",
                    Location = new Point(15, 15),
                    AutoSize = true
                };

                TextBox txt = new TextBox
                {
                    Location = new Point(15, 40),
                    Size = new Size(370, 25),
                    Text = currentName,
                    MaxLength = 100
                };

                Button btnOk = new Button
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Location = new Point(220, 80),
                    Size = new Size(80, 30)
                };

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(305, 80),
                    Size = new Size(80, 30)
                };

                dialog.Controls.Add(lbl);
                dialog.Controls.Add(txt);
                dialog.Controls.Add(btnOk);
                dialog.Controls.Add(btnCancel);
                dialog.AcceptButton = btnOk;
                dialog.CancelButton = btnCancel;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        string newName = txt.Text.Trim();
                        _attendanceService.UpdateSessionName(selected.SessionId, newName);

                        // Reload sessions — preserve selection
                        int selectedSessionId = selected.SessionId;
                        LoadSessionsComboBox();

                        for (int i = 0; i < cmbSession.Items.Count; i++)
                        {
                            AttendanceSession s = cmbSession.Items[i] as AttendanceSession;
                            if (s != null && s.SessionId == selectedSessionId)
                            {
                                cmbSession.SelectedIndex = i;
                                break;
                            }
                        }

                        ShowSuccess("Session renamed successfully.");
                    }
                    catch (Exception ex)
                    {
                        ShowError("Failed to rename session:\n\n" + ex.Message);
                    }
                }
            }
        }

        // ── Delete Session Button ────────────────────────────────
        private void btnDeleteSession_Click(object sender, EventArgs e)
        {
            AttendanceSession selected = cmbSession.SelectedItem as AttendanceSession;
            if (selected == null)
            {
                ShowWarning("Please select a session to delete.");
                return;
            }

            if (!ConfirmDelete("session \"" + selected.ToString() + "\""))
                return;

            try
            {
                _attendanceService.DeleteSession(selected.SessionId);
                ShowSuccess("Session deleted successfully.");
                LoadSessionsComboBox();
                dgvAttendance.Rows.Clear();
                UpdateSummary();
            }
            catch (Exception ex)
            {
                ShowError("Failed to delete session:\n\n" + ex.Message);
            }
        }

        // ── Save Attendance Button ───────────────────────────────
        private void btnSave_Click(object sender, EventArgs e)
        {
            AttendanceSession selected = cmbSession.SelectedItem as AttendanceSession;
            if (selected == null)
            {
                ShowWarning("Please select a session first.");
                return;
            }

            if (dgvAttendance.Rows.Count == 0)
            {
                ShowWarning("There are no attendance records to save.");
                return;
            }

            try
            {
                int savedCount = 0;

                foreach (DataGridViewRow row in dgvAttendance.Rows)
                {
                    int studentId = Convert.ToInt32(row.Cells["StudentId"].Value);
                    string status = row.Cells["Status"].Value != null
                        ? row.Cells["Status"].Value.ToString()
                        : "Absent";

                    string timeInText = row.Cells["TimeIn"].Value != null
                        ? row.Cells["TimeIn"].Value.ToString().Trim()
                        : string.Empty;

                    DateTime? timeIn = null;
                    if (!string.IsNullOrEmpty(timeInText))
                    {
                        DateTime parsed;
                        if (DateTime.TryParse(timeInText, out parsed))
                            timeIn = parsed;
                        else if (DateTime.TryParseExact(timeInText,
                            new[] { "HH:mm", "H:mm", "h:mm tt", "hh:mm tt" },
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None, out parsed))
                            timeIn = parsed;
                    }

                    AttendanceRecord record = new AttendanceRecord
                    {
                        SessionId = selected.SessionId,
                        StudentId = studentId,
                        Status = status,
                        TimeIn = timeIn
                    };

                    _attendanceService.SaveAttendanceRecord(record);
                    savedCount++;
                }

                ShowSuccess("Saved " + savedCount + " record(s) to the database.");

                // Reload to show updated "Last Saved" timestamps
                LoadAttendanceRecords(selected.SessionId);
            }
            catch (Exception ex)
            {
                ShowError("Failed to save attendance:\n\n" + ex.Message);
            }
        }
    }
}
