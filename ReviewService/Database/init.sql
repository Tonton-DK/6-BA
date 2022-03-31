CREATE DATABASE test;
USE test;

CREATE TABLE User (
  UserID CHAR(36) PRIMARY KEY,
  Firstname NVARCHAR(500) NOT NULL,
  Lastname NVARCHAR(500) NOT NULL
);

ALTER user 'root' IDENTIFIED WITH mysql_native_password BY '';
flush privileges;
