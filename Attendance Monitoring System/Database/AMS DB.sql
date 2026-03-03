-- ─────────────────────────────────────────────────────────
-- DATABASE CREATION & SCHEMA
-- ─────────────────────────────────────────────────────────

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
  schedule_id INT NULL,
  session_date DATE NOT NULL,
  created_by_teacher_id INT NOT NULL,
  FOREIGN KEY (class_id) REFERENCES classes(class_id),
  FOREIGN KEY (schedule_id) REFERENCES class_schedule(schedule_id),
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
  updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  FOREIGN KEY (teacher_id) REFERENCES teachers(teacher_id),
  CONSTRAINT fk_users_student FOREIGN KEY (student_id) REFERENCES students(student_id)
);

-- ─────────────────────────────────────────────────────────
-- STORED PROCEDURES
-- ─────────────────────────────────────────────────────────

DELIMITER //

-- ── USERS ────────────────────────────────────────────────

DROP PROCEDURE IF EXISTS sp_GetUserByUsername //
CREATE PROCEDURE sp_GetUserByUsername(
    IN p_username VARCHAR(50)
)
BEGIN
    SELECT user_id, username, password_hash, role, teacher_id, student_id, is_active
    FROM users
    WHERE username = p_username;
END //

DROP PROCEDURE IF EXISTS sp_CreateUser //
CREATE PROCEDURE sp_CreateUser(
    IN p_username VARCHAR(50),
    IN p_password_hash VARCHAR(255),
    IN p_role ENUM('Admin','Registrar','Teacher','Student'),
    IN p_teacher_id INT,
    IN p_student_id INT
)
BEGIN
    INSERT INTO users (username, password_hash, role, teacher_id, student_id)
    VALUES (p_username, p_password_hash, p_role, p_teacher_id, p_student_id);
END //

DROP PROCEDURE IF EXISTS sp_GetAllUsers //
CREATE PROCEDURE sp_GetAllUsers()
BEGIN
    SELECT u.user_id, u.username, u.role, u.teacher_id, u.student_id,
           CONCAT(t.first_name, ' ', t.last_name) AS teacher_name,
           CONCAT(s.first_name, ' ', s.last_name) AS student_name,
           u.is_active
    FROM users u
    LEFT JOIN teachers t ON u.teacher_id = t.teacher_id
    LEFT JOIN students s ON u.student_id = s.student_id
    ORDER BY u.username;
END //

DROP PROCEDURE IF EXISTS sp_UpdateUser //
CREATE PROCEDURE sp_UpdateUser( 
    IN p_user_id INT, 
    IN p_username VARCHAR(50), 
    IN p_role ENUM('Admin','Registrar','Teacher','Student'), 
    IN p_teacher_id INT, 
    IN p_student_id INT, 
    IN p_is_active INT 
) 
BEGIN 
    UPDATE users 
    SET username = p_username, 
        role = p_role, 
        teacher_id = p_teacher_id, 
        student_id = p_student_id, 
        is_active = p_is_active 
    WHERE user_id = p_user_id; 
END //

DROP PROCEDURE IF EXISTS sp_ResetUserPassword //
CREATE PROCEDURE sp_ResetUserPassword(
    IN p_user_id INT,
    IN p_password_hash VARCHAR(255)
)
BEGIN
    UPDATE users
    SET password_hash = p_password_hash
    WHERE user_id = p_user_id;
END //

-- ── STUDENTS ─────────────────────────────────────────────

DROP PROCEDURE IF EXISTS sp_GetAllStudents //
CREATE PROCEDURE sp_GetAllStudents()
BEGIN
    SELECT student_id, registration_no, first_name, last_name,
           gender, date_of_birth
    FROM students
    ORDER BY last_name, first_name;
END //

DROP PROCEDURE IF EXISTS sp_GetStudentById //
CREATE PROCEDURE sp_GetStudentById(
    IN p_student_id INT
)
BEGIN
    SELECT student_id, registration_no, first_name, last_name,
           gender, date_of_birth
    FROM students
    WHERE student_id = p_student_id;
END //

