USE attendance_db;

-- ── Teachers 
INSERT INTO teachers (first_name, last_name, email, designation) VALUES
('Maria',   'Santos',   'maria.santos@school.edu',   'Professor'),
('Roberto', 'Cruz',     'roberto.cruz@school.edu',   'Associate Professor'),
('Angela',  'Reyes',    'angela.reyes@school.edu',   'Instructor'),
('Carlos',  'Garcia',   'carlos.garcia@school.edu',  'Assistant Professor'),
('Diana',   'Flores',   'diana.flores@school.edu',   'Professor'),
('Paul', 'Gomez', 'paul.gomez@school.edu', 'Instructor'),
('Lisa', 'Torres', 'lisa.torres@school.edu', 'Assistant Professor'),
('Mark', 'Bautista', 'mark.bautista@school.edu', 'Professor'),
('Anna', 'Villanueva', 'anna.v@school.edu', 'Associate Professor'),
('James', 'Perez', 'james.perez@school.edu', 'Instructor'),
('Laura', 'Mendoza', 'laura.mendoza@school.edu', 'Assistant Professor'),
('Kevin', 'Castro', 'kevin.castro@school.edu', 'Instructor'),
('Rachel', 'Aquino', 'rachel.aquino@school.edu', 'Professor'),
('Brian', 'Navarro', 'brian.navarro@school.edu', 'Associate Professor'),
('Sarah', 'Morales', 'sarah.morales@school.edu', 'Instructor'),
('David', 'Ramos', 'david.ramos@school.edu', 'Assistant Professor'),
('Jessica', 'Lopez', 'jessica.lopez@school.edu', 'Instructor'),
('Daniel', 'Sison', 'daniel.sison@school.edu', 'Professor'),
('Michelle', 'Domingo', 'michelle.domingo@school.edu', 'Instructor'),
('Chris', 'Rivera', 'chris.rivera@school.edu', 'Associate Professor');

-- ── Students 
INSERT INTO students (registration_no, first_name, last_name, gender, date_of_birth) VALUES
('2025-0001', 'Jane',    'Doe',      'Female', '2003-01-15'),
('2025-0002', 'John',    'Smith',    'Male',   '2004-03-22'),
('2025-0003', 'Emily',   'Johnson',  'Female', '2003-06-10'),
('2025-0004', 'Michael', 'Williams', 'Male',   '2004-09-05'),
('2025-0005', 'Sarah',   'Brown',    'Female', '2003-12-18'),
('2025-0006', 'David', 'Miller', 'Male', '2004-02-11'),
('2025-0007', 'Sophia', 'Wilson', 'Female', '2003-08-25'),
('2025-0008', 'James', 'Moore', 'Male', '2004-11-03'),
('2025-0009', 'Olivia', 'Taylor', 'Female', '2003-04-14'),
('2025-0010', 'Daniel', 'Anderson', 'Male', '2004-07-08'),
('2025-0011', 'Emma', 'Thomas', 'Female', '2003-09-30'),
('2025-0012', 'Matthew', 'Jackson', 'Male', '2004-01-19'),
('2025-0013', 'Isabella', 'White', 'Female', '2003-05-22'),
('2025-0014', 'Joseph', 'Harris', 'Male', '2004-10-12'),
('2025-0015', 'Mia', 'Martin', 'Female', '2003-03-05'),
('2025-0016', 'Lucas', 'Thompson', 'Male', '2004-08-17'),
('2025-0017', 'Charlotte', 'Garcia', 'Female', '2003-11-28'),
('2025-0018', 'Ethan', 'Martinez', 'Male', '2004-06-09'),
('2025-0019', 'Amelia', 'Robinson', 'Female', '2003-02-14'),
('2025-0020', 'Alexander', 'Clark', 'Male', '2004-12-01');

-- ── Subjects 
INSERT INTO subjects (subject_code, subject_name) VALUES
('CS101', 'Introduction to Computer Science'),
('CS201', 'Data Structures and Algorithms'),
('CS301', 'Database Management Systems'),
('MA101', 'Calculus I'),
('EN101', 'English Communication'),
('PHY101', 'General Physics I'), ('PHY102', 'General Physics II'),
('CHM101', 'General Chemistry'), ('BIO101', 'Introduction to Biology'),
('HIS101', 'World History'), ('PHI101', 'Introduction to Philosophy'),
('PSY101', 'General Psychology'), ('SOC101', 'Introduction to Sociology'),
('ECO101', 'Microeconomics'), ('ECO102', 'Macroeconomics'),
('ART101', 'Art Appreciation'), ('MUS101', 'Music History'),
('PE101', 'Physical Education I'), ('PE102', 'Physical Education II'),
('LIT101', 'World Literature');

