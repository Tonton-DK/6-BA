CREATE DATABASE db;
USE db;

CREATE TABLE Category (
  ID CHAR(36) PRIMARY KEY,
  Name NVARCHAR(500) NOT NULL,
  Description NVARCHAR(500) NOT NULL
);

INSERT INTO Category(ID, Name, Description) VALUES("8f802c9b-ea67-4771-82e0-ff168c4d2222", "Carpentry", "");
INSERT INTO Category(ID, Name, Description) VALUES("edd0a905-2f7c-4721-a893-8e73aeb187b9", "Child care", "");
INSERT INTO Category(ID, Name, Description) VALUES("2841f85c-2c30-4761-95c3-a67b7d738058", "Cleaning - house", "");
INSERT INTO Category(ID, Name, Description) VALUES("a6b497a4-e32d-434f-abfb-5084b9c8780c", "Cleaning - windows", "");
INSERT INTO Category(ID, Name, Description) VALUES("4f054e3b-a90a-4cd6-8e4b-815d32be50fd", "Delivery", "");
INSERT INTO Category(ID, Name, Description) VALUES("fb09cadb-ed71-4934-9700-64f17e30fe6d", "Dog care", "");
INSERT INTO Category(ID, Name, Description) VALUES("8a05e791-d284-46d8-9e5e-f5cb079908aa", "Errands misc.", "");
INSERT INTO Category(ID, Name, Description) VALUES("7fdf708c-fbea-4520-8eff-d85c44a794a0", "Furniture assembly", "");
INSERT INTO Category(ID, Name, Description) VALUES("3ecdcc3f-5ebd-4752-b637-8eb2971ce5fc", "Garden work", "");
INSERT INTO Category(ID, Name, Description) VALUES("9d89c23c-0f81-46ac-93ff-a93d01a058eb", "Handyman misc.", "");
INSERT INTO Category(ID, Name, Description) VALUES("f3d5e436-a124-4b39-b89c-64c8a68af50a", "Homework help", "");
INSERT INTO Category(ID, Name, Description) VALUES("87e77df5-fd88-4df5-928e-cc934191f5ac", "Lawn mowing", "");
INSERT INTO Category(ID, Name, Description) VALUES("9c096091-ca3b-4121-941b-f5d5dcfba304", "Moving help", "");
INSERT INTO Category(ID, Name, Description) VALUES("0813cf38-cc19-436d-835c-3d6d5d131b89", "Painting", "");
INSERT INTO Category(ID, Name, Description) VALUES("00d8f580-4ce3-4c73-bcca-0e34bd0c5140", "Repairing - minor", "");
INSERT INTO Category(ID, Name, Description) VALUES("cd7d224c-636a-4daf-a22f-4028d8515aa8", "Repairing - vehicle (car, boat, bicycle)", "");
INSERT INTO Category(ID, Name, Description) VALUES("5648ae7c-7efa-4437-af50-027d1eeaa459", "Serving / other help for party", "");
INSERT INTO Category(ID, Name, Description) VALUES("2b83bc89-b1fe-4749-b3a7-80485935a646", "Shopping", "");
INSERT INTO Category(ID, Name, Description) VALUES("99bec542-a2bc-475a-94b4-c593cc9f8e83", "Tech help", "");

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

ALTER user 'root' IDENTIFIED WITH mysql_native_password BY '';
flush privileges;
