using System;
using System.Drawing;
using System.Windows.Forms;

namespace Attendance_Monitoring_System.Forms
{
    public partial class BaseForm : Form
    {
        public BaseForm()
        {
            this.Font = new Font(SystemFonts.MessageBoxFont.FontFamily, 10f, FontStyle.Regular);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ShowIcon = false;
            this.KeyPreview = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ApplyButtonStyles(this.Controls);
            ApplyDataGridStyles(this.Controls);
        }

        // ── Reusable Message Helpers ─────────────────────────────

        protected void ShowSuccess(string message)
        {
            MessageBox.Show(message, "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected void ShowError(string message)
        {
            MessageBox.Show(message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected void ShowWarning(string message)
        {
            MessageBox.Show(message, "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        protected bool ConfirmDelete(string itemName = "this record")
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete " + itemName + "?\nThis action cannot be undone.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            return result == DialogResult.Yes;
        }

        // ── Style Helpers ────────────────────────────────────────

        private void ApplyButtonStyles(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.BackColor = Color.FromArgb(41, 128, 185);
                    btn.ForeColor = Color.White;
                    btn.Cursor = Cursors.Hand;
                    btn.Height = 40;
                }

                if (control.HasChildren)
                {
                    ApplyButtonStyles(control.Controls);
                }
            }
        }

        private void ApplyDataGridStyles(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is DataGridView grid)
                {
                    grid.BorderStyle = BorderStyle.None;
                    grid.BackgroundColor = Color.White;
                    grid.EnableHeadersVisualStyles = false;

                    grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                    grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185);
                    grid.ColumnHeadersHeight = 40;

                    grid.RowHeadersVisible = false;
                    grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                    grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
                    grid.DefaultCellStyle.SelectionForeColor = Color.White;
                    grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                    grid.RowTemplate.Height = 35;
                }

                if (control.HasChildren)
                {
                    ApplyDataGridStyles(control.Controls);
                }
            }
        }
    }
}