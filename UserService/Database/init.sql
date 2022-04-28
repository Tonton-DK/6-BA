CREATE DATABASE db;
USE db;

CREATE TABLE User (
  ID CHAR(36) PRIMARY KEY,
  Email NVARCHAR(500) NOT NULL,
  PasswordSalt NVARCHAR(500) NOT NULL,
  PasswordHash NVARCHAR(500) NOT NULL,
  Firstname NVARCHAR(500) NOT NULL,
  Lastname NVARCHAR(500) NOT NULL,
  PhoneNumber NVARCHAR(500) NOT NULL,
  ProfilePicture NVARCHAR(500) NOT NULL,
  IsServiceProvider BIT NOT NULL
);

ALTER user 'root' IDENTIFIED WITH mysql_native_password BY '';
flush privileges;
