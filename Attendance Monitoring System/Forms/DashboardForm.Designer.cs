namespace Attendance_Monitoring_System.Forms
{
    partial class DashboardForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DashboardForm));
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.btnNavLogout = new System.Windows.Forms.Button();
            this.btnNavUsers = new System.Windows.Forms.Button();
            this.btnNavAttendance = new System.Windows.Forms.Button();
            this.btnNavRemarks = new System.Windows.Forms.Button();
            this.btnNavEnrollment = new System.Windows.Forms.Button();
            this.btnNavClasses = new System.Windows.Forms.Button();
            this.btnNavSchedule = new System.Windows.Forms.Button();
            this.btnNavSubjects = new System.Windows.Forms.Button();
            this.btnNavTeachers = new System.Windows.Forms.Button();
            this.btnNavStudents = new System.Windows.Forms.Button();
            this.btnNavDashboard = new System.Windows.Forms.Button();
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.lblAppTitle = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.imageListSidebar = new System.Windows.Forms.ImageList(this.components);
            this.pnlSidebar.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.pnlSidebar.Controls.Add(this.btnNavLogout);
            this.pnlSidebar.Controls.Add(this.btnNavUsers);
            this.pnlSidebar.Controls.Add(this.btnNavAttendance);
            this.pnlSidebar.Controls.Add(this.btnNavRemarks);
            this.pnlSidebar.Controls.Add(this.btnNavEnrollment);
            this.pnlSidebar.Controls.Add(this.btnNavClasses);
            this.pnlSidebar.Controls.Add(this.btnNavSchedule);
            this.pnlSidebar.Controls.Add(this.btnNavSubjects);
            this.pnlSidebar.Controls.Add(this.btnNavTeachers);
            this.pnlSidebar.Controls.Add(this.btnNavStudents);
            this.pnlSidebar.Controls.Add(this.btnNavDashboard);
            this.pnlSidebar.Controls.Add(this.lblUserInfo);
            this.pnlSidebar.Controls.Add(this.lblAppTitle);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSidebar.Location = new System.Drawing.Point(0, 0);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(220, 605);
            this.pnlSidebar.TabIndex = 0;
            // 
            // btnNavLogout
            // 
            this.btnNavLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(57)))), ((int)(((byte)(43)))));
            this.btnNavLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavLogout.FlatAppearance.BorderSize = 0;
            this.btnNavLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavLogout.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavLogout.ForeColor = System.Drawing.Color.White;
            this.btnNavLogout.Location = new System.Drawing.Point(0, 560);
            this.btnNavLogout.Name = "btnNavLogout";
            this.btnNavLogout.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnNavLogout.Size = new System.Drawing.Size(220, 45);
            this.btnNavLogout.TabIndex = 12;
            this.btnNavLogout.Text = "Log Out";
            this.btnNavLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavLogout.UseVisualStyleBackColor = false;
            // 
            // btnNavUsers
            // 
            this.btnNavUsers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavUsers.FlatAppearance.BorderSize = 0;
            this.btnNavUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavUsers.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavUsers.ForeColor = System.Drawing.Color.White;
            this.btnNavUsers.Location = new System.Drawing.Point(0, 500);
            this.btnNavUsers.Name = "btnNavUsers";
            this.btnNavUsers.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnNavUsers.Size = new System.Drawing.Size(220, 45);
            this.btnNavUsers.TabIndex = 11;
            this.btnNavUsers.Text = "User Management";
            this.btnNavUsers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavUsers.UseVisualStyleBackColor = true;
            // 
            // btnNavAttendance
            // 
            this.btnNavAttendance.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavAttendance.FlatAppearance.BorderSize = 0;
            this.btnNavAttendance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavAttendance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavAttendance.ForeColor = System.Drawing.Color.White;
            this.btnNavAttendance.Location = new System.Drawing.Point(0, 410);
            this.btnNavAttendance.Name = "btnNavAttendance";
            this.btnNavAttendance.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnNavAttendance.Size = new System.Drawing.Size(220, 45);
            this.btnNavAttendance.TabIndex = 10;
            this.btnNavAttendance.Text = "Attendance";
            this.btnNavAttendance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavAttendance.UseVisualStyleBackColor = true;
            // 
            // btnNavRemarks
            // 
            this.btnNavRemarks.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavRemarks.FlatAppearance.BorderSize = 0;
            this.btnNavRemarks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavRemarks.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavRemarks.ForeColor = System.Drawing.Color.White;
            this.btnNavRemarks.Location = new System.Drawing.Point(0, 455);
            this.btnNavRemarks.Name = "btnNavRemarks";
            this.btnNavRemarks.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnNavRemarks.Size = new System.Drawing.Size(220, 45);
            this.btnNavRemarks.TabIndex = 9;
            this.btnNavRemarks.Text = "Remarks";
            this.btnNavRemarks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavRemarks.UseVisualStyleBackColor = true;
            // 
            // btnNavEnrollment
            // 
            this.btnNavEnrollment.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavEnrollment.FlatAppearance.BorderSize = 0;
            this.btnNavEnrollment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavEnrollment.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavEnrollment.ForeColor = System.Drawing.Color.White;
            this.btnNavEnrollment.Location = new System.Drawing.Point(0, 365);
            this.btnNavEnrollment.Name = "btnNavEnrollment";
            this.btnNavEnrollment.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnNavEnrollment.Size = new System.Drawing.Size(220, 45);
            this.btnNavEnrollment.TabIndex = 8;
            this.btnNavEnrollment.Text = "Enrollment";
            this.btnNavEnrollment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavEnrollment.UseVisualStyleBackColor = true;
            // 
            // btnNavClasses
            // 
            this.btnNavClasses.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavClasses.FlatAppearance.BorderSize = 0;
            this.btnNavClasses.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavClasses.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavClasses.ForeColor = System.Drawing.Color.White;
            this.btnNavClasses.Location = new System.Drawing.Point(0, 275);
            this.btnNavClasses.Name = "btnNavClasses";
            this.btnNavClasses.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnNavClasses.Size = new System.Drawing.Size(220, 45);
            this.btnNavClasses.TabIndex = 7;
            this.btnNavClasses.Text = "Classes";
            this.btnNavClasses.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavClasses.UseVisualStyleBackColor = true;
            // 
            // btnNavSchedule
            // 
            this.btnNavSchedule.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavSchedule.FlatAppearance.BorderSize = 0;
            this.btnNavSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavSchedule.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavSchedule.ForeColor = System.Drawing.Color.White;
            this.btnNavSchedule.Location = new System.Drawing.Point(0, 320);
            this.btnNavSchedule.Name = "btnNavSchedule";
            this.btnNavSchedule.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnNavSchedule.Size = new System.Drawing.Size(220, 45);
            this.btnNavSchedule.TabIndex = 6;
            this.btnNavSchedule.Text = "Schedule";
            this.btnNavSchedule.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavSchedule.UseVisualStyleBackColor = true;
            // 
            // btnNavSubjects
            // 
            this.btnNavSubjects.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavSubjects.FlatAppearance.BorderSize = 0;
            this.btnNavSubjects.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavSubjects.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavSubjects.ForeColor = System.Drawing.Color.White;
            this.btnNavSubjects.Location = new System.Drawing.Point(0, 230);
            this.btnNavSubjects.Name = "btnNavSubjects";
            this.btnNavSubjects.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnNavSubjects.Size = new System.Drawing.Size(220, 45);
            this.btnNavSubjects.TabIndex = 5;
            this.btnNavSubjects.Text = "Subjects";
            this.btnNavSubjects.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavSubjects.UseVisualStyleBackColor = true;
            // 
            // btnNavTeachers
            // 
            this.btnNavTeachers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavTeachers.FlatAppearance.BorderSize = 0;
            this.btnNavTeachers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavTeachers.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavTeachers.ForeColor = System.Drawing.Color.White;
            this.btnNavTeachers.Location = new System.Drawing.Point(0, 185);
            this.btnNavTeachers.Name = "btnNavTeachers";
            this.btnNavTeachers.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnNavTeachers.Size = new System.Drawing.Size(220, 45);
            this.btnNavTeachers.TabIndex = 4;
            this.btnNavTeachers.Text = "Teachers";
            this.btnNavTeachers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavTeachers.UseVisualStyleBackColor = true;
            // 
            // btnNavStudents
            // 
            this.btnNavStudents.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavStudents.FlatAppearance.BorderSize = 0;
            this.btnNavStudents.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavStudents.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavStudents.ForeColor = System.Drawing.Color.White;
            this.btnNavStudents.Location = new System.Drawing.Point(0, 140);
            this.btnNavStudents.Name = "btnNavStudents";
            this.btnNavStudents.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnNavStudents.Size = new System.Drawing.Size(220, 45);
            this.btnNavStudents.TabIndex = 3;
            this.btnNavStudents.Text = "Students";
            this.btnNavStudents.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavStudents.UseVisualStyleBackColor = true;
            // 
            // btnNavDashboard
            // 
            this.btnNavDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavDashboard.FlatAppearance.BorderSize = 0;
            this.btnNavDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavDashboard.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavDashboard.ForeColor = System.Drawing.Color.White;
            this.btnNavDashboard.Location = new System.Drawing.Point(0, 95);
            this.btnNavDashboard.Name = "btnNavDashboard";
            this.btnNavDashboard.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnNavDashboard.Size = new System.Drawing.Size(220, 45);
            this.btnNavDashboard.TabIndex = 2;
            this.btnNavDashboard.Text = "Dashboard";
            this.btnNavDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavDashboard.UseVisualStyleBackColor = true;
            // 
            // lblUserInfo
            // 
            this.lblUserInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserInfo.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.lblUserInfo.Location = new System.Drawing.Point(0, 55);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(220, 25);
            this.lblUserInfo.TabIndex = 1;
            this.lblUserInfo.Text = "Welcome, Admin";
            this.lblUserInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAppTitle
            // 
            this.lblAppTitle.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppTitle.ForeColor = System.Drawing.Color.White;
            this.lblAppTitle.Location = new System.Drawing.Point(0, 10);
            this.lblAppTitle.Name = "lblAppTitle";
            this.lblAppTitle.Size = new System.Drawing.Size(220, 50);
            this.lblAppTitle.TabIndex = 0;
            this.lblAppTitle.Text = "AMS";
            this.lblAppTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(220, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.pnlContent.Size = new System.Drawing.Size(914, 605);
            this.pnlContent.TabIndex = 2;
            // 
            // imageListSidebar
            // 
            this.imageListSidebar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSidebar.ImageStream")));
            this.imageListSidebar.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSidebar.Images.SetKeyName(0, "dashboard");
            this.imageListSidebar.Images.SetKeyName(1, "students");
            this.imageListSidebar.Images.SetKeyName(2, "teachers");
            this.imageListSidebar.Images.SetKeyName(3, "subjects");
            this.imageListSidebar.Images.SetKeyName(4, "classes");
            this.imageListSidebar.Images.SetKeyName(5, "schedule");
            this.imageListSidebar.Images.SetKeyName(6, "enrollment");
            this.imageListSidebar.Images.SetKeyName(7, "attendance");
            this.imageListSidebar.Images.SetKeyName(8, "remarks");
            this.imageListSidebar.Images.SetKeyName(9, "users");
            this.imageListSidebar.Images.SetKeyName(10, "logout");
            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 605);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlSidebar);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DashboardForm";
            this.Text = "Attendance Monitoring System";
            this.pnlSidebar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Label lblAppTitle;
        private System.Windows.Forms.Label lblUserInfo;
        private System.Windows.Forms.Button btnNavLogout;
        private System.Windows.Forms.Button btnNavUsers;
        private System.Windows.Forms.Button btnNavAttendance;
        private System.Windows.Forms.Button btnNavRemarks;
        private System.Windows.Forms.Button btnNavEnrollment;
        private System.Windows.Forms.Button btnNavClasses;
        private System.Windows.Forms.Button btnNavSchedule;
        private System.Windows.Forms.Button btnNavSubjects;
        private System.Windows.Forms.Button btnNavTeachers;
        private System.Windows.Forms.Button btnNavStudents;
        private System.Windows.Forms.Button btnNavDashboard;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.ImageList imageListSidebar;
    }
}