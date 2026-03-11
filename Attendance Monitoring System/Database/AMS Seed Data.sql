-- ============================================================
-- AMS Seed Data
-- Run this AFTER executing "AMS DB.sql" to populate test data.
-- ============================================================

USE attendance_db;

-- ── Teachers (5) ─────────────────────────────────────────────
INSERT INTO teachers (first_name, last_name, email, designation) VALUES
('Maria',   'Santos',   'maria.santos@school.edu',   'Professor'),
('Roberto', 'Cruz',     'roberto.cruz@school.edu',   'Associate Professor'),
('Angela',  'Reyes',    'angela.reyes@school.edu',   'Instructor'),
('Carlos',  'Garcia',   'carlos.garcia@school.edu',  'Assistant Professor'),
('Diana',   'Flores',   'diana.flores@school.edu',   'Professor');

-- ── Students (5) ─────────────────────────────────────────────
INSERT INTO students (registration_no, first_name, last_name, gender, date_of_birth) VALUES
('2025-0001', 'Jane',    'Doe',      'Female', '2003-01-15'),
('2025-0002', 'John',    'Smith',    'Male',   '2004-03-22'),
('2025-0003', 'Emily',   'Johnson',  'Female', '2003-06-10'),
('2025-0004', 'Michael', 'Williams', 'Male',   '2004-09-05'),
('2025-0005', 'Sarah',   'Brown',    'Female', '2003-12-18');

-- ── Subjects (5) ─────────────────────────────────────────────
INSERT INTO subjects (subject_code, subject_name) VALUES
('CS101', 'Introduction to Computer Science'),
('CS201', 'Data Structures and Algorithms'),
('CS301', 'Database Management Systems'),
('MA101', 'Calculus I'),
('EN101', 'English Communication');

-- ── Classes (5) ──────────────────────────────────────────────
-- teacher_id references: 1=Santos, 2=Cruz, 3=Reyes, 4=Garcia, 5=Flores
INSERT INTO classes (subject_id, teacher_id, academic_year, semester, section) VALUES
(1, 1, '2024-2025', 1, 'A'),
(2, 2, '2024-2025', 1, 'A'),
(3, 1, '2024-2025', 2, 'A'),
(4, 4, '2024-2025', 1, 'B'),
(5, 5, '2024-2025', 2, 'A');

-- ── Class Schedules (5) ──────────────────────────────────────
-- day_of_week: 1=Mon, 2=Tue, 3=Wed, 4=Thu, 5=Fri
INSERT INTO class_schedule (class_id, day_of_week, start_time, end_time, room) VALUES
(1, 1, '08:00 AM', '09:30 AM', 'Room 101'),
(2, 2, '10:00 AM', '11:30 AM', 'Room 202'),
(3, 3, '01:00 PM', '02:30 PM', 'Room 303'),
(4, 4, '08:00 AM', '09:30 AM', 'Room 104'),
(5, 5, '10:00 AM', '11:30 AM', 'Room 205');

-- ── Enrollments (10 — each student in 2 classes) ────────────
INSERT INTO enrollments (class_id, student_id) VALUES
(1, 1), (1, 2), (1, 3),    -- CS101-A: Doe, Smith, Johnson
(2, 2), (2, 4), (2, 5),    -- CS201-A: Smith, Williams, Brown
(3, 1), (3, 3),             -- CS301-A: Doe, Johnson
(4, 4), (4, 5);             -- MA101-B: Williams, Brown

-- ── Attendance Sessions (5) ──────────────────────────────────
INSERT INTO attendance_sessions (class_id, session_name, session_date, created_by_teacher_id) VALUES
(1, 'Week 1 Lecture',  '2025-01-06', 1),
(1, 'Week 2 Lecture',  '2025-01-13', 1),
(2, 'Week 1 Lab',      '2025-01-07', 2),
(3, NULL,               '2025-01-08', 1),
(4, 'Midterm Review',   '2025-01-09', 4);

-- ── Attendance Records (15) ──────────────────────────────────
-- Session 1: CS101-A Week 1 (Doe, Smith, Johnson)
INSERT INTO attendance_records (session_id, student_id, status_id, time_in) VALUES
(1, 1, 1, '2025-01-06 08:05:00'),   -- Doe: Present
(1, 2, 3, '2025-01-06 08:22:00'),   -- Smith: Late
(1, 3, 1, '2025-01-06 08:03:00');   -- Johnson: Present

-- Session 2: CS101-A Week 2 (Doe, Smith, Johnson)
INSERT INTO attendance_records (session_id, student_id, status_id, time_in) VALUES
(2, 1, 1, '2025-01-13 08:01:00'),   -- Doe: Present
(2, 2, 2, NULL),                     -- Smith: Absent
(2, 3, 4, NULL);                     -- Johnson: Excused

