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
-- Table structure for table `class_schedule`
--

DROP TABLE IF EXISTS `class_schedule`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `class_schedule` (
  `schedule_id` int NOT NULL AUTO_INCREMENT,
  `class_id` int NOT NULL,
  `day_of_week` int NOT NULL,
  `start_time` varchar(20) NOT NULL,
  `end_time` varchar(20) NOT NULL,
  `room` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`schedule_id`),
  UNIQUE KEY `class_id` (`class_id`,`day_of_week`,`start_time`,`end_time`),
  CONSTRAINT `class_schedule_ibfk_1` FOREIGN KEY (`class_id`) REFERENCES `classes` (`class_id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `class_schedule`
--

LOCK TABLES `class_schedule` WRITE;
/*!40000 ALTER TABLE `class_schedule` DISABLE KEYS */;
INSERT INTO `class_schedule` VALUES (1,1,1,'08:00 AM','09:30 AM','Room 101'),(2,2,2,'10:00 AM','11:30 AM','Room 202'),(3,3,3,'01:00 PM','02:30 PM','Room 303'),(4,4,4,'08:00 AM','09:30 AM','Room 104'),(5,5,5,'10:00 AM','11:30 AM','Room 205'),(6,6,1,'13:00','14:30','Room 401'),(7,7,2,'14:00','15:30','Room 402'),(8,8,3,'09:00','10:30','Room 403'),(9,9,4,'11:00','12:30','Room 404'),(10,10,5,'08:00','09:30','Room 405'),(11,11,1,'15:00','16:30','Room 501'),(12,12,2,'16:00','17:30','Room 502'),(13,13,3,'10:00','11:30','Room 503'),(14,14,4,'13:00','14:30','Room 504'),(15,15,5,'14:00','15:30','Room 505'),(16,16,1,'09:00','10:30','Room 601'),(17,17,2,'11:00','12:30','Room 602'),(18,18,3,'13:00','14:30','Room 603'),(19,19,4,'15:00','16:30','Room 604'),(20,20,5,'16:00','17:30','Room 605');
/*!40000 ALTER TABLE `class_schedule` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-03-12  9:13:13
