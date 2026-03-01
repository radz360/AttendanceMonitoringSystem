USE attendance_db;

-- ============================================================
-- USERS (Auth)
-- ============================================================

DELIMITER //

CREATE PROCEDURE sp_GetUserByUsername(
    IN p_username VARCHAR(50)
)
BEGIN
    SELECT user_id, username, password_hash, role, teacher_id, is_active
    FROM users
    WHERE username = p_username;
END //

CREATE PROCEDURE sp_CreateUser(
    IN p_username VARCHAR(50),
    IN p_password_hash VARCHAR(255),
    IN p_role ENUM('Admin','Teacher'),
    IN p_teacher_id INT
)
BEGIN
    INSERT INTO users (username, password_hash, role, teacher_id)
    VALUES (p_username, p_password_hash, p_role, p_teacher_id);
END //

CREATE PROCEDURE sp_GetAllUsers()
BEGIN
    SELECT u.user_id, u.username, u.role, u.teacher_id,
           CONCAT(t.first_name, ' ', t.last_name) AS teacher_name,
           u.is_active
    FROM users u
    LEFT JOIN teachers t ON u.teacher_id = t.teacher_id
    ORDER BY u.username;
END //

CREATE PROCEDURE sp_UpdateUser(
    IN p_user_id INT,
    IN p_username VARCHAR(50),
    IN p_role ENUM('Admin','Teacher'),
    IN p_teacher_id INT,
    IN p_is_active TINYINT(1)
)
BEGIN
    UPDATE users
    SET username = p_username,
        role = p_role,
        teacher_id = p_teacher_id,
        is_active = p_is_active
    WHERE user_id = p_user_id;
END //

CREATE PROCEDURE sp_ResetUserPassword(
    IN p_user_id INT,
    IN p_password_hash VARCHAR(255)
)
BEGIN
    UPDATE users
    SET password_hash = p_password_hash
    WHERE user_id = p_user_id;
END //

-- ============================================================
-- STUDENTS
-- ============================================================

CREATE PROCEDURE sp_GetAllStudents()
BEGIN
    SELECT student_id, registration_no, first_name, last_name,
           gender, date_of_birth
    FROM students
    ORDER BY last_name, first_name;
END //

CREATE PROCEDURE sp_GetStudentById(
    IN p_student_id INT
)
BEGIN
    SELECT student_id, registration_no, first_name, last_name,
           gender, date_of_birth
    FROM students
    WHERE student_id = p_student_id;
END //

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

CREATE PROCEDURE sp_DeleteStudent(
    IN p_student_id INT
)
BEGIN
    DELETE FROM students WHERE student_id = p_student_id;
END //

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

-- ============================================================
-- TEACHERS
-- ============================================================

CREATE PROCEDURE sp_GetAllTeachers()
BEGIN
    SELECT teacher_id, first_name, last_name, email, designation
    FROM teachers
    ORDER BY last_name, first_name;
END //

CREATE PROCEDURE sp_GetTeacherById(
    IN p_teacher_id INT
)
BEGIN
    SELECT teacher_id, first_name, last_name, email, designation
    FROM teachers
    WHERE teacher_id = p_teacher_id;
END //

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

CREATE PROCEDURE sp_DeleteTeacher(
    IN p_teacher_id INT
)
BEGIN
    DELETE FROM teachers WHERE teacher_id = p_teacher_id;
END //

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

-- ============================================================
-- SUBJECTS
-- ============================================================

CREATE PROCEDURE sp_GetAllSubjects()
BEGIN
    SELECT subject_id, subject_code, subject_name
    FROM subjects
    ORDER BY subject_code;
END //

CREATE PROCEDURE sp_GetSubjectById(
    IN p_subject_id INT
)
BEGIN
    SELECT subject_id, subject_code, subject_name
    FROM subjects
    WHERE subject_id = p_subject_id;
END //

CREATE PROCEDURE sp_AddSubject(
    IN p_subject_code VARCHAR(15),
    IN p_subject_name VARCHAR(100)
)
BEGIN
    INSERT INTO subjects (subject_code, subject_name)
    VALUES (p_subject_code, p_subject_name);
END //

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

