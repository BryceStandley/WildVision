CREATE DATABASE  IF NOT EXISTS `it01-animals` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `it01-animals`;
-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: 10.51.33.50    Database: it01-animals
-- ------------------------------------------------------
-- Server version	9.0.1

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
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `accesstype`
--

DROP TABLE IF EXISTS `accesstype`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `accesstype` (
  `AccessType_ID` binary(16) NOT NULL,
  `AccessType_Details` varchar(45) NOT NULL,
  PRIMARY KEY (`AccessType_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `animal`
--

DROP TABLE IF EXISTS `animal`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `animal` (
  `Animal_ID` binary(16) NOT NULL,
  `Animal_Name` varchar(45) NOT NULL,
  `Animal_DOB` date NOT NULL,
  `Animal_Type` varchar(45) NOT NULL,
  `Video_Path` varchar(255) DEFAULT NULL,
  `Video_Thumbnail_Path` varchar(255) DEFAULT NULL,
  `Video_Upload_Date` date DEFAULT NULL,
  `Thumbnail_Data` longblob,
  `Video_Data` longblob,
  `Video_File_Name` varchar(255) NOT NULL DEFAULT '',
  PRIMARY KEY (`Animal_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `animalaccess`
--

DROP TABLE IF EXISTS `animalaccess`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `animalaccess` (
  `Access_ID` binary(16) NOT NULL,
  `Access_Type` varchar(25) NOT NULL,
  `Assigned_Date` date NOT NULL,
  `Animal_ID` binary(16) NOT NULL,
  `User_ID` binary(16) NOT NULL,
  PRIMARY KEY (`Access_ID`),
  KEY `IX_animalaccess_Animal_ID` (`Animal_ID`),
  KEY `IX_animalaccess_User_ID` (`User_ID`),
  CONSTRAINT `FK_animalaccess_animal_Animal_ID` FOREIGN KEY (`Animal_ID`) REFERENCES `animal` (`Animal_ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_animalaccess_user_User_ID` FOREIGN KEY (`User_ID`) REFERENCES `user` (`User_ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `billing`
--

DROP TABLE IF EXISTS `billing`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `billing` (
  `Billing_ID` binary(16) NOT NULL,
  `GPC_ID` binary(16) NOT NULL,
  `Job_ID` binary(16) NOT NULL,
  `User_ID` binary(16) NOT NULL,
  `Subscription_ID` binary(16) NOT NULL,
  PRIMARY KEY (`Billing_ID`),
  KEY `IX_billing_GPC_ID` (`GPC_ID`),
  KEY `IX_billing_Job_ID` (`Job_ID`),
  KEY `IX_billing_Subscription_ID` (`Subscription_ID`),
  KEY `IX_billing_User_ID` (`User_ID`),
  CONSTRAINT `FK_billing_graphic_GPC_ID` FOREIGN KEY (`GPC_ID`) REFERENCES `graphic` (`GPC_ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_billing_jobscompleted_Job_ID` FOREIGN KEY (`Job_ID`) REFERENCES `jobscompleted` (`Job_ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_billing_subscription_Subscription_ID` FOREIGN KEY (`Subscription_ID`) REFERENCES `subscription` (`Subscription_ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_billing_user_User_ID` FOREIGN KEY (`User_ID`) REFERENCES `user` (`User_ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `graphic`
--

DROP TABLE IF EXISTS `graphic`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `graphic` (
  `GPC_ID` binary(16) NOT NULL,
  `GPC_Name` varchar(255) NOT NULL,
  `GPC_Date_Upload` date NOT NULL,
  `File_Path` varchar(255) NOT NULL,
  `Animal_ID` binary(16) NOT NULL,
  `GPC_Size` int NOT NULL,
  PRIMARY KEY (`GPC_ID`),
  KEY `IX_graphic_Animal_ID` (`Animal_ID`),
  CONSTRAINT `FK_graphic_animal_Animal_ID` FOREIGN KEY (`Animal_ID`) REFERENCES `animal` (`Animal_ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `jobdetails`
--

DROP TABLE IF EXISTS `jobdetails`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `jobdetails` (
  `JD_ID` binary(16) NOT NULL,
  `GPC_ID` binary(16) NOT NULL,
  `Model_ID` binary(16) NOT NULL,
  `Model_Gen_Type` varchar(45) NOT NULL,
  PRIMARY KEY (`JD_ID`),
  KEY `IX_jobdetails_GPC_ID` (`GPC_ID`),
  KEY `IX_jobdetails_Model_ID` (`Model_ID`),
  CONSTRAINT `FK_jobdetails_graphic_GPC_ID` FOREIGN KEY (`GPC_ID`) REFERENCES `graphic` (`GPC_ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_jobdetails_model3d_Model_ID` FOREIGN KEY (`Model_ID`) REFERENCES `model3d` (`Model_ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `jobscompleted`
--

DROP TABLE IF EXISTS `jobscompleted`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `jobscompleted` (
  `Job_ID` binary(16) NOT NULL,
  `Job_Type` varchar(45) NOT NULL,
  `Jobs_Start` date NOT NULL,
  `Jobs_End` date NOT NULL,
  `Job_Size` int NOT NULL,
  `JD_ID` binary(16) NOT NULL,
  PRIMARY KEY (`Job_ID`),
  KEY `IX_jobscompleted_JD_ID` (`JD_ID`),
  CONSTRAINT `FK_jobscompleted_jobdetails_JD_ID` FOREIGN KEY (`JD_ID`) REFERENCES `jobdetails` (`JD_ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `jobspending`
--

DROP TABLE IF EXISTS `jobspending`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `jobspending` (
  `Queue_Number` int NOT NULL AUTO_INCREMENT,
  `Job_Added` date NOT NULL,
  `Status` char(35) NOT NULL,
  `JD_ID` binary(16) NOT NULL,
  `JDID` binary(16) NOT NULL,
  PRIMARY KEY (`Queue_Number`),
  KEY `IX_jobspending_JDID` (`JDID`),
  CONSTRAINT `FK_jobspending_jobdetails_JDID` FOREIGN KEY (`JDID`) REFERENCES `jobdetails` (`JD_ID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `model3d`
--

DROP TABLE IF EXISTS `model3d`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `model3d` (
  `Model_ID` binary(16) NOT NULL,
  `Model_Title` varchar(255) NOT NULL,
  `Model_Date_Gen` date NOT NULL,
  `File_Path` varchar(255) NOT NULL,
  `GPC_ID` binary(16) NOT NULL,
  PRIMARY KEY (`Model_ID`),
  KEY `IX_model3d_GPC_ID` (`GPC_ID`),
  CONSTRAINT `FK_model3d_graphic_GPC_ID` FOREIGN KEY (`GPC_ID`) REFERENCES `graphic` (`GPC_ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `org_requests`
--

DROP TABLE IF EXISTS `org_requests`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `org_requests` (
  `Request_ID` binary(16) NOT NULL,
  `Org_ID` binary(16) NOT NULL,
  `User_ID` binary(16) NOT NULL,
  `Date_Requested` date NOT NULL,
  `Date_Processed` date DEFAULT NULL,
  `Status` varchar(45) NOT NULL,
  PRIMARY KEY (`Request_ID`),
  KEY `IX_org_requests_Org_ID` (`Org_ID`),
  KEY `IX_org_requests_User_ID` (`User_ID`),
  CONSTRAINT `FK_org_requests_organisation_Org_ID` FOREIGN KEY (`Org_ID`) REFERENCES `organisation` (`Org_ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_org_requests_user_User_ID` FOREIGN KEY (`User_ID`) REFERENCES `user` (`User_ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `organisation`
--

DROP TABLE IF EXISTS `organisation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `organisation` (
  `Org_ID` binary(16) NOT NULL,
  `Org_Name` varchar(45) NOT NULL,
  `Org_Email` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Org_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `organisationaccess`
--

DROP TABLE IF EXISTS `organisationaccess`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `organisationaccess` (
  `OrgAccess_ID` binary(16) NOT NULL,
  `Access_Type` varchar(45) NOT NULL,
  `Assigned_Date` date NOT NULL,
  `Org_ID` binary(16) NOT NULL,
  `Access_ID` binary(16) NOT NULL,
  PRIMARY KEY (`OrgAccess_ID`),
  KEY `IX_organisationaccess_Access_ID` (`Access_ID`),
  KEY `IX_organisationaccess_Org_ID` (`Org_ID`),
  CONSTRAINT `FK_organisationaccess_animalaccess_Access_ID` FOREIGN KEY (`Access_ID`) REFERENCES `animalaccess` (`Access_ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_organisationaccess_organisation_Org_ID` FOREIGN KEY (`Org_ID`) REFERENCES `organisation` (`Org_ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `subscription`
--

DROP TABLE IF EXISTS `subscription`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subscription` (
  `Subscription_ID` binary(16) NOT NULL,
  `Subscription_Title` varchar(45) NOT NULL,
  `Storage_Size` int NOT NULL,
  `Charge_Rate` int NOT NULL,
  PRIMARY KEY (`Subscription_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `transaction`
--

DROP TABLE IF EXISTS `transaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transaction` (
  `Transaction_ID` binary(16) NOT NULL,
  `TransType_ID` binary(16) NOT NULL,
  `User_ID` binary(16) NOT NULL,
  `Animal_ID` binary(16) NOT NULL,
  PRIMARY KEY (`Transaction_ID`),
  KEY `IX_transaction_Animal_ID` (`Animal_ID`),
  KEY `IX_transaction_TransType_ID` (`TransType_ID`),
  KEY `IX_transaction_User_ID` (`User_ID`),
  CONSTRAINT `FK_transaction_animal_Animal_ID` FOREIGN KEY (`Animal_ID`) REFERENCES `animal` (`Animal_ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_transaction_transactiontype_TransType_ID` FOREIGN KEY (`TransType_ID`) REFERENCES `transactiontype` (`TransType_ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_transaction_user_User_ID` FOREIGN KEY (`User_ID`) REFERENCES `user` (`User_ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `transactiontype`
--

DROP TABLE IF EXISTS `transactiontype`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transactiontype` (
  `TransType_ID` binary(16) NOT NULL,
  `Trans_Details` varchar(255) NOT NULL,
  PRIMARY KEY (`TransType_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `User_ID` binary(16) NOT NULL,
  `Permission_Level` char(10) NOT NULL,
  `User_Name` varchar(255) NOT NULL,
  `User_Email` varchar(255) NOT NULL,
  `User_Password` varchar(12) NOT NULL,
  `User_Date_Join` date NOT NULL,
  `subscription_ID` binary(16) NOT NULL,
  PRIMARY KEY (`User_ID`),
  KEY `IX_user_subscription_ID` (`subscription_ID`),
  CONSTRAINT `FK_user_subscription_subscription_ID` FOREIGN KEY (`subscription_ID`) REFERENCES `subscription` (`Subscription_ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `useraccess`
--

DROP TABLE IF EXISTS `useraccess`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `useraccess` (
  `Org_ID` binary(16) NOT NULL,
  `User_ID` binary(16) NOT NULL,
  `AccessType_ID` binary(16) NOT NULL,
  PRIMARY KEY (`Org_ID`,`User_ID`),
  KEY `IX_useraccess_AccessType_ID` (`AccessType_ID`),
  KEY `IX_useraccess_User_ID` (`User_ID`),
  CONSTRAINT `FK_useraccess_accesstype_AccessType_ID` FOREIGN KEY (`AccessType_ID`) REFERENCES `accesstype` (`AccessType_ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_useraccess_organisation_Org_ID` FOREIGN KEY (`Org_ID`) REFERENCES `organisation` (`Org_ID`) ON DELETE CASCADE,
  CONSTRAINT `FK_useraccess_user_User_ID` FOREIGN KEY (`User_ID`) REFERENCES `user` (`User_ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-11-22 15:43:43
