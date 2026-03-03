USE attendance_db;

-- ─────────────────────────────────────────────────────────
-- ALTER attendance_records to add time_in column
-- (run once; safe to ignore "Duplicate column" error if
--  applied to an existing database)
-- ─────────────────────────────────────────────────────────

ALTER TABLE attendance_records
    ADD COLUMN IF NOT EXISTS time_in DATETIME NULL;

DELIMITER //

-- ─────────────────────────────────────────────────────────
-- ATTENDANCE SESSIONS
-- ─────────────────────────────────────────────────────────

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

-- ─────────────────────────────────────────────────────────
-- ATTENDANCE RECORDS
-- ─────────────────────────────────────────────────────────

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
           ast.status_name                          AS status,
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

-- ─────────────────────────────────────────────────────────
-- SESSIONS BY TEACHER
-- ─────────────────────────────────────────────────────────

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

-- ─────────────────────────────────────────────────────────
-- CLASSES BY TEACHER (for AttendanceForm dropdown)
-- ─────────────────────────────────────────────────────────

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

DELIMITER ;
