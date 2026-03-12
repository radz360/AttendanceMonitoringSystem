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
-- Table structure for table `remarks`
--

DROP TABLE IF EXISTS `remarks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `remarks` (
  `remark_id` int NOT NULL AUTO_INCREMENT,
  `session_id` int NOT NULL,
  `student_id` int NOT NULL,
  `teacher_id` int NOT NULL,
  `category_id` int DEFAULT NULL,
  `remark_text` varchar(255) NOT NULL,
  `remark_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`remark_id`),
  KEY `session_id` (`session_id`,`student_id`),
  KEY `teacher_id` (`teacher_id`),
  KEY `category_id` (`category_id`),
  CONSTRAINT `remarks_ibfk_1` FOREIGN KEY (`session_id`, `student_id`) REFERENCES `attendance_records` (`session_id`, `student_id`),
  CONSTRAINT `remarks_ibfk_2` FOREIGN KEY (`teacher_id`) REFERENCES `teachers` (`teacher_id`),
  CONSTRAINT `remarks_ibfk_3` FOREIGN KEY (`category_id`) REFERENCES `remark_categories` (`category_id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `remarks`
--

LOCK TABLES `remarks` WRITE;
/*!40000 ALTER TABLE `remarks` DISABLE KEYS */;
INSERT INTO `remarks` VALUES (1,1,2,1,1,'Arrived 22 minutes late due to traffic.','2026-03-12 08:10:53'),(2,2,2,1,4,'Did not attend, no notice given.','2026-03-12 08:10:53'),(3,2,3,1,2,'Excused — submitted medical certificate.','2026-03-12 08:10:53'),(4,3,5,2,1,'Arrived 18 minutes late, warned about punctuality.','2026-03-12 08:10:53'),(5,4,3,1,3,'Missed class, needs to submit makeup assignment.','2026-03-12 08:10:53'),(6,6,8,6,4,'Absent with no prior notice.','2026-03-12 08:10:53'),(7,7,9,7,1,'Late due to bad weather.','2026-03-12 08:10:53'),(8,8,11,8,2,'Excused for school competition.','2026-03-12 08:10:53'),(9,9,14,9,4,'Missed discussion, please catch up.','2026-03-12 08:10:53'),(10,10,15,10,1,'Tardy by 15 mins.','2026-03-12 08:10:53'),(11,6,7,6,1,'Stuck in traffic.','2026-03-12 08:10:53'),(12,6,6,6,3,'Excellent participation today.','2026-03-12 08:10:53'),(13,7,10,7,3,'Distracted during lab.','2026-03-12 08:10:53'),(14,8,12,8,3,'Helped peers with the assignment.','2026-03-12 08:10:53'),(15,9,13,9,4,'Submitted homework early.','2026-03-12 08:10:53'),(16,10,16,10,3,'Very attentive during review.','2026-03-12 08:10:53'),(17,11,17,11,4,'Needs to bring materials next time.','2026-03-12 08:10:53'),(18,12,8,6,3,'Improved focus this session.','2026-03-12 08:10:53'),(19,12,6,6,4,'Forgot textbook.','2026-03-12 08:10:53'),(20,12,7,6,3,'Participated actively despite being late.','2026-03-12 08:10:53');
/*!40000 ALTER TABLE `remarks` ENABLE KEYS */;
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
