﻿USE MailReader;
IF OBJECT_ID('Mails', 'U') IS NULL
CREATE TABLE Mails
(
  Id INT NOT NULL PRIMARY KEY,
  Subject NVARCHAR(255),
  [From] NVARCHAR(255),
  Body NVARCHAR(MAX)
)