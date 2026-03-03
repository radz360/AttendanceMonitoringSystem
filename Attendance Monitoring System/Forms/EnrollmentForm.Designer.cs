namespace Attendance_Monitoring_System.Forms
{
    partial class EnrollmentForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.lblSelectClass = new System.Windows.Forms.Label();
            this.cmbClass = new System.Windows.Forms.ComboBox();
            this.lblAvailable = new System.Windows.Forms.Label();
            this.lblEnrolled = new System.Windows.Forms.Label();
            this.dgvAvailable = new System.Windows.Forms.DataGridView();
            this.dgvEnrolled = new System.Windows.Forms.DataGridView();
            this.btnEnroll = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnrolled)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblFormTitle.Location = new System.Drawing.Point(20, 15);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(268, 30);
            this.lblFormTitle.TabIndex = 0;
            this.lblFormTitle.Text = "Enrollment Management";
            // 
            // lblSelectClass
            // 
            this.lblSelectClass.AutoSize = true;
            this.lblSelectClass.Location = new System.Drawing.Point(20, 58);
            this.lblSelectClass.Name = "lblSelectClass";
            this.lblSelectClass.Size = new System.Drawing.Size(82, 19);
            this.lblSelectClass.TabIndex = 1;
            this.lblSelectClass.Text = "Select Class:";
            // 
            // cmbClass
            // 
            this.cmbClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClass.FormattingEnabled = true;
            this.cmbClass.Location = new System.Drawing.Point(120, 55);
            this.cmbClass.Name = "cmbClass";
            this.cmbClass.Size = new System.Drawing.Size(500, 25);
            this.cmbClass.TabIndex = 2;
            // 
            // lblAvailable
            // 
            this.lblAvailable.AutoSize = true;
            this.lblAvailable.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblAvailable.Location = new System.Drawing.Point(20, 95);
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.Size = new System.Drawing.Size(134, 19);
            this.lblAvailable.TabIndex = 3;
            this.lblAvailable.Text = "Available Students";
            // 
            // lblEnrolled
            // 
            this.lblEnrolled.AutoSize = true;
            this.lblEnrolled.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblEnrolled.Location = new System.Drawing.Point(490, 95);
            this.lblEnrolled.Name = "lblEnrolled";
            this.lblEnrolled.Size = new System.Drawing.Size(127, 19);
            this.lblEnrolled.TabIndex = 4;
            this.lblEnrolled.Text = "Enrolled Students";
            // 
            // dgvAvailable
            // 
            this.dgvAvailable.AllowUserToAddRows = false;
            this.dgvAvailable.AllowUserToDeleteRows = false;
            this.dgvAvailable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAvailable.Location = new System.Drawing.Point(20, 120);
            this.dgvAvailable.MultiSelect = true;
            this.dgvAvailable.Name = "dgvAvailable";
            this.dgvAvailable.ReadOnly = true;
            this.dgvAvailable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAvailable.Size = new System.Drawing.Size(330, 430);
            this.dgvAvailable.TabIndex = 5;
            // 
            // dgvEnrolled
            // 
            this.dgvEnrolled.AllowUserToAddRows = false;
            this.dgvEnrolled.AllowUserToDeleteRows = false;
            this.dgvEnrolled.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEnrolled.Location = new System.Drawing.Point(490, 120);
            this.dgvEnrolled.MultiSelect = true;
            this.dgvEnrolled.Name = "dgvEnrolled";
            this.dgvEnrolled.ReadOnly = true;
            this.dgvEnrolled.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEnrolled.Size = new System.Drawing.Size(330, 430);
            this.dgvEnrolled.TabIndex = 6;
            // 
            // btnEnroll
            // 
            this.btnEnroll.Location = new System.Drawing.Point(370, 250);
            this.btnEnroll.Name = "btnEnroll";
            this.btnEnroll.Size = new System.Drawing.Size(100, 40);
            this.btnEnroll.TabIndex = 7;
            this.btnEnroll.Text = "Enroll >>";
            this.btnEnroll.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(370, 310);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(100, 40);
            this.btnRemove.TabIndex = 8;
            this.btnRemove.Text = "<< Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            // 
            // EnrollmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 600);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnEnroll);
            this.Controls.Add(this.dgvEnrolled);
            this.Controls.Add(this.dgvAvailable);
            this.Controls.Add(this.lblEnrolled);
            this.Controls.Add(this.lblAvailable);
            this.Controls.Add(this.cmbClass);
            this.Controls.Add(this.lblSelectClass);
            this.Controls.Add(this.lblFormTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "EnrollmentForm";
            this.Text = "Enrollment";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnrolled)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Label lblSelectClass;
        private System.Windows.Forms.ComboBox cmbClass;
        private System.Windows.Forms.Label lblAvailable;
        private System.Windows.Forms.Label lblEnrolled;
        private System.Windows.Forms.DataGridView dgvAvailable;
        private System.Windows.Forms.DataGridView dgvEnrolled;
        private System.Windows.Forms.Button btnEnroll;
        private System.Windows.Forms.Button btnRemove;
    }
}
