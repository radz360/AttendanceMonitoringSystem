using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Attendance_Monitoring_System.Models;
using Attendance_Monitoring_System.Services;

namespace Attendance_Monitoring_System.Forms
{
    public partial class LoginForm : Form
    {
        private readonly AuthService _authService = new AuthService();

        public LoginForm()
        {
            InitializeComponent();

            // --- Event Handlers ---
            btnLogin.Click += new EventHandler(btnLogin_Click);
            chkShowPassword.CheckedChanged += new EventHandler(chkShowPassword_CheckedChanged);
            txtPassword.KeyDown += new KeyEventHandler(txtPassword_KeyDown);
            txtUsername.KeyDown += new KeyEventHandler(txtUsername_KeyDown);
        }

        // ── Login Button Click ──────────────────────────────────
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // --- Presence Validation ---
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter your username.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter your password.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // --- Length Validation ---
            if (txtPassword.Text.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            try
            {
                // --- Attempt Login ---
                User user = _authService.Login(txtUsername.Text.Trim(), txtPassword.Text);

                if (user != null)
                {
                    // Hide login form, open dashboard
                    this.Hide();

                    DashboardForm dashboard = new DashboardForm(user);
                    dashboard.FormClosed += (s, args) => this.Close();
                    dashboard.Show();
                }
                else
                {
                    MessageBox.Show("Invalid username or password, or account is deactivated.",
                        "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred while connecting to the database:\n\n" + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Configuration error:\n\n" + ex.Message,
                    "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Show/Hide Password ──────────────────────────────────
        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        // ── Allow Enter Key to Submit ───────────────────────────
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
                e.SuppressKeyPress = true; // Prevents the "ding" sound
            }
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPassword.Focus();
                e.SuppressKeyPress = true;
            }
        }
    }
}