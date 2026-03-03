namespace Attendance_Monitoring_System.Forms
{
    partial class ScheduleForm
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
            this.pnlInput = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtRoom = new System.Windows.Forms.TextBox();
            this.cmbEndTime = new System.Windows.Forms.ComboBox();
            this.cmbStartTime = new System.Windows.Forms.ComboBox();
            this.cmbDay = new System.Windows.Forms.ComboBox();
            this.lblRoom = new System.Windows.Forms.Label();
            this.lblEndTime = new System.Windows.Forms.Label();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.lblDay = new System.Windows.Forms.Label();
            this.dgvSchedules = new System.Windows.Forms.DataGridView();
            this.pnlInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedules)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblFormTitle.Location = new System.Drawing.Point(20, 15);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(252, 30);
            this.lblFormTitle.TabIndex = 0;
            this.lblFormTitle.Text = "Schedule Management";
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
            // pnlInput
            // 
            this.pnlInput.BackColor = System.Drawing.Color.White;
            this.pnlInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInput.Controls.Add(this.btnClear);
            this.pnlInput.Controls.Add(this.btnDelete);
            this.pnlInput.Controls.Add(this.btnUpdate);
            this.pnlInput.Controls.Add(this.btnAdd);
            this.pnlInput.Controls.Add(this.txtRoom);
            this.pnlInput.Controls.Add(this.cmbEndTime);
            this.pnlInput.Controls.Add(this.cmbStartTime);
            this.pnlInput.Controls.Add(this.cmbDay);
            this.pnlInput.Controls.Add(this.lblRoom);
            this.pnlInput.Controls.Add(this.lblEndTime);
            this.pnlInput.Controls.Add(this.lblStartTime);
            this.pnlInput.Controls.Add(this.lblDay);
            this.pnlInput.Location = new System.Drawing.Point(20, 90);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Size = new System.Drawing.Size(800, 195);
            this.pnlInput.TabIndex = 3;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(470, 140);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 40);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(360, 140);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 40);
            this.btnDelete.TabIndex = 10;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(250, 140);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(100, 40);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(140, 140);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 40);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // txtRoom
            // 
            this.txtRoom.Location = new System.Drawing.Point(500, 15);
            this.txtRoom.MaxLength = 30;
            this.txtRoom.Name = "txtRoom";
            this.txtRoom.Size = new System.Drawing.Size(200, 25);
            this.txtRoom.TabIndex = 7;
            // 
            // cmbEndTime
            // 
            this.cmbEndTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEndTime.FormattingEnabled = true;
            this.cmbEndTime.Location = new System.Drawing.Point(500, 50);
            this.cmbEndTime.Name = "cmbEndTime";
            this.cmbEndTime.Size = new System.Drawing.Size(200, 25);
            this.cmbEndTime.TabIndex = 6;
            // 
            // cmbStartTime
            // 
            this.cmbStartTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartTime.FormattingEnabled = true;
            this.cmbStartTime.Location = new System.Drawing.Point(140, 50);
            this.cmbStartTime.Name = "cmbStartTime";
            this.cmbStartTime.Size = new System.Drawing.Size(200, 25);
            this.cmbStartTime.TabIndex = 5;
            // 
            // cmbDay
            // 
            this.cmbDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDay.FormattingEnabled = true;
            this.cmbDay.Location = new System.Drawing.Point(140, 15);
            this.cmbDay.Name = "cmbDay";
            this.cmbDay.Size = new System.Drawing.Size(200, 25);
            this.cmbDay.TabIndex = 4;
            // 
            // lblRoom
            // 
            this.lblRoom.AutoSize = true;
            this.lblRoom.Location = new System.Drawing.Point(400, 18);
            this.lblRoom.Name = "lblRoom";
            this.lblRoom.Size = new System.Drawing.Size(43, 19);
            this.lblRoom.TabIndex = 3;
            this.lblRoom.Text = "Room";
            // 
            // lblEndTime
            // 
            this.lblEndTime.AutoSize = true;
            this.lblEndTime.Location = new System.Drawing.Point(400, 53);
            this.lblEndTime.Name = "lblEndTime";
            this.lblEndTime.Size = new System.Drawing.Size(64, 19);
            this.lblEndTime.TabIndex = 2;
            this.lblEndTime.Text = "End Time";
            // 
            // lblStartTime
            // 
            this.lblStartTime.AutoSize = true;
            this.lblStartTime.Location = new System.Drawing.Point(20, 53);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(69, 19);
            this.lblStartTime.TabIndex = 1;
            this.lblStartTime.Text = "Start Time";
            // 
            // lblDay
            // 
            this.lblDay.AutoSize = true;
            this.lblDay.Location = new System.Drawing.Point(20, 18);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(83, 19);
            this.lblDay.TabIndex = 0;
            this.lblDay.Text = "Day of Week";
            // 
            // dgvSchedules
            // 
            this.dgvSchedules.AllowUserToAddRows = false;
            this.dgvSchedules.AllowUserToDeleteRows = false;
            this.dgvSchedules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSchedules.Location = new System.Drawing.Point(20, 295);
            this.dgvSchedules.MultiSelect = false;
            this.dgvSchedules.Name = "dgvSchedules";
            this.dgvSchedules.ReadOnly = true;
            this.dgvSchedules.Size = new System.Drawing.Size(800, 260);
            this.dgvSchedules.TabIndex = 4;
            // 
            // ScheduleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 600);
            this.Controls.Add(this.dgvSchedules);
            this.Controls.Add(this.pnlInput);
            this.Controls.Add(this.cmbClass);
            this.Controls.Add(this.lblSelectClass);
            this.Controls.Add(this.lblFormTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ScheduleForm";
            this.Text = "Schedule";
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSchedules)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Label lblSelectClass;
        private System.Windows.Forms.ComboBox cmbClass;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.Label lblEndTime;
        private System.Windows.Forms.Label lblRoom;
        private System.Windows.Forms.ComboBox cmbDay;
        private System.Windows.Forms.ComboBox cmbStartTime;
        private System.Windows.Forms.ComboBox cmbEndTime;
        private System.Windows.Forms.TextBox txtRoom;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dgvSchedules;
    }
}