-- ── Classes
-- teacher_id references: 1=Santos, 2=Cruz, 3=Reyes, 4=Garcia, 5=Flores
INSERT INTO classes (subject_id, teacher_id, academic_year, semester, section) VALUES
(1, 1, '2024-2025', 1, 'A'),
(2, 2, '2024-2025', 1, 'A'),
(3, 1, '2024-2025', 2, 'A'),
(4, 4, '2024-2025', 1, 'B'),
(5, 5, '2024-2025', 2, 'A'),
(6, 6, '2024-2025', 1, 'A'), (7, 7, '2024-2025', 1, 'A'),
(8, 8, '2024-2025', 1, 'B'), (9, 9, '2024-2025', 2, 'A'),
(10, 10, '2024-2025', 2, 'C'), (11, 11, '2024-2025', 1, 'A'),
(12, 12, '2024-2025', 1, 'B'), (13, 13, '2024-2025', 2, 'A'),
(14, 14, '2024-2025', 2, 'A'), (15, 15, '2024-2025', 1, 'C'),
(16, 6, '2024-2025', 1, 'A'), (17, 7, '2024-2025', 2, 'B'),
(18, 8, '2024-2025', 1, 'A'), (19, 9, '2024-2025', 2, 'A'),
(20, 10, '2024-2025', 1, 'A');

-- ── Class Schedules
INSERT INTO class_schedule (class_id, day_of_week, start_time, end_time, room) VALUES
(1, 1, '08:00 AM', '09:30 AM', 'Room 101'),
(2, 2, '10:00 AM', '11:30 AM', 'Room 202'),
(3, 3, '01:00 PM', '02:30 PM', 'Room 303'),
(4, 4, '08:00 AM', '09:30 AM', 'Room 104'),
(5, 5, '10:00 AM', '11:30 AM', 'Room 205'),
(6, 1, '13:00', '14:30', 'Room 401'), (7, 2, '14:00', '15:30', 'Room 402'),
(8, 3, '09:00', '10:30', 'Room 403'), (9, 4, '11:00', '12:30', 'Room 404'),
(10, 5, '08:00', '09:30', 'Room 405'), (11, 1, '15:00', '16:30', 'Room 501'),
(12, 2, '16:00', '17:30', 'Room 502'), (13, 3, '10:00', '11:30', 'Room 503'),
(14, 4, '13:00', '14:30', 'Room 504'), (15, 5, '14:00', '15:30', 'Room 505'),
(16, 1, '09:00', '10:30', 'Room 601'), (17, 2, '11:00', '12:30', 'Room 602'),
(18, 3, '13:00', '14:30', 'Room 603'), (19, 4, '15:00', '16:30', 'Room 604'),
(20, 5, '16:00', '17:30', 'Room 605');

-- ── Enrollments
INSERT INTO enrollments (class_id, student_id) VALUES
(1, 1), (1, 2), (1, 3),    -- CS101-A: Doe, Smith, Johnson
(2, 2), (2, 4), (2, 5),    -- CS201-A: Smith, Williams, Brown
(3, 1), (3, 3),             -- CS301-A: Doe, Johnson
(4, 4), (4, 5),             -- MA101-B: Williams, Brown
(6, 6), (6, 7), (6, 8), (7, 9), (7, 10), (8, 11),
(8, 12), (9, 13), (9, 14), (10, 15), (10, 16), (11, 17);

