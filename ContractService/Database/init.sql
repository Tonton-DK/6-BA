CREATE DATABASE test;
USE test;

CREATE TABLE Contract (
  ID CHAR(36) PRIMARY KEY,
  JobId CHAR(36) NOT NULL,
  OfferId CHAR(36) NOT NULL,
  ClientId CHAR(36) NOT NULL,
  ProviderId CHAR(36) NOT NULL,
  CreationDate DATETIME NOT NULL,
  ClosedDate DATETIME,
  State ENUM ('Open', 'Concluded', 'Cancelled') NOT NULL
);

ALTER user 'root' IDENTIFIED WITH mysql_native_password BY '';
flush privileges;