-- Session 3: CS201-A Week 1 (Smith, Williams, Brown)
INSERT INTO attendance_records (session_id, student_id, status_id, time_in) VALUES
(3, 2, 1, '2025-01-07 10:02:00'),   -- Smith: Present
(3, 4, 1, '2025-01-07 10:00:00'),   -- Williams: Present
(3, 5, 3, '2025-01-07 10:18:00');   -- Brown: Late

-- Session 4: CS301-A (Doe, Johnson)
INSERT INTO attendance_records (session_id, student_id, status_id, time_in) VALUES
(4, 1, 1, '2025-01-08 13:00:00'),   -- Doe: Present
(4, 3, 2, NULL);                     -- Johnson: Absent

-- Session 5: MA101-B Midterm Review (Williams, Brown)
INSERT INTO attendance_records (session_id, student_id, status_id, time_in) VALUES
(5, 4, 1, '2025-01-09 08:02:00'),   -- Williams: Present
(5, 5, 1, '2025-01-09 08:00:00');   -- Brown: Present

-- ── Remarks (5) ──────────────────────────────────────────────
INSERT INTO remarks (session_id, student_id, teacher_id, category_id, remark_text) VALUES
(1, 2, 1, 1, 'Arrived 22 minutes late due to traffic.'),
(2, 2, 1, 4, 'Did not attend, no notice given.'),
(2, 3, 1, 2, 'Excused — submitted medical certificate.'),
(3, 5, 2, 1, 'Arrived 18 minutes late, warned about punctuality.'),
(4, 3, 1, 3, 'Missed class, needs to submit makeup assignment.');

-- ── Users (11) ───────────────────────────────────────────────
-- Passwords in comments for reference (hashed with BCrypt below):
--   Admin:     admin      / admin123
--   Registrar: registrar  / registrar123
--   Teachers:  t.santos, t.cruz, t.reyes, t.garcia, t.flores / teacher123
--   Students:  auto-generated username = registration_no
--              password = lastname (lowercase) + DOB (MMddyyyy)
--     2025-0001 / doe01152003
--     2025-0002 / smith03222004
--     2025-0003 / johnson06102003
--     2025-0004 / williams09052004
--     2025-0005 / brown12182003

INSERT INTO users (username, password_hash, role, teacher_id, student_id) VALUES
-- Admin
('admin',
 '$2a$11$tSPq2Qb6irgXpKTYKFR9o.F0qdqhtZC.OtqWqRKDKF67v1/aaSdWm',
 'Admin', NULL, NULL),

-- Registrar
('registrar',
 '$2a$11$0is1tutPQtk291qm212F8eqfqn8D.I7UE6Yyb6OFevrhQ6Dhl5K26',
 'Registrar', NULL, NULL),

-- Teachers (linked to teacher_id 1–5)
('t.santos',
 '$2a$11$FGfBxKS4.8XtM0EFcwyhIeLuZt6IFkBc6gm9AscRUf24ZU6DDPVDa',
 'Teacher', 1, NULL),
('t.cruz',
 '$2a$11$b1Q.H.xGI5ejHdtZ/zzUFO2Teh/VjO7x6HMSI8xxdBAVeOXrnIhiW',
 'Teacher', 2, NULL),
('t.reyes',
 '$2a$11$NgT5rlo1yZy4kSSK2lpV1.iRzfcsr9wn2OtDmYlPyT75wHaVkIFU2',
 'Teacher', 3, NULL),
('t.garcia',
 '$2a$11$DXS0HfhHDSLSEMNi9ur3.OfclhAJOW/xNrv6u/h1jIOLl/J/sfMK2',
 'Teacher', 4, NULL),
('t.flores',
 '$2a$11$VxlUwGC1zOiUM4R5iDYr/epovWRj9a/BBU8DqYLC5l844mnJ7Vqc.',
 'Teacher', 5, NULL),

-- Students (linked to student_id 1–5)
('2025-0001',
 '$2a$11$TD6tMsMJlABi8GL/hEobROehcw7RdsHgoz3gciGKdSLSoYLH5adwm',
 'Student', NULL, 1),
('2025-0002',
 '$2a$11$TIeqjv/Geolk42y4s4F6derXewJYoBoKZoeNHzRdF3pJqP73ybGh6',
 'Student', NULL, 2),
('2025-0003',
 '$2a$11$w/TpvYE7mdR6oONhMtlGQ.ZohZ3ELDTmIbJ5Rzz5t0wzXZH0C8W.e',
 'Student', NULL, 3),
('2025-0004',
 '$2a$11$0ab0Aavr2xyC.DnKFEcjCujZSPupZKSkw7.Xjs2oF20MJ7OpG4vHS',
 'Student', NULL, 4),
('2025-0005',
 '$2a$11$JaBJTn3//JxoKM1TwhguxOefn/ulrLnpmF0p/lQb2s/A0Xvz8HvOK',
 'Student', NULL, 5);