-- ── Attendance Sessions 
INSERT INTO attendance_sessions (class_id, session_name, session_date, created_by_teacher_id) VALUES
(1, 'Week 1 Lecture',  '2025-01-06', 1),
(1, 'Week 2 Lecture',  '2025-01-13', 1),
(2, 'Week 1 Lab',      '2025-01-07', 2),
(3, NULL,               '2025-01-08', 1),
(4, 'Midterm Review',   '2025-01-09', 4),
(6, 'Intro', '2025-01-10', 6), (6, 'Lecture 2', '2025-01-17', 6),
(7, 'Lab 1', '2025-01-11', 7), (8, 'Discussion', '2025-01-12', 8),
(9, 'Theory 1', '2025-01-13', 9), (10, 'Review', '2025-01-14', 10),
(11, 'Workshop', '2025-01-15', 11), (12, 'Quiz 1', '2025-01-16', 12),
(13, 'Field Work', '2025-01-17', 13), (14, 'Project Prep', '2025-01-18', 14),
(15, 'Midterms', '2025-01-19', 15), (16, 'Final Review', '2025-01-20', 6),
(17, 'Guest Speaker', '2025-01-21', 7), (18, 'Lab 2', '2025-01-22', 8),
(19, 'Presentation', '2025-01-23', 9);

-- ── Attendance Records
INSERT INTO attendance_records (session_id, student_id, status_id, time_in) VALUES
(1, 1, 1, '2025-01-06 08:05:00'),  
(1, 2, 3, '2025-01-06 08:22:00'),   
(1, 3, 1, '2025-01-06 08:03:00'),   
(2, 1, 1, '2025-01-13 08:01:00'),   
(2, 2, 2, NULL),                  
(2, 3, 4, NULL),                     
(3, 2, 1, '2025-01-07 10:02:00'),  
(3, 4, 1, '2025-01-07 10:00:00'),  
(3, 5, 3, '2025-01-07 10:18:00'), 
(4, 1, 1, '2025-01-08 13:00:00'),  
(4, 3, 2, NULL),                  
(5, 4, 1, '2025-01-09 08:02:00'), 
(5, 5, 1, '2025-01-09 08:00:00'),  
(6, 6, 1, '2025-01-10 13:00:00'), 
(6, 7, 1, '2025-01-10 13:05:00'),
(6, 8, 2, NULL), 
(7, 9, 3, '2025-01-11 14:20:00'),
(7, 10, 1, '2025-01-11 14:00:00'), 
(8, 11, 4, NULL),
(8, 12, 1, '2025-01-12 09:00:00'), 
(9, 13, 1, '2025-01-13 11:00:00'),
(9, 14, 2, NULL), 
(10, 15, 3, '2025-01-14 08:15:00'),
(10, 16, 1, '2025-01-14 08:00:00'), 
(11, 17, 1, '2025-01-15 15:00:00'),
(12, 6, 1, '2025-01-17 13:00:00'), 
(12, 7, 3, '2025-01-17 13:10:00'),
(12, 8, 1, '2025-01-17 13:00:00');

-- ── Remarks (20 Total) ──────────────────────────────────────────────
INSERT INTO remarks (session_id, student_id, teacher_id, category_id, remark_text) VALUES
(1, 2, 1, 1, 'Arrived 22 minutes late due to traffic.'),
(2, 2, 1, 4, 'Did not attend, no notice given.'),
(2, 3, 1, 2, 'Excused — submitted medical certificate.'),
(3, 5, 2, 1, 'Arrived 18 minutes late, warned about punctuality.'),
(4, 3, 1, 3, 'Missed class, needs to submit makeup assignment.'),
(6, 8, 6, 4, 'Absent with no prior notice.'),
(7, 9, 7, 1, 'Late due to bad weather.'),
(8, 11, 8, 2, 'Excused for school competition.'),
(9, 14, 9, 4, 'Missed discussion, please catch up.'),
(10, 15, 10, 1, 'Tardy by 15 mins.'),
(6, 7, 6, 1, 'Stuck in traffic.'),
(6, 6, 6, 3, 'Excellent participation today.'),
(7, 10, 7, 3, 'Distracted during lab.'),
(8, 12, 8, 3, 'Helped peers with the assignment.'),
(9, 13, 9, 4, 'Submitted homework early.'),
(10, 16, 10, 3, 'Very attentive during review.'),
(11, 17, 11, 4, 'Needs to bring materials next time.'),
(12, 8, 6, 3, 'Improved focus this session.'),
(12, 6, 6, 4, 'Forgot textbook.'),
(12, 7, 6, 3, 'Participated actively despite being late.');

-- ── Users ───────────────────────────────────────────────
--   Passwords in comments for reference:
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
--     2025-0006 on-wards / student123