DROP PROCEDURE IF EXISTS sp_AddStudent //
CREATE PROCEDURE sp_AddStudent(
    IN p_registration_no VARCHAR(20),
    IN p_first_name VARCHAR(50),
    IN p_last_name VARCHAR(50),
    IN p_gender ENUM('Male','Female','Other'),
    IN p_date_of_birth DATE
)
BEGIN
    INSERT INTO students (registration_no, first_name, last_name, gender, date_of_birth)
    VALUES (p_registration_no, p_first_name, p_last_name, p_gender, p_date_of_birth);
END //

DROP PROCEDURE IF EXISTS sp_UpdateStudent //
CREATE PROCEDURE sp_UpdateStudent(
    IN p_student_id INT,
    IN p_registration_no VARCHAR(20),
    IN p_first_name VARCHAR(50),
    IN p_last_name VARCHAR(50),
    IN p_gender ENUM('Male','Female','Other'),
    IN p_date_of_birth DATE
)
BEGIN
    UPDATE students
    SET registration_no = p_registration_no,
        first_name = p_first_name,
        last_name = p_last_name,
        gender = p_gender,
        date_of_birth = p_date_of_birth
    WHERE student_id = p_student_id;
END //

DROP PROCEDURE IF EXISTS sp_DeleteStudent //
CREATE PROCEDURE sp_DeleteStudent(
    IN p_student_id INT
)
BEGIN
    DELETE FROM students WHERE student_id = p_student_id;
END //

DROP PROCEDURE IF EXISTS sp_SearchStudents //
CREATE PROCEDURE sp_SearchStudents(
    IN p_keyword VARCHAR(100)
)
BEGIN
    SET @kw = CONCAT('%', p_keyword, '%');
    SELECT student_id, registration_no, first_name, last_name,
           gender, date_of_birth
    FROM students
    WHERE registration_no LIKE @kw
       OR first_name LIKE @kw
       OR last_name LIKE @kw
    ORDER BY last_name, first_name;
END //

-- ── TEACHERS ─────────────────────────────────────────────

DROP PROCEDURE IF EXISTS sp_GetAllTeachers //
CREATE PROCEDURE sp_GetAllTeachers()
BEGIN
    SELECT teacher_id, first_name, last_name, email, designation
    FROM teachers
    ORDER BY last_name, first_name;
END //

DROP PROCEDURE IF EXISTS sp_GetTeacherById //
CREATE PROCEDURE sp_GetTeacherById(
    IN p_teacher_id INT
)
BEGIN
    SELECT teacher_id, first_name, last_name, email, designation
    FROM teachers
    WHERE teacher_id = p_teacher_id;
END //

DROP PROCEDURE IF EXISTS sp_AddTeacher //
CREATE PROCEDURE sp_AddTeacher(
    IN p_first_name VARCHAR(50),
    IN p_last_name VARCHAR(50),
    IN p_email VARCHAR(100),
    IN p_designation VARCHAR(50)
)
BEGIN
    INSERT INTO teachers (first_name, last_name, email, designation)
    VALUES (p_first_name, p_last_name, p_email, p_designation);
END //

DROP PROCEDURE IF EXISTS sp_UpdateTeacher //
CREATE PROCEDURE sp_UpdateTeacher(
    IN p_teacher_id INT,
    IN p_first_name VARCHAR(50),
    IN p_last_name VARCHAR(50),
    IN p_email VARCHAR(100),
    IN p_designation VARCHAR(50)
    )
BEGIN
    UPDATE teachers
    SET first_name = p_first_name,
        last_name = p_last_name,
        email = p_email,
        designation = p_designation
    WHERE teacher_id = p_teacher_id;
END //

DROP PROCEDURE IF EXISTS sp_DeleteTeacher //
CREATE PROCEDURE sp_DeleteTeacher(
    IN p_teacher_id INT
)
BEGIN
    DELETE FROM teachers WHERE teacher_id = p_teacher_id;
END //

DROP PROCEDURE IF EXISTS sp_SearchTeachers //
CREATE PROCEDURE sp_SearchTeachers(
    IN p_keyword VARCHAR(100)
)
BEGIN
    SET @kw = CONCAT('%', p_keyword, '%');
    SELECT teacher_id, first_name, last_name, email, designation
    FROM teachers
    WHERE first_name LIKE @kw
       OR last_name LIKE @kw
       OR email LIKE @kw
       OR designation LIKE @kw
    ORDER BY last_name, first_name;
