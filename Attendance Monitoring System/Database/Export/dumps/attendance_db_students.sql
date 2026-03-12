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
-- Table structure for table `students`
--

DROP TABLE IF EXISTS `students`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `students` (
  `student_id` int NOT NULL AUTO_INCREMENT,
  `registration_no` varchar(20) NOT NULL,
  `first_name` varchar(50) NOT NULL,
  `last_name` varchar(50) NOT NULL,
  `gender` enum('Male','Female','Other') NOT NULL,
  `date_of_birth` date NOT NULL,
  PRIMARY KEY (`student_id`),
  UNIQUE KEY `registration_no` (`registration_no`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `students`
--

LOCK TABLES `students` WRITE;
/*!40000 ALTER TABLE `students` DISABLE KEYS */;
INSERT INTO `students` VALUES (1,'2025-0001','Jane','Doe','Female','2003-01-15'),(2,'2025-0002','John','Smith','Male','2004-03-22'),(3,'2025-0003','Emily','Johnson','Female','2003-06-10'),(4,'2025-0004','Michael','Williams','Male','2004-09-05'),(5,'2025-0005','Sarah','Brown','Female','2003-12-18'),(6,'2025-0006','David','Miller','Male','2004-02-11'),(7,'2025-0007','Sophia','Wilson','Female','2003-08-25'),(8,'2025-0008','James','Moore','Male','2004-11-03'),(9,'2025-0009','Olivia','Taylor','Female','2003-04-14'),(10,'2025-0010','Daniel','Anderson','Male','2004-07-08'),(11,'2025-0011','Emma','Thomas','Female','2003-09-30'),(12,'2025-0012','Matthew','Jackson','Male','2004-01-19'),(13,'2025-0013','Isabella','White','Female','2003-05-22'),(14,'2025-0014','Joseph','Harris','Male','2004-10-12'),(15,'2025-0015','Mia','Martin','Female','2003-03-05'),(16,'2025-0016','Lucas','Thompson','Male','2004-08-17'),(17,'2025-0017','Charlotte','Garcia','Female','2003-11-28'),(18,'2025-0018','Ethan','Martinez','Male','2004-06-09'),(19,'2025-0019','Amelia','Robinson','Female','2003-02-14'),(20,'2025-0020','Alexander','Clark','Male','2004-12-01');
/*!40000 ALTER TABLE `students` ENABLE KEYS */;
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
