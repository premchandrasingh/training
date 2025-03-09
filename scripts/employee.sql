CREATE TABLE `employee` (
  `EmployeeId` bigint NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `LastName` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci DEFAULT NULL,
  `Password` varchar(100) NOT NULL,
  `Role` varchar(50) NOT NULL,
  `IsActive` tinyint NOT NULL DEFAULT (1),
  `CreatedBy` varchar(100) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `CreatedDate` datetime NOT NULL DEFAULT (utc_timestamp()),
  PRIMARY KEY (`EmployeeId`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;