END //

-- ── SUBJECTS ─────────────────────────────────────────────

DROP PROCEDURE IF EXISTS sp_GetAllSubjects //
CREATE PROCEDURE sp_GetAllSubjects()
BEGIN
    SELECT subject_id, subject_code, subject_name
    FROM subjects
    ORDER BY subject_code;
END //

DROP PROCEDURE IF EXISTS sp_GetSubjectById //
CREATE PROCEDURE sp_GetSubjectById(
    IN p_subject_id INT
)
BEGIN
    SELECT subject_id, subject_code, subject_name
    FROM subjects
    WHERE subject_id = p_subject_id;
END //

DROP PROCEDURE IF EXISTS sp_AddSubject //
CREATE PROCEDURE sp_AddSubject(
    IN p_subject_code VARCHAR(15),
    IN p_subject_name VARCHAR(100)
)
BEGIN
    INSERT INTO subjects (subject_code, subject_name)
    VALUES (p_subject_code, p_subject_name);
END //

DROP PROCEDURE IF EXISTS sp_UpdateSubject //
CREATE PROCEDURE sp_UpdateSubject(
    IN p_subject_id INT,
    IN p_subject_code VARCHAR(15),
    IN p_subject_name VARCHAR(100)
)
BEGIN
    UPDATE subjects
    SET subject_code = p_subject_code,
        subject_name = p_subject_name
    WHERE subject_id = p_subject_id;
END //

DROP PROCEDURE IF EXISTS sp_DeleteSubject //
CREATE PROCEDURE sp_DeleteSubject(
    IN p_subject_id INT
)
BEGIN
    DELETE FROM subjects WHERE subject_id = p_subject_id;
END //

DROP PROCEDURE IF EXISTS sp_SearchSubjects //
CREATE PROCEDURE sp_SearchSubjects(
    IN p_keyword VARCHAR(100)
)
BEGIN
    SET @kw = CONCAT('%', p_keyword, '%');
    SELECT subject_id, subject_code, subject_name
    FROM subjects
    WHERE subject_code LIKE @kw
       OR subject_name LIKE @kw
    ORDER BY subject_code;
END //

-- ── CLASSES ──────────────────────────────────────────────

DROP PROCEDURE IF EXISTS sp_GetAllClasses //
CREATE PROCEDURE sp_GetAllClasses()
BEGIN
    SELECT c.class_id, c.subject_id, c.teacher_id,
           s.subject_code, s.subject_name,
           CONCAT(t.first_name, ' ', t.last_name) AS teacher_name,
           c.academic_year, c.semester, c.section
    FROM classes c
    INNER JOIN subjects s ON c.subject_id = s.subject_id
    INNER JOIN teachers t ON c.teacher_id = t.teacher_id
    ORDER BY c.academic_year DESC, c.semester, s.subject_code;
END //

DROP PROCEDURE IF EXISTS sp_GetClassById //
CREATE PROCEDURE sp_GetClassById(
    IN p_class_id INT
)
BEGIN
    SELECT c.class_id, c.subject_id, c.teacher_id,
           s.subject_code, s.subject_name,
           CONCAT(t.first_name, ' ', t.last_name) AS teacher_name,
           c.academic_year, c.semester, c.section
    FROM classes c
    INNER JOIN subjects s ON c.subject_id = s.subject_id
    INNER JOIN teachers t ON c.teacher_id = t.teacher_id
    WHERE c.class_id = p_class_id;
END //

DROP PROCEDURE IF EXISTS sp_GetClassesByTeacherId //
CREATE PROCEDURE sp_GetClassesByTeacherId(
    IN p_teacher_id INT
)
BEGIN
    SELECT c.class_id, c.subject_id, c.teacher_id,
           s.subject_code, s.subject_name,
           CONCAT(t.first_name, ' ', t.last_name) AS teacher_name,
           c.academic_year, c.semester, c.section
    FROM classes c
    INNER JOIN subjects s ON c.subject_id = s.subject_id
    INNER JOIN teachers t ON c.teacher_id = t.teacher_id
    WHERE c.teacher_id = p_teacher_id
    ORDER BY c.academic_year DESC, c.semester, s.subject_code;
