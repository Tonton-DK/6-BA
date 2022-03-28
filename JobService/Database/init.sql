CREATE DATABASE test;
USE test;

CREATE TABLE Job (
  JobID CHAR(36) PRIMARY KEY,
  Title NVARCHAR(500) NOT NULL,
  Description NVARCHAR(500) NOT NULL
);

ALTER user 'root' IDENTIFIED WITH mysql_native_password BY '';
flush privileges;
