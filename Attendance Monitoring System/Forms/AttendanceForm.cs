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
            btnDeleteSession.Click += btnDeleteSession_Click;
            btnSave.Click += btnSave_Click;
        }

        // ── Form Load ───────────────────────────────────────────
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Style delete button red
            btnDeleteSession.BackColor = Color.FromArgb(192, 57, 43);

            // Build the attendance grid columns
            SetupAttendanceGrid();

            // Load classes based on role
            LoadClassesComboBox();
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

            // Reg. No — read-only
            dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RegistrationNo",
                HeaderText = "Reg. No",
                ReadOnly = true
            });

            // Student Name — read-only
            dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "StudentName",
                HeaderText = "Student Name",
                ReadOnly = true
            });

            // Status — ComboBox column
            DataGridViewComboBoxColumn colStatus = new DataGridViewComboBoxColumn
            {
                Name = "Status",
                HeaderText = "Status"
            };
            colStatus.Items.AddRange("Present", "Absent", "Late");
            dgvAttendance.Columns.Add(colStatus);

            // Time In — editable text box
            dgvAttendance.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TimeIn",
                HeaderText = "Time In"
            });
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
                    dgvAttendance.Rows.Clear();
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

                    dgvAttendance.Rows.Add(
                        rec.RecordId,
                        rec.StudentId,
                        rec.RegistrationNo,
                        rec.StudentName,
                        rec.Status,
                        timeInText);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to load attendance records:\n\n" + ex.Message);
            }
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
                    ScheduleId = null,
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

                ShowSuccess("Session created successfully with " + enrolled.Count + " student(s) marked Absent.");

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

        // ── Delete Session Button ────────────────────────────────
        private void btnDeleteSession_Click(object sender, EventArgs e)
        {
            AttendanceSession selected = cmbSession.SelectedItem as AttendanceSession;
            if (selected == null)
            {
                ShowWarning("Please select a session to delete.");
                return;
            }

            if (!ConfirmDelete("session #" + selected.SessionId +
                               " (" + selected.SessionDate.ToShortDateString() + ")"))
                return;

            try
            {
                _attendanceService.DeleteSession(selected.SessionId);
                ShowSuccess("Session deleted successfully.");
                LoadSessionsComboBox();
                dgvAttendance.Rows.Clear();
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

                ShowSuccess("Attendance saved for " + savedCount + " student(s).");
            }
            catch (Exception ex)
            {
                ShowError("Failed to save attendance:\n\n" + ex.Message);
            }
        }
    }
}
