CREATE DATABASE test;
USE test;

CREATE TABLE Offer (
  OfferID CHAR(36) PRIMARY KEY,
  ParentJobId CHAR(36) NOT NULL,
  ProviderId CHAR(36) NOT NULL,
  PreviousOfferId CHAR(36),
  Price int NOT NULL,
  Duration VARCHAR(500) NOT NULL,
  DATE DATETIME NOT NULL
);

ALTER user 'root' IDENTIFIED WITH mysql_native_password BY '';
flush privileges;
