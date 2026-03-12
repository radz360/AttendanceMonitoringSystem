use attendance_db;

-- What it does: it connects the teachers, classes, and subjects tables to show the actual names 
-- of the teachers alongside the subjects they are assigned to.
SELECT 
    t.first_name, 
    t.last_name, 
    s.subject_name
FROM teachers t
JOIN classes c ON t.teacher_id = c.teacher_id
JOIN subjects s ON c.subject_id = s.subject_id;

-- What it does: checks the enrollments table and counts how many students are enrolled in each specific subject.
SELECT 
    sub.subject_name, 
    COUNT(e.student_id) AS total_students
FROM classes c
JOIN subjects sub ON c.subject_id = sub.subject_id
JOIN enrollments e ON c.class_id = e.class_id
GROUP BY sub.subject_name;

-- What it does: It searches the attendance records to find the names of any students who have received an Absent status.
SELECT DISTINCT 
    s.first_name, 
    s.last_name, 
    ast.status_name
FROM students s
JOIN attendance_records ar ON s.student_id = ar.student_id
JOIN attendance_status ast ON ar.status_id = ast.status_id
WHERE ast.status_name = 'Absent';

-- What it does: It counts how many classes each teacher has and only shows the ones who are teaching more than 1 class.
SELECT 
    t.first_name, 
    t.last_name, 
    COUNT(c.class_id) AS classes_taught
FROM teachers t
JOIN classes c ON t.teacher_id = c.teacher_id
GROUP BY t.first_name, t.last_name
HAVING COUNT(c.class_id) > 1;

-- What it does: It counts the total number of remarks created by each teacher and sorts them from highest to lowest.
SELECT 
    t.first_name, 
    t.last_name, 
    COUNT(r.remark_id) AS total_remarks
FROM teachers t
JOIN remarks r ON t.teacher_id = r.teacher_id
GROUP BY t.first_name, t.last_name
ORDER BY total_remarks DESC;