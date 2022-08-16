-- MySQL dump 10.13  Distrib 8.0.24, for Win64 (x86_64)
--
-- Host: localhost    Database: gestao_indra
-- ------------------------------------------------------
-- Server version	8.0.24

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
-- Table structure for table `pessoasfisicas`
--

DROP TABLE IF EXISTS `pessoasfisicas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pessoasfisicas` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nome` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `DtNascimento` datetime(6) NOT NULL,
  `Sexo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Email` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Senha` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConfirmacaoSenha` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Cpf` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Celular` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Tipo` int NOT NULL,
  `DtCriacao` datetime(6) NOT NULL,
  `DtAlteracao` datetime(6) NOT NULL,
  `Ativo` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pessoasfisicas`
--

LOCK TABLES `pessoasfisicas` WRITE;
/*!40000 ALTER TABLE `pessoasfisicas` DISABLE KEYS */;
INSERT INTO `pessoasfisicas` VALUES (1,'Cliente Teste','2021-05-04 00:00:00.000000','Masculino','cliente@cliente','cliente','cliente','025.847.871-30','(00) 00000-0000',1,'2021-05-04 10:31:49.000000','2021-06-10 11:32:18.977404',1),(2,'Profissional','2021-05-04 00:00:00.000000','Masculino','prof@prof333','123123','123123','000.000.000-01','(00) 00000-0001',0,'2021-05-04 10:32:32.000000','2021-06-10 11:52:41.896138',0),(3,'Manel','2004-04-24 00:00:00.000000','Feminino','alanbrado@gmail.com','vila2016','vila2016','012.354.568-79','(62) 62626-2626',1,'2021-05-06 22:01:22.252735','2021-06-10 11:38:28.922140',0),(4,'Claudia Patricia da Silva','1989-05-07 00:00:00.000000','Feminino','cliente2@cliente','1234','1234','123.406.400-63','(62) 62626-2626',1,'2021-05-07 00:14:08.096823','2021-05-07 00:14:08.097094',1),(6,'Bruno Carlos da Silva','2001-08-23 00:00:00.000000','Masculino','brunocdasilva12@gmail.com','bruno','bruno','707.024.011-29','(62) 99247-3911',0,'2021-05-07 14:26:44.000000','2021-06-06 04:05:45.881412',1),(7,'Eliane da Silva Costa','1970-12-31 00:00:00.000000','Masculino','eliane@gmail.com','12345','12345','165.409.839-46','(62) 99412-8545',0,'2021-05-30 00:30:39.822613','2021-05-30 00:34:21.839446',1),(8,'Emanuel Italo Ferreira Menezes','1999-05-29 00:00:00.000000','Masculino','pluggg10@gmail.com','168065400','168065400','025.847.871-30','(62) 98519-4429',1,'2021-06-03 19:57:00.684106','2021-06-10 11:40:38.748663',0),(9,'cliente','1999-05-12 00:00:00.000000','Masculino','cliente@cliente2','123456','123456','156.615.616-51','(62) 32323-2322',1,'2021-06-03 20:38:51.644618','2021-06-10 11:40:31.363063',0),(10,'Vinicin do Botox','1999-12-05 00:00:00.000000','Masculino','vinicin@botox','123456','123456','516.515.616-51','(62) 98519-4581',0,'2021-06-03 22:24:08.103654','2021-06-03 22:32:20.639364',0),(11,'Gabriel gomes','2021-06-03 00:00:00.000000','Masculino','gabrielteans@gmail.com','123456','123456','709.236.841-31','(62) 99203-9748',1,'2021-06-03 22:27:48.671301','2021-06-03 22:27:48.671303',1),(12,'Caio Henrick de Oliveira','2000-08-17 00:00:00.000000','Masculino','caiege_@hotmail.com','102030','102030','704.583.421-73','(62) 99286-6869',1,'2021-06-06 02:32:11.549384','2021-06-10 11:39:30.761353',0),(13,'Carlebs Clouds ','2021-06-12 00:00:00.000000','Masculino','carlos.io@outlook.com','123','123','121.212.121-21','(77) 77777-7777',1,'2021-06-06 02:41:27.252444','2021-06-10 11:38:54.405744',0),(14,'Solange Menzes','1981-08-12 00:00:00.000000','Feminino','solelohin@gmail.com','168065400Ff','168065400Ff','001.368.031-56','(62) 98581-5947',1,'2021-06-06 03:21:55.121407','2021-06-10 11:40:22.985992',0),(15,'Beatriz Lins','1984-05-29 00:00:00.000000','Feminino','bealins@gmail.com','168065400Ff','168065400Ff','188.919.841-89','(62) 95985-9559',0,'2021-06-06 03:25:12.092081','2021-06-06 03:25:12.092089',1),(16,'Luiz Carlos Prestes','1999-05-29 00:00:00.000000','vapo','bealins@gmail.com','168065400Ff','168065400Ff','02584787130','629851984125',0,'0001-01-01 00:00:00.000000','2021-06-06 03:52:59.168508',0),(19,'Luiza Santos','1999-05-29 00:00:00.000000','Masculino','luizasantos@gmail.com','168065400','168065400','168.065.400-12','(62) 98519-4429',1,'2021-06-06 03:35:16.575691','2021-06-10 11:39:19.421674',0),(20,'Gabriela Vieira ','1999-05-23 00:00:00.000000','Feminino','gabrielavieira-@hotmail.com','Gabriela1@','Gabriela1@','701.794.871-83','(62) 98123-2305',1,'2021-06-10 00:59:44.559809','2021-06-10 00:59:44.559858',1),(21,'Enrique Buzzi','1990-10-02 00:00:00.000000','Masculino','buzzipsy@gmail.com','013755esb','013755esb','000.440.801-23','(62) 98171-8367',1,'2021-06-10 11:33:32.966001','2021-06-10 11:38:12.976611',0),(22,'Cristian Leandro','1998-01-29 00:00:00.000000','Masculino','cristiangynmarchetti@gmail.com','19769898','19769898','700.773.011-61','(62) 99310-3057',1,'2021-06-14 11:25:00.680391','2021-06-14 11:25:00.680498',1),(23,'Administrador','2001-08-23 00:00:00.000000','Masculino','admin@admin','admin','admin','123.456.789-00','(62) 99123-2345',2,'2021-06-14 11:25:00.680391','2021-06-14 11:25:00.680498',1);
/*!40000 ALTER TABLE `pessoasfisicas` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-06-25 17:31:41