END //

DROP PROCEDURE IF EXISTS sp_AddClass //
CREATE PROCEDURE sp_AddClass(
    IN p_subject_id INT,
    IN p_teacher_id INT,
    IN p_academic_year VARCHAR(9),
    IN p_semester INT,
    IN p_section VARCHAR(10)
)
BEGIN
    INSERT INTO classes (subject_id, teacher_id, academic_year, semester, section)
    VALUES (p_subject_id, p_teacher_id, p_academic_year, p_semester, p_section);
END //

DROP PROCEDURE IF EXISTS sp_UpdateClass //
CREATE PROCEDURE sp_UpdateClass(
    IN p_class_id INT,
    IN p_subject_id INT,
    IN p_teacher_id INT,
    IN p_academic_year VARCHAR(9),
    IN p_semester INT,
    IN p_section VARCHAR(10)
)
BEGIN
    UPDATE classes
    SET subject_id = p_subject_id,
        teacher_id = p_teacher_id,
        academic_year = p_academic_year,
        semester = p_semester,
        section = p_section
    WHERE class_id = p_class_id;
END //

DROP PROCEDURE IF EXISTS sp_DeleteClass //
CREATE PROCEDURE sp_DeleteClass(
    IN p_class_id INT
)
BEGIN
    DELETE FROM classes WHERE class_id = p_class_id;
END //

DROP PROCEDURE IF EXISTS sp_SearchClasses //
CREATE PROCEDURE sp_SearchClasses(
    IN p_keyword VARCHAR(100)
)
BEGIN
    SET @kw = CONCAT('%', p_keyword, '%');
    SELECT c.class_id, c.subject_id, c.teacher_id,
           s.subject_code, s.subject_name,
           CONCAT(t.first_name, ' ', t.last_name) AS teacher_name,
           c.academic_year, c.semester, c.section
    FROM classes c
    INNER JOIN subjects s ON c.subject_id = s.subject_id
    INNER JOIN teachers t ON c.teacher_id = t.teacher_id
    WHERE s.subject_code LIKE @kw
       OR s.subject_name LIKE @kw
       OR t.first_name LIKE @kw
       OR t.last_name LIKE @kw
       OR c.academic_year LIKE @kw
       OR c.section LIKE @kw
    ORDER BY c.academic_year DESC, c.semester, s.subject_code;
END //

-- ── SCHEDULES ────────────────────────────────────────────

DROP PROCEDURE IF EXISTS sp_GetSchedulesByClassId //
CREATE PROCEDURE sp_GetSchedulesByClassId(
    IN p_class_id INT
)
BEGIN
    SELECT schedule_id, class_id, day_of_week, start_time, end_time, room
    FROM class_schedule
    WHERE class_id = p_class_id
    ORDER BY day_of_week, start_time;
END //

DROP PROCEDURE IF EXISTS sp_AddSchedule //
CREATE PROCEDURE sp_AddSchedule(
    IN p_class_id INT,
    IN p_day_of_week INT,
    IN p_start_time VARCHAR(20),
    IN p_end_time VARCHAR(20),
    IN p_room VARCHAR(30)
)
BEGIN
    INSERT INTO class_schedule (class_id, day_of_week, start_time, end_time, room)
    VALUES (p_class_id, p_day_of_week, p_start_time, p_end_time, p_room);
END //

DROP PROCEDURE IF EXISTS sp_UpdateSchedule //
CREATE PROCEDURE sp_UpdateSchedule(
    IN p_schedule_id INT,
    IN p_day_of_week INT,
    IN p_start_time VARCHAR(20),
    IN p_end_time VARCHAR(20),
    IN p_room VARCHAR(30)
)
BEGIN
    UPDATE class_schedule
    SET day_of_week = p_day_of_week,
        start_time = p_start_time,
        end_time = p_end_time,
        room = p_room
    WHERE schedule_id = p_schedule_id;
END //

