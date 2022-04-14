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
  IsServiceProvider BIT NOT NULL
);

INSERT INTO User(ID, Email, PasswordSalt, PasswordHash, Firstname, Lastname, PhoneNumber, IsServiceProvider) VALUES("156be3a6-5537-41f8-9608-705c7cd7cbc3", "test@mail.dk", "s7eu/Mz30bvsZnjSBot7gg==", "O+3jrM6+EudEJ3WwNS67aFqEi5eztupag3u+YG3Es64=", "John", "Doe", "12345678", 0);

ALTER user 'root' IDENTIFIED WITH mysql_native_password BY '';
flush privileges;
