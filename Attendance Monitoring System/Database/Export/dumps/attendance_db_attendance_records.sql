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
-- Table structure for table `attendance_records`
--

DROP TABLE IF EXISTS `attendance_records`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `attendance_records` (
  `attendance_record_id` int NOT NULL AUTO_INCREMENT,
  `session_id` int NOT NULL,
  `student_id` int NOT NULL,
  `status_id` int NOT NULL,
  `time_in` datetime DEFAULT NULL,
  `marked_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`attendance_record_id`),
  UNIQUE KEY `session_id` (`session_id`,`student_id`),
  KEY `student_id` (`student_id`),
  KEY `status_id` (`status_id`),
  CONSTRAINT `attendance_records_ibfk_1` FOREIGN KEY (`session_id`) REFERENCES `attendance_sessions` (`session_id`),
  CONSTRAINT `attendance_records_ibfk_2` FOREIGN KEY (`student_id`) REFERENCES `students` (`student_id`),
  CONSTRAINT `attendance_records_ibfk_3` FOREIGN KEY (`status_id`) REFERENCES `attendance_status` (`status_id`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `attendance_records`
--

LOCK TABLES `attendance_records` WRITE;
/*!40000 ALTER TABLE `attendance_records` DISABLE KEYS */;
INSERT INTO `attendance_records` VALUES (1,1,1,1,'2025-01-06 08:05:00','2026-03-12 08:10:53'),(2,1,2,3,'2025-01-06 08:22:00','2026-03-12 08:10:53'),(3,1,3,1,'2025-01-06 08:03:00','2026-03-12 08:10:53'),(4,2,1,1,'2025-01-13 08:01:00','2026-03-12 08:10:53'),(5,2,2,2,NULL,'2026-03-12 08:10:53'),(6,2,3,4,NULL,'2026-03-12 08:10:53'),(7,3,2,1,'2025-01-07 10:02:00','2026-03-12 08:10:53'),(8,3,4,1,'2025-01-07 10:00:00','2026-03-12 08:10:53'),(9,3,5,3,'2025-01-07 10:18:00','2026-03-12 08:10:53'),(10,4,1,1,'2025-01-08 13:00:00','2026-03-12 08:10:53'),(11,4,3,2,NULL,'2026-03-12 08:10:53'),(12,5,4,1,'2025-01-09 08:02:00','2026-03-12 08:10:53'),(13,5,5,1,'2025-01-09 08:00:00','2026-03-12 08:10:53'),(14,6,6,1,'2025-01-10 13:00:00','2026-03-12 08:10:53'),(15,6,7,1,'2025-01-10 13:05:00','2026-03-12 08:10:53'),(16,6,8,2,NULL,'2026-03-12 08:10:53'),(17,7,9,3,'2025-01-11 14:20:00','2026-03-12 08:10:53'),(18,7,10,1,'2025-01-11 14:00:00','2026-03-12 08:10:53'),(19,8,11,4,NULL,'2026-03-12 08:10:53'),(20,8,12,1,'2025-01-12 09:00:00','2026-03-12 08:10:53'),(21,9,13,1,'2025-01-13 11:00:00','2026-03-12 08:10:53'),(22,9,14,2,NULL,'2026-03-12 08:10:53'),(23,10,15,3,'2025-01-14 08:15:00','2026-03-12 08:10:53'),(24,10,16,1,'2025-01-14 08:00:00','2026-03-12 08:10:53'),(25,11,17,1,'2025-01-15 15:00:00','2026-03-12 08:10:53'),(26,12,6,1,'2025-01-17 13:00:00','2026-03-12 08:10:53'),(27,12,7,3,'2025-01-17 13:10:00','2026-03-12 08:10:53'),(28,12,8,1,'2025-01-17 13:00:00','2026-03-12 08:10:53');
/*!40000 ALTER TABLE `attendance_records` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-03-12  9:13:14
