CREATE DATABASE test;
USE test;

CREATE TABLE Category (
  ID CHAR(36) PRIMARY KEY,

  Name NVARCHAR(500) NOT NULL,
  Description NVARCHAR(500) NOT NULL
);

INSERT INTO Category(ID, Name, Description) VALUES("156be3a6-5537-41f8-9608-705c7cd7cbc3", "Garden Work", "Fix my garden, please.");
INSERT INTO Category(ID, Name, Description) VALUES("0ebbe367-300a-4c86-9070-d6e106d7e4b9", "Furniture Assembly", "Fix my furniture, please.");
INSERT INTO Category(ID, Name, Description) VALUES("62507ed1-e247-4fb1-bbf8-0a73479cc911", "Dog Walking", "Walk my dog, please.");

CREATE TABLE Job (
  ID CHAR(36) PRIMARY KEY,
  
  Title NVARCHAR(500) NOT NULL,
  Description NVARCHAR(500) NOT NULL,
  Deadline DATETIME NOT NULL,
  
  Road NVARCHAR(500) NOT NULL,
  Number NVARCHAR(500) NOT NULL,
  Zip NVARCHAR(500) NOT NULL,
  
  ClientID CHAR(36) NOT NULL,
  
  CategoryID CHAR(36),
  FOREIGN KEY(CategoryID) REFERENCES Category(ID)
);

INSERT INTO Job(ID, Title, Description, Deadline, ClientID, CategoryID, Road, Number, Zip) VALUES("156be3a6-5537-41f8-9608-705c7cd7cbc3", "IKEA Sofa", "Fix my sofa, please.", "2020-04-20 23:00:00", "62507ed1-e247-4fb1-bbf8-0a73479cc911", "0ebbe367-300a-4c86-9070-d6e106d7e4b9", "SDU Campus", "1", "5000");

ALTER user 'root' IDENTIFIED WITH mysql_native_password BY '';
flush privileges;