CREATE PROCEDURE sp_DeleteSubject(
    IN p_subject_id INT
)
BEGIN
    DELETE FROM subjects WHERE subject_id = p_subject_id;
END //

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

-- ============================================================
-- CLASSES
-- ============================================================

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

CREATE PROCEDURE sp_DeleteClass(
    IN p_class_id INT
)
BEGIN
    DELETE FROM classes WHERE class_id = p_class_id;
END //

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

-- ============================================================
-- CLASS SCHEDULE
-- ============================================================

CREATE PROCEDURE sp_GetSchedulesByClassId(
    IN p_class_id INT
)
BEGIN
    SELECT schedule_id, class_id, day_of_week, start_time, end_time, room
    FROM class_schedule
    WHERE class_id = p_class_id
    ORDER BY day_of_week, start_time;
END //

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

CREATE PROCEDURE sp_DeleteSchedule(
    IN p_schedule_id INT
)
BEGIN
    DELETE FROM class_schedule WHERE schedule_id = p_schedule_id;
END //

-- ============================================================
-- ENROLLMENTS
-- ============================================================

CREATE PROCEDURE sp_GetEnrollmentsByClassId(
    IN p_class_id INT
)
BEGIN
    SELECT e.enrollment_id, e.class_id, e.student_id,
           s.registration_no,
           CONCAT(s.first_name, ' ', s.last_name) AS student_name,
           e.enrolled_at
    FROM enrollments e
    INNER JOIN students s ON e.student_id = s.student_id
    WHERE e.class_id = p_class_id
    ORDER BY s.last_name, s.first_name;
END //

CREATE PROCEDURE sp_GetUnenrolledStudents(
    IN p_class_id INT
)
BEGIN
    SELECT s.student_id, s.registration_no,
           CONCAT(s.first_name, ' ', s.last_name) AS student_name
    FROM students s
    WHERE s.student_id NOT IN (
        SELECT student_id FROM enrollments WHERE class_id = p_class_id
    )
    ORDER BY s.last_name, s.first_name;
END //

CREATE PROCEDURE sp_EnrollStudent(
    IN p_class_id INT,
    IN p_student_id INT
)
BEGIN
    INSERT INTO enrollments (class_id, student_id)
    VALUES (p_class_id, p_student_id);
END //

CREATE PROCEDURE sp_UnenrollStudent(
    IN p_enrollment_id INT
)
BEGIN
    DELETE FROM enrollments WHERE enrollment_id = p_enrollment_id;
END //

-- ============================================================
-- ATTENDANCE SESSIONS
-- ============================================================

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

CREATE PROCEDURE sp_CreateSession(
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

-- ============================================================
-- ATTENDANCE RECORDS
-- ============================================================

CREATE PROCEDURE sp_GetAttendanceBySession(
    IN p_session_id INT
)
BEGIN
    SELECT ar.attendance_record_id, ar.session_id, ar.student_id,
           s.registration_no,
           CONCAT(s.first_name, ' ', s.last_name) AS student_name,
           ar.status_id,
           ast.status_name,
           ar.marked_at
    FROM attendance_records ar
    INNER JOIN students s ON ar.student_id = s.student_id
    INNER JOIN attendance_status ast ON ar.status_id = ast.status_id
    WHERE ar.session_id = p_session_id
    ORDER BY s.last_name, s.first_name;
END //

CREATE PROCEDURE sp_AddAttendanceRecord(
    IN p_session_id INT,
    IN p_student_id INT,
    IN p_status_id INT
)
BEGIN
    INSERT INTO attendance_records (session_id, student_id, status_id)
    VALUES (p_session_id, p_student_id, p_status_id);
END //

CREATE PROCEDURE sp_UpdateAttendanceRecord(
    IN p_attendance_record_id INT,
    IN p_status_id INT
)
BEGIN
    UPDATE attendance_records
    SET status_id = p_status_id,
        marked_at = CURRENT_TIMESTAMP
    WHERE attendance_record_id = p_attendance_record_id;
END //

-- ============================================================
-- REMARKS
-- ============================================================

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

CREATE PROCEDURE sp_DeleteRemark(
    IN p_remark_id INT
)
BEGIN
    DELETE FROM remarks WHERE remark_id = p_remark_id;
END //

DELIMITER ;