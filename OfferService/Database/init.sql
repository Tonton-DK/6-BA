CREATE DATABASE db;
USE db;

CREATE TABLE Offer (
  ID CHAR(36) PRIMARY KEY,
  JobId CHAR(36) NOT NULL,
  ProviderId CHAR(36) NOT NULL,
  PreviousOfferId CHAR(36),
  Price int NOT NULL,
  Duration VARCHAR(500) NOT NULL,
  DATE DATETIME NOT NULL,
  State ENUM('Open', 'Concluded', 'Cancelled'),
  Comment NVARCHAR(500) NOT NULL
);

ALTER user 'root' IDENTIFIED WITH mysql_native_password BY '';
flush privileges;
