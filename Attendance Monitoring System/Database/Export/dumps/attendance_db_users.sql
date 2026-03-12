-- MySQL dump 10.13  Distrib 8.0.45, for Win64 (x86_64)
--
-- Host: localhost    Database: attendance_db
-- ------------------------------------------------------
-- Server version	8.0.45

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `role` enum('Admin','Registrar','Teacher','Student') NOT NULL,
  `teacher_id` int DEFAULT NULL,
  `student_id` int DEFAULT NULL,
  `is_active` int NOT NULL DEFAULT '1',
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `username` (`username`),
  KEY `teacher_id` (`teacher_id`),
  KEY `fk_users_student` (`student_id`),
  CONSTRAINT `fk_users_student` FOREIGN KEY (`student_id`) REFERENCES `students` (`student_id`),
  CONSTRAINT `users_ibfk_1` FOREIGN KEY (`teacher_id`) REFERENCES `teachers` (`teacher_id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'admin','$2a$11$tSPq2Qb6irgXpKTYKFR9o.F0qdqhtZC.OtqWqRKDKF67v1/aaSdWm','Admin',NULL,NULL,1,'2026-03-12 08:10:53'),(2,'registrar','$2a$11$0is1tutPQtk291qm212F8eqfqn8D.I7UE6Yyb6OFevrhQ6Dhl5K26','Registrar',NULL,NULL,1,'2026-03-12 08:10:53'),(3,'t.santos','$2a$11$FGfBxKS4.8XtM0EFcwyhIeLuZt6IFkBc6gm9AscRUf24ZU6DDPVDa','Teacher',1,NULL,1,'2026-03-12 08:10:53'),(4,'t.cruz','$2a$11$b1Q.H.xGI5ejHdtZ/zzUFO2Teh/VjO7x6HMSI8xxdBAVeOXrnIhiW','Teacher',2,NULL,1,'2026-03-12 08:10:53'),(5,'t.reyes','$2a$11$NgT5rlo1yZy4kSSK2lpV1.iRzfcsr9wn2OtDmYlPyT75wHaVkIFU2','Teacher',3,NULL,1,'2026-03-12 08:10:53'),(6,'t.garcia','$2a$11$DXS0HfhHDSLSEMNi9ur3.OfclhAJOW/xNrv6u/h1jIOLl/J/sfMK2','Teacher',4,NULL,1,'2026-03-12 08:10:53'),(7,'t.flores','$2a$11$VxlUwGC1zOiUM4R5iDYr/epovWRj9a/BBU8DqYLC5l844mnJ7Vqc.','Teacher',5,NULL,1,'2026-03-12 08:10:53'),(8,'2025-0001','$2a$11$TD6tMsMJlABi8GL/hEobROehcw7RdsHgoz3gciGKdSLSoYLH5adwm','Student',NULL,1,1,'2026-03-12 08:10:53'),(9,'2025-0002','$2a$11$TIeqjv/Geolk42y4s4F6derXewJYoBoKZoeNHzRdF3pJqP73ybGh6','Student',NULL,2,1,'2026-03-12 08:10:53'),(10,'2025-0003','$2a$11$w/TpvYE7mdR6oONhMtlGQ.ZohZ3ELDTmIbJ5Rzz5t0wzXZH0C8W.e','Student',NULL,3,1,'2026-03-12 08:10:53'),(11,'2025-0004','$2a$11$0ab0Aavr2xyC.DnKFEcjCujZSPupZKSkw7.Xjs2oF20MJ7OpG4vHS','Student',NULL,4,1,'2026-03-12 08:10:53'),(12,'2025-0005','$2a$11$JaBJTn3//JxoKM1TwhguxOefn/ulrLnpmF0p/lQb2s/A0Xvz8HvOK','Student',NULL,5,1,'2026-03-12 08:10:53'),(13,'2025-0006','$2a$10$cduh7qao2M1ectm9vRSSA.eAi9cLbeZvK.9LhorQBiWLDqnRVuusy','Student',NULL,6,1,'2026-03-12 08:10:53'),(14,'2025-0007','$2a$10$cduh7qao2M1ectm9vRSSA.eAi9cLbeZvK.9LhorQBiWLDqnRVuusy','Student',NULL,7,1,'2026-03-12 08:10:53'),(15,'2025-0008','$2a$10$cduh7qao2M1ectm9vRSSA.eAi9cLbeZvK.9LhorQBiWLDqnRVuusy','Student',NULL,8,1,'2026-03-12 08:10:53'),(16,'2025-0009','$2a$10$cduh7qao2M1ectm9vRSSA.eAi9cLbeZvK.9LhorQBiWLDqnRVuusy','Student',NULL,9,1,'2026-03-12 08:10:53'),(17,'2025-0010','$2a$10$cduh7qao2M1ectm9vRSSA.eAi9cLbeZvK.9LhorQBiWLDqnRVuusy','Student',NULL,10,1,'2026-03-12 08:10:53'),(18,'2025-0011','$2a$10$cduh7qao2M1ectm9vRSSA.eAi9cLbeZvK.9LhorQBiWLDqnRVuusy','Student',NULL,11,1,'2026-03-12 08:10:53'),(19,'2025-0012','$2a$10$cduh7qao2M1ectm9vRSSA.eAi9cLbeZvK.9LhorQBiWLDqnRVuusy','Student',NULL,12,1,'2026-03-12 08:10:53'),(20,'2025-0013','$2a$10$cduh7qao2M1ectm9vRSSA.eAi9cLbeZvK.9LhorQBiWLDqnRVuusy','Student',NULL,13,1,'2026-03-12 08:10:53');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-03-12  9:13:15
