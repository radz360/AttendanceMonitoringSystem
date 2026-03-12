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
-- Table structure for table `attendance_sessions`
--

DROP TABLE IF EXISTS `attendance_sessions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `attendance_sessions` (
  `session_id` int NOT NULL AUTO_INCREMENT,
  `class_id` int NOT NULL,
  `session_name` varchar(100) DEFAULT NULL,
  `session_date` date NOT NULL,
  `created_by_teacher_id` int NOT NULL,
  PRIMARY KEY (`session_id`),
  UNIQUE KEY `class_id` (`class_id`,`session_date`),
  KEY `created_by_teacher_id` (`created_by_teacher_id`),
  CONSTRAINT `attendance_sessions_ibfk_1` FOREIGN KEY (`class_id`) REFERENCES `classes` (`class_id`),
  CONSTRAINT `attendance_sessions_ibfk_2` FOREIGN KEY (`created_by_teacher_id`) REFERENCES `teachers` (`teacher_id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `attendance_sessions`
--

LOCK TABLES `attendance_sessions` WRITE;
/*!40000 ALTER TABLE `attendance_sessions` DISABLE KEYS */;
INSERT INTO `attendance_sessions` VALUES (1,1,'Week 1 Lecture','2025-01-06',1),(2,1,'Week 2 Lecture','2025-01-13',1),(3,2,'Week 1 Lab','2025-01-07',2),(4,3,NULL,'2025-01-08',1),(5,4,'Midterm Review','2025-01-09',4),(6,6,'Intro','2025-01-10',6),(7,6,'Lecture 2','2025-01-17',6),(8,7,'Lab 1','2025-01-11',7),(9,8,'Discussion','2025-01-12',8),(10,9,'Theory 1','2025-01-13',9),(11,10,'Review','2025-01-14',10),(12,11,'Workshop','2025-01-15',11),(13,12,'Quiz 1','2025-01-16',12),(14,13,'Field Work','2025-01-17',13),(15,14,'Project Prep','2025-01-18',14),(16,15,'Midterms','2025-01-19',15),(17,16,'Final Review','2025-01-20',6),(18,17,'Guest Speaker','2025-01-21',7),(19,18,'Lab 2','2025-01-22',8),(20,19,'Presentation','2025-01-23',9);
/*!40000 ALTER TABLE `attendance_sessions` ENABLE KEYS */;
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
