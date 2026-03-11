DROP SCHEMA IF EXISTS attendance_db;
CREATE SCHEMA attendance_db;
USE attendance_db;

CREATE TABLE students (
  student_id INT AUTO_INCREMENT PRIMARY KEY,
  registration_no VARCHAR(20) NOT NULL UNIQUE,
  first_name VARCHAR(50) NOT NULL,
  last_name  VARCHAR(50) NOT NULL,
  gender ENUM('Male','Female','Other') NOT NULL,
  date_of_birth DATE NOT NULL
);

CREATE TABLE teachers (
  teacher_id INT AUTO_INCREMENT PRIMARY KEY,
  first_name VARCHAR(50) NOT NULL,
  last_name  VARCHAR(50) NOT NULL,
  email VARCHAR(100) UNIQUE,
  designation VARCHAR(50)
);

CREATE TABLE subjects (
  subject_id INT AUTO_INCREMENT PRIMARY KEY,
  subject_code VARCHAR(15) NOT NULL UNIQUE,
  subject_name VARCHAR(100) NOT NULL
);

CREATE TABLE classes (
  class_id INT AUTO_INCREMENT PRIMARY KEY,
  subject_id INT NOT NULL,
  teacher_id INT NOT NULL,
  academic_year VARCHAR(9) NOT NULL,
  semester INT NOT NULL,
  section VARCHAR(10) NOT NULL,
  FOREIGN KEY (subject_id) REFERENCES subjects(subject_id),
  FOREIGN KEY (teacher_id) REFERENCES teachers(teacher_id),
  UNIQUE (subject_id, teacher_id, academic_year, semester, section)
);

CREATE TABLE class_schedule (
  schedule_id INT AUTO_INCREMENT PRIMARY KEY,
  class_id INT NOT NULL,
  day_of_week INT NOT NULL,
  start_time VARCHAR(20) NOT NULL,
  end_time   VARCHAR(20) NOT NULL,
  room VARCHAR(30),
  FOREIGN KEY (class_id) REFERENCES classes(class_id),
  UNIQUE (class_id, day_of_week, start_time, end_time)
);

CREATE TABLE enrollments (
  enrollment_id INT AUTO_INCREMENT PRIMARY KEY,
  class_id INT NOT NULL,
  student_id INT NOT NULL,
  enrolled_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (class_id) REFERENCES classes(class_id),
  FOREIGN KEY (student_id) REFERENCES students(student_id),
  UNIQUE (class_id, student_id)
);

CREATE TABLE attendance_sessions (
  session_id INT AUTO_INCREMENT PRIMARY KEY,
  class_id INT NOT NULL,
  session_name VARCHAR(100) NULL,
  session_date DATE NOT NULL,
  created_by_teacher_id INT NOT NULL,
  FOREIGN KEY (class_id) REFERENCES classes(class_id),
  FOREIGN KEY (created_by_teacher_id) REFERENCES teachers(teacher_id),
  UNIQUE (class_id, session_date)
);

CREATE TABLE attendance_status (
  status_id INT PRIMARY KEY,
  status_name VARCHAR(20) NOT NULL UNIQUE
);

INSERT INTO attendance_status (status_id, status_name) VALUES
(1,'Present'),
(2,'Absent'),
(3,'Late'),
(4,'Excused');

CREATE TABLE attendance_records (
  attendance_record_id INT AUTO_INCREMENT PRIMARY KEY,
  session_id INT NOT NULL,
  student_id INT NOT NULL,
  status_id INT NOT NULL,
  time_in DATETIME NULL, -- newly added column baked into the schema
  marked_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (session_id) REFERENCES attendance_sessions(session_id),
  FOREIGN KEY (student_id) REFERENCES students(student_id),
  FOREIGN KEY (status_id) REFERENCES attendance_status(status_id),
  UNIQUE (session_id, student_id)
);

CREATE TABLE remark_categories (
  category_id INT PRIMARY KEY,
  category_name VARCHAR(30) NOT NULL UNIQUE
);

INSERT INTO remark_categories (category_id, category_name) VALUES
(1,'Late reason'),
(2,'Excuse note'),
(3,'Behavior'),
(4,'Other');

CREATE TABLE remarks (
  remark_id INT AUTO_INCREMENT PRIMARY KEY,
  session_id INT NOT NULL,
  student_id INT NOT NULL,
  teacher_id INT NOT NULL,
  category_id INT NULL,
  remark_text VARCHAR(255) NOT NULL,
  remark_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (session_id, student_id)
    REFERENCES attendance_records(session_id, student_id),
  FOREIGN KEY (teacher_id) REFERENCES teachers(teacher_id),
  FOREIGN KEY (category_id) REFERENCES remark_categories(category_id)
);

CREATE TABLE users (
  user_id INT AUTO_INCREMENT PRIMARY KEY,
  username VARCHAR(50) NOT NULL UNIQUE,
  password_hash VARCHAR(255) NOT NULL,
  role ENUM('Admin','Registrar','Teacher','Student') NOT NULL,
  teacher_id INT NULL,
  student_id INT NULL,
  is_active INT NOT NULL DEFAULT 1,
  created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  FOREIGN KEY (teacher_id) REFERENCES teachers(teacher_id),
  CONSTRAINT fk_users_student FOREIGN KEY (student_id) REFERENCES students(student_id)
);