DROP PROCEDURE IF EXISTS sp_DeleteSchedule //
CREATE PROCEDURE sp_DeleteSchedule(
    IN p_schedule_id INT
)
BEGIN
    DELETE FROM class_schedule WHERE schedule_id = p_schedule_id;
END //

-- ── ENROLLMENTS ──────────────────────────────────────────

DROP PROCEDURE IF EXISTS sp_GetEnrolledStudents //
CREATE PROCEDURE sp_GetEnrolledStudents(
    IN p_class_id INT
)
BEGIN
    SELECT e.enrollment_id, e.class_id, e.enrolled_at,
           s.student_id, s.registration_no, s.first_name, 
           s.last_name, CONCAT(s.first_name, ' ', s.last_name) AS student_name,
           s.gender, s.date_of_birth
    FROM enrollments e
    INNER JOIN students s ON e.student_id = s.student_id
    WHERE e.class_id = p_class_id
    ORDER BY s.last_name, s.first_name;
END //

DROP PROCEDURE IF EXISTS sp_GetAvailableStudents //
CREATE PROCEDURE sp_GetAvailableStudents(
    IN p_class_id INT
)
BEGIN
    SELECT s.student_id, s.registration_no, s.first_name, 
           s.last_name, CONCAT(s.first_name, ' ', s.last_name) AS student_name,
           s.gender, s.date_of_birth
    FROM students s
    WHERE s.student_id NOT IN (
        SELECT e.student_id 
        FROM enrollments e 
        WHERE e.class_id = p_class_id
    )
    ORDER BY s.last_name, s.first_name;
END //

DROP PROCEDURE IF EXISTS sp_EnrollStudent //
CREATE PROCEDURE sp_EnrollStudent(
    IN p_class_id INT,
    IN p_student_id INT
)
BEGIN
    INSERT INTO enrollments (class_id, student_id)
    VALUES (p_class_id, p_student_id);
END //

DROP PROCEDURE IF EXISTS sp_UnenrollStudent //
CREATE PROCEDURE sp_UnenrollStudent(
    IN p_enrollment_id INT
)
BEGIN
    DELETE FROM enrollments WHERE enrollment_id = p_enrollment_id;
END //

DROP PROCEDURE IF EXISTS sp_GetStudentsByTeacherId //
CREATE PROCEDURE sp_GetStudentsByTeacherId(
    IN p_teacher_id INT
)
BEGIN
    SELECT DISTINCT s.student_id, s.registration_no, s.first_name, s.last_name,
           s.gender, s.date_of_birth
    FROM students s
    INNER JOIN enrollments e ON s.student_id = e.student_id
    INNER JOIN classes c ON e.class_id = c.class_id
    WHERE c.teacher_id = p_teacher_id
    ORDER BY s.last_name, s.first_name;
END //

-- ── ATTENDANCE SESSIONS & RECORDS ────────────────────────

DROP PROCEDURE IF EXISTS sp_GetSessionsByClassId //
CREATE PROCEDURE sp_GetSessionsByClassId(
    IN p_class_id INT
)
BEGIN
    SELECT a.session_id, a.class_id, a.schedule_id, a.session_date,
           a.created_by_teacher_id,
           CONCAT(t.first_name, ' ', t.last_name) AS teacher_name
    FROM attendance_sessions a
    INNER JOIN teachers t ON a.created_by_teacher_id = t.teacher_id
    WHERE a.class_id = p_class_id
    ORDER BY a.session_date DESC;
END //

DROP PROCEDURE IF EXISTS sp_GetSessionsByTeacherId //
CREATE PROCEDURE sp_GetSessionsByTeacherId(
    IN p_teacher_id INT
)
BEGIN
    SELECT a.session_id,
           a.class_id,
           a.schedule_id,
           a.session_date,
           a.created_by_teacher_id,
           CONCAT(t.first_name, ' ', t.last_name) AS teacher_name
    FROM attendance_sessions a
    INNER JOIN teachers t ON a.created_by_teacher_id = t.teacher_id
    WHERE a.created_by_teacher_id = p_teacher_id
    ORDER BY a.session_date DESC;
END //

DROP PROCEDURE IF EXISTS sp_CreateAttendanceSession //
CREATE PROCEDURE sp_CreateAttendanceSession(
    IN p_class_id INT,
    IN p_schedule_id INT,
    IN p_session_date DATE,
    IN p_created_by_teacher_id INT
)
BEGIN
    INSERT INTO attendance_sessions (class_id, schedule_id, session_date, created_by_teacher_id)
    VALUES (p_class_id, p_schedule_id, p_session_date, p_created_by_teacher_id);
    SELECT LAST_INSERT_ID() AS session_id;
END //

DROP PROCEDURE IF EXISTS sp_DeleteAttendanceSession //
CREATE PROCEDURE sp_DeleteAttendanceSession(
    IN p_session_id INT
)
BEGIN
    -- Remove child remarks first (FK: session_id+student_id → attendance_records)
    DELETE FROM remarks
    WHERE session_id = p_session_id;

    -- Remove attendance records for this session
    DELETE FROM attendance_records
    WHERE session_id = p_session_id;

    -- Remove the session itself
    DELETE FROM attendance_sessions
    WHERE session_id = p_session_id;
END //

DROP PROCEDURE IF EXISTS sp_GetAttendanceRecords //
CREATE PROCEDURE sp_GetAttendanceRecords(
    IN p_session_id INT
)
BEGIN
    SELECT ar.attendance_record_id AS record_id,
           ar.session_id,
           ar.student_id,
           s.registration_no,
           CONCAT(s.first_name, ' ', s.last_name) AS student_name,
           ast.status_name                                  AS status,
           ar.time_in
    FROM attendance_records ar
    INNER JOIN students s ON ar.student_id = s.student_id
    INNER JOIN attendance_status ast ON ar.status_id = ast.status_id
    WHERE ar.session_id = p_session_id
    ORDER BY s.last_name, s.first_name;
END //

DROP PROCEDURE IF EXISTS sp_SaveAttendanceRecord //
CREATE PROCEDURE sp_SaveAttendanceRecord(
    IN p_session_id INT,
    IN p_student_id INT,
    IN p_status     VARCHAR(20),
    IN p_time_in    DATETIME
)
BEGIN
    INSERT INTO attendance_records (session_id, student_id, status_id, time_in, marked_at)
    SELECT p_session_id,
           p_student_id,
           ast.status_id,
           p_time_in,
           CURRENT_TIMESTAMP
    FROM attendance_status ast
    WHERE ast.status_name = p_status
    ON DUPLICATE KEY UPDATE
        status_id  = VALUES(status_id),
        time_in    = VALUES(time_in),
        marked_at  = CURRENT_TIMESTAMP;
END //

-- ── REMARKS ──────────────────────────────────────────────

DROP PROCEDURE IF EXISTS sp_GetRemarksBySession //
CREATE PROCEDURE sp_GetRemarksBySession(
    IN p_session_id INT
)
BEGIN
    SELECT r.remark_id, r.session_id, r.student_id,
           CONCAT(s.first_name, ' ', s.last_name) AS student_name,
           r.teacher_id,
           CONCAT(t.first_name, ' ', t.last_name) AS teacher_name,
           r.category_id,
           rc.category_name,
           r.remark_text, r.remark_date
    FROM remarks r
    INNER JOIN students s ON r.student_id = s.student_id
    INNER JOIN teachers t ON r.teacher_id = t.teacher_id
    LEFT JOIN remark_categories rc ON r.category_id = rc.category_id
    WHERE r.session_id = p_session_id
    ORDER BY r.remark_date DESC;
END //

DROP PROCEDURE IF EXISTS sp_AddRemark //
CREATE PROCEDURE sp_AddRemark(
    IN p_session_id INT,
    IN p_student_id INT,
    IN p_teacher_id INT,
    IN p_category_id INT,
    IN p_remark_text VARCHAR(255)
)
BEGIN
    INSERT INTO remarks (session_id, student_id, teacher_id, category_id, remark_text)
    VALUES (p_session_id, p_student_id, p_teacher_id, p_category_id, p_remark_text);
END //