INSERT INTO users (username, password_hash, role, teacher_id, student_id) VALUES
-- Admin
('admin','$2a$11$tSPq2Qb6irgXpKTYKFR9o.F0qdqhtZC.OtqWqRKDKF67v1/aaSdWm','Admin', NULL, NULL),

-- Registrar
('registrar','$2a$11$0is1tutPQtk291qm212F8eqfqn8D.I7UE6Yyb6OFevrhQ6Dhl5K26','Registrar', NULL, NULL),

-- Teachers
('t.santos','$2a$11$FGfBxKS4.8XtM0EFcwyhIeLuZt6IFkBc6gm9AscRUf24ZU6DDPVDa','Teacher', 1, NULL),
('t.cruz','$2a$11$b1Q.H.xGI5ejHdtZ/zzUFO2Teh/VjO7x6HMSI8xxdBAVeOXrnIhiW','Teacher', 2, NULL),
('t.reyes','$2a$11$NgT5rlo1yZy4kSSK2lpV1.iRzfcsr9wn2OtDmYlPyT75wHaVkIFU2','Teacher', 3, NULL),
('t.garcia','$2a$11$DXS0HfhHDSLSEMNi9ur3.OfclhAJOW/xNrv6u/h1jIOLl/J/sfMK2','Teacher', 4, NULL),
('t.flores','$2a$11$VxlUwGC1zOiUM4R5iDYr/epovWRj9a/BBU8DqYLC5l844mnJ7Vqc.','Teacher', 5, NULL),

-- Students 
('2025-0001','$2a$11$TD6tMsMJlABi8GL/hEobROehcw7RdsHgoz3gciGKdSLSoYLH5adwm','Student', NULL, 1),
('2025-0002','$2a$11$TIeqjv/Geolk42y4s4F6derXewJYoBoKZoeNHzRdF3pJqP73ybGh6','Student', NULL, 2),
('2025-0003','$2a$11$w/TpvYE7mdR6oONhMtlGQ.ZohZ3ELDTmIbJ5Rzz5t0wzXZH0C8W.e','Student', NULL, 3),
('2025-0004','$2a$11$0ab0Aavr2xyC.DnKFEcjCujZSPupZKSkw7.Xjs2oF20MJ7OpG4vHS','Student', NULL, 4),
('2025-0005','$2a$11$JaBJTn3//JxoKM1TwhguxOefn/ulrLnpmF0p/lQb2s/A0Xvz8HvOK','Student', NULL, 5),
('2025-0006','$2a$10$cduh7qao2M1ectm9vRSSA.eAi9cLbeZvK.9LhorQBiWLDqnRVuusy', 'Student', NULL, 6),
('2025-0007','$2a$10$cduh7qao2M1ectm9vRSSA.eAi9cLbeZvK.9LhorQBiWLDqnRVuusy', 'Student', NULL, 7),
('2025-0008','$2a$10$cduh7qao2M1ectm9vRSSA.eAi9cLbeZvK.9LhorQBiWLDqnRVuusy', 'Student', NULL, 8),
('2025-0009','$2a$10$cduh7qao2M1ectm9vRSSA.eAi9cLbeZvK.9LhorQBiWLDqnRVuusy', 'Student', NULL, 9),
('2025-0010','$2a$10$cduh7qao2M1ectm9vRSSA.eAi9cLbeZvK.9LhorQBiWLDqnRVuusy', 'Student', NULL, 10),
('2025-0011','$2a$10$cduh7qao2M1ectm9vRSSA.eAi9cLbeZvK.9LhorQBiWLDqnRVuusy', 'Student', NULL, 11),
('2025-0012','$2a$10$cduh7qao2M1ectm9vRSSA.eAi9cLbeZvK.9LhorQBiWLDqnRVuusy', 'Student', NULL, 12),
('2025-0013','$2a$10$cduh7qao2M1ectm9vRSSA.eAi9cLbeZvK.9LhorQBiWLDqnRVuusy', 'Student', NULL, 13);

SELECT * FROM students;
SELECT * FROM teachers;
SELECT * FROM subjects;
SELECT * FROM classes;
SELECT * FROM class_schedule;
SELECT * FROM enrollments;
SELECT * FROM attendance_sessions;
SELECT * FROM attendance_status;
SELECT * FROM attendance_records;
SELECT * FROM remark_categories;
SELECT * FROM remarks;
SELECT * FROM users;