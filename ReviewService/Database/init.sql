CREATE DATABASE test;
USE test;

CREATE TABLE Review (
  ID CHAR(36) PRIMARY KEY,
  ContractId CHAR(36) NOT NULL,
  CreatorId CHAR(36) NOT NULL,
  TargetId CHAR(36) NOT NULL,
  Comment VARCHAR(500) NOT NULL,
  Rating SMALLINT NOT NULL,
  ReviewType SMALLINT NOT NULL
);

ALTER user 'root' IDENTIFIED WITH mysql_native_password BY '';
flush privileges;