DROP PROCEDURE IF EXISTS sp_UpdateRemark //
CREATE PROCEDURE sp_UpdateRemark(
    IN p_remark_id INT,
    IN p_category_id INT,
    IN p_remark_text VARCHAR(255)
)
BEGIN
    UPDATE remarks
    SET category_id = p_category_id,
        remark_text = p_remark_text
    WHERE remark_id = p_remark_id;
END //

DROP PROCEDURE IF EXISTS sp_DeleteRemark //
CREATE PROCEDURE sp_DeleteRemark(
    IN p_remark_id INT
)
BEGIN
    DELETE FROM remarks WHERE remark_id = p_remark_id;
END //

DROP PROCEDURE IF EXISTS sp_GetAllRemarkCategories //
CREATE PROCEDURE sp_GetAllRemarkCategories()
BEGIN
    SELECT category_id, category_name
    FROM remark_categories
    ORDER BY category_id;
END //

DROP PROCEDURE IF EXISTS sp_GetStudentsBySession //
CREATE PROCEDURE sp_GetStudentsBySession(
    IN p_session_id INT
)
BEGIN
    SELECT DISTINCT ar.student_id, s.registration_no,
           CONCAT(s.first_name, ' ', s.last_name) AS student_name
    FROM attendance_records ar
    INNER JOIN students s ON ar.student_id = s.student_id
    WHERE ar.session_id = p_session_id
    ORDER BY s.last_name, s.first_name;
END //

DROP PROCEDURE IF EXISTS sp_GetAllRemarks //
CREATE PROCEDURE sp_GetAllRemarks()
BEGIN
    SELECT r.remark_id, r.session_id, r.student_id,
           CONCAT(s.first_name, ' ', s.last_name) AS student_name,
           r.teacher_id,
           CONCAT(t.first_name, ' ', t.last_name) AS teacher_name,
           r.category_id,
           rc.category_name,
           r.remark_text, r.remark_date
    FROM remarks r
    INNER JOIN students s ON r.student_id = s.student_id
    INNER JOIN teachers t ON r.teacher_id = t.teacher_id
    LEFT JOIN remark_categories rc ON r.category_id = rc.category_id
    ORDER BY r.remark_date DESC;
END //

DROP PROCEDURE IF EXISTS sp_GetRemarksByStudentId //
CREATE PROCEDURE sp_GetRemarksByStudentId(
    IN p_student_id INT
)
BEGIN
    SELECT r.remark_id, r.session_id, r.student_id,
           CONCAT(s.first_name, ' ', s.last_name) AS student_name,
           r.teacher_id,
           CONCAT(t.first_name, ' ', t.last_name) AS teacher_name,
           r.category_id,
           rc.category_name,
           r.remark_text, r.remark_date
    FROM remarks r
    INNER JOIN students s ON r.student_id = s.student_id
    INNER JOIN teachers t ON r.teacher_id = t.teacher_id
    LEFT JOIN remark_categories rc ON r.category_id = rc.category_id
    WHERE r.student_id = p_student_id
    ORDER BY r.remark_date DESC;
END //

DROP PROCEDURE IF EXISTS sp_SearchRemarks //
CREATE PROCEDURE sp_SearchRemarks(
    IN p_keyword VARCHAR(100)
)
BEGIN
    SET @kw = CONCAT('%', p_keyword, '%');
    SELECT r.remark_id, r.session_id, r.student_id,
           CONCAT(s.first_name, ' ', s.last_name) AS student_name,
           r.teacher_id,
           CONCAT(t.first_name, ' ', t.last_name) AS teacher_name,
           r.category_id,
           rc.category_name,
           r.remark_text, r.remark_date
    FROM remarks r
    INNER JOIN students s ON r.student_id = s.student_id
    INNER JOIN teachers t ON r.teacher_id = t.teacher_id
    LEFT JOIN remark_categories rc ON r.category_id = rc.category_id
    WHERE s.first_name LIKE @kw
       OR s.last_name LIKE @kw
       OR r.remark_text LIKE @kw
       OR t.first_name LIKE @kw
       OR t.last_name LIKE @kw
       OR rc.category_name LIKE @kw
    ORDER BY r.remark_date DESC;
END //

DELIMITER ;