using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Attendance_Monitoring_System.Models;
using Attendance_Monitoring_System.Services;

namespace Attendance_Monitoring_System.Forms
{
    public partial class UserManagementForm : BaseForm
    {
        private readonly User _currentUser;
        private readonly UserService _userService = new UserService();
        private readonly AuthService _authService = new AuthService();

        private int _selectedUserId = -1;
        private List<User> _allUsers = new List<User>();

        public UserManagementForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            // --- Event Handlers ---
            cmbRole.SelectedIndexChanged += cmbRole_SelectedIndexChanged;
            btnCreate.Click += btnCreate_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnResetPassword.Click += btnResetPassword_Click;
            btnClear.Click += btnClear_Click;
            txtSearch.TextChanged += txtSearch_TextChanged;
            dgvUsers.CellClick += dgvUsers_CellClick;
        }

        // ── Form Load ───────────────────────────────────────────
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Style reset password button orange
            btnResetPassword.BackColor = Color.FromArgb(211, 84, 0);

            // Set default role selection
            if (cmbRole.Items.Count > 0)
                cmbRole.SelectedIndex = 0;

            // Load grid
            LoadUsers();
        }

        // ── Role Changed (no-op now, kept for event handler) ─────
        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            // No linked combos to toggle anymore
        }

        // ── Load Users into Grid ─────────────────────────────────
        private void LoadUsers()
        {
            try
            {
                _allUsers = _userService.GetAllUsers();
                PopulateGrid(_allUsers);
            }
            catch (Exception ex)
            {
                ShowError("Failed to load users:\n\n" + ex.Message);
            }
        }

        // ── Populate DataGridView ────────────────────────────────
        private void PopulateGrid(List<User> users)
        {
            dgvUsers.DataSource = null;
            dgvUsers.DataSource = users;

            if (dgvUsers.Columns.Count > 0)
            {
                dgvUsers.Columns["UserId"].Visible = false;
                dgvUsers.Columns["PasswordHash"].Visible = false;
                dgvUsers.Columns["TeacherId"].Visible = false;
                dgvUsers.Columns["StudentId"].Visible = false;

                dgvUsers.Columns["Username"].HeaderText = "Username";
                dgvUsers.Columns["Role"].HeaderText = "Role";
                dgvUsers.Columns["IsActive"].HeaderText = "Active";
                dgvUsers.Columns["CreatedAt"].HeaderText = "Created";
                dgvUsers.Columns["CreatedAt"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            }
        }

        // ── Grid Row Click — Populate Fields ─────────────────────
        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvUsers.Rows[e.RowIndex];

            _selectedUserId = Convert.ToInt32(row.Cells["UserId"].Value);

            txtUsername.Text = row.Cells["Username"].Value.ToString();
            txtPassword.Clear(); // Never show existing password

            // Set role
            string role = row.Cells["Role"].Value.ToString();
            for (int i = 0; i < cmbRole.Items.Count; i++)
            {
                if (string.Equals(cmbRole.Items[i].ToString(), role, StringComparison.OrdinalIgnoreCase))
                {
                    cmbRole.SelectedIndex = i;
                    break;
                }
            }

            // Set active checkbox
            chkActive.Checked = Convert.ToBoolean(row.Cells["IsActive"].Value);
        }

        // ── Create User ──────────────────────────────────────────
        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!ValidateCreateInput()) return;

            try
            {
                string role = cmbRole.SelectedItem.ToString();

                _authService.CreateUser(
                    txtUsername.Text.Trim(),
                    txtPassword.Text,
                    role,
                    null,
                    null
                );

                ShowSuccess("User created successfully.");
                ClearFields();
                LoadUsers();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Number == 1062) // Duplicate key
                {
                    ShowWarning("A user with this username already exists.");
                }
                else
                {
                    ShowError("Database error:\n\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to create user:\n\n" + ex.Message);
            }
        }

        // ── Update User ──────────────────────────────────────────
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_selectedUserId < 0)
            {
                ShowWarning("Please select a user from the list to update.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowWarning("Username cannot be empty.");
                txtUsername.Focus();
                return;
            }

            try
            {
                string role = cmbRole.SelectedItem.ToString();

                User user = new User
                {
                    UserId = _selectedUserId,
                    Username = txtUsername.Text.Trim(),
                    Role = role,
                    IsActive = chkActive.Checked
                };

                _userService.UpdateUser(user);
                ShowSuccess("User updated successfully.");
                ClearFields();
                LoadUsers();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    ShowWarning("A user with this username already exists.");
                }
                else
                {
                    ShowError("Database error:\n\n" + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to update user:\n\n" + ex.Message);
            }
        }

        // ── Reset Password ───────────────────────────────────────
        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (_selectedUserId < 0)
            {
                ShowWarning("Please select a user from the list first.");
                return;
            }

            // Prompt for new password using a simple custom dialog
            string newPassword = ShowPasswordInputDialog();

            if (newPassword == null)
                return; // User cancelled

            if (newPassword.Length < 8)
            {
                ShowWarning("Password must be at least 8 characters.");
                return;
            }

            try
            {
                _userService.ResetPassword(_selectedUserId, newPassword);
                ShowSuccess("Password reset successfully.");
            }
            catch (Exception ex)
            {
                ShowError("Failed to reset password:\n\n" + ex.Message);
            }
        }

        // ── Custom Password Input Dialog ─────────────────────────
        private string ShowPasswordInputDialog()
        {
            using (Form dialog = new Form())
            {
                dialog.Text = "Reset Password";
                dialog.Size = new Size(380, 170);
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                Label lbl = new Label
                {
                    Text = "Enter new password (minimum 8 characters):",
                    Location = new Point(15, 15),
                    AutoSize = true
                };

                TextBox txt = new TextBox
                {
                    Location = new Point(15, 40),
                    Size = new Size(330, 25),
                    UseSystemPasswordChar = true,
                    MaxLength = 50
                };

                Button btnOk = new Button
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Location = new Point(180, 80),
                    Size = new Size(80, 30)
                };

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(265, 80),
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
                    string value = txt.Text.Trim();
                    if (string.IsNullOrWhiteSpace(value))
                        return null;
                    return value;
                }

                return null;
            }
        }

        // ── Search / Filter ──────────────────────────────────────
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                PopulateGrid(_allUsers);
                return;
            }

            List<User> filtered = _allUsers.FindAll(u =>
                u.Username.ToLower().Contains(keyword) ||
                u.Role.ToLower().Contains(keyword)
            );

            PopulateGrid(filtered);
        }

        // ── Clear Fields ─────────────────────────────────────────
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            _selectedUserId = -1;
            txtUsername.Clear();
            txtPassword.Clear();
            txtPassword.Enabled = true;

            if (cmbRole.Items.Count > 0)
                cmbRole.SelectedIndex = 0;

            chkActive.Checked = true;

            dgvUsers.ClearSelection();
            txtUsername.Focus();
        }

        // ── Input Validation (Create) ────────────────────────────
        private bool ValidateCreateInput()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowWarning("Please enter a username.");
                txtUsername.Focus();
                return false;
            }

            if (txtUsername.Text.Trim().Length < 3)
            {
                ShowWarning("Username must be at least 3 characters.");
                txtUsername.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowWarning("Please enter a password.");
                txtPassword.Focus();
                return false;
            }

            if (txtPassword.Text.Length < 8)
            {
                ShowWarning("Password must be at least 8 characters.");
                txtPassword.Focus();
                return false;
            }

            if (cmbRole.SelectedItem == null)
            {
                ShowWarning("Please select a role.");
                cmbRole.Focus();
                return false;
            }

            return true;
        }
    }
}
