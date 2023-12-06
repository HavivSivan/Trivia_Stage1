Create Database "Trivia";
use Trivia
CREATE TABLE Players (
  PlayerId INT IDENTITY(0,1) PRIMARY KEY,
  Email VARCHAR(255),
  [Password] varchar(255),
  PlayerName VARCHAR(200),
  Ranking INT,
  Points INT,
  QuestionsMade INT
  CONSTRAINT FK_Ranking 
  FOREIGN KEY (Ranking)
  REFERENCES Rank(RankId)
);
CREATE TABLE Rank (
  RankId INT IDENTITY(1,1) PRIMARY KEY,
  RankName VARCHAR(200)
);

CREATE TABLE QuestionStatus (
  StatusId INT IDENTITY(1,1) PRIMARY KEY,
  Status VARCHAR(200)
);

CREATE TABLE Subjects(
  SubjectId INT IDENTITY(1,1) PRIMARY KEY,
  SubjectName VARCHAR(200)
);
CREATE TABLE Questions (
  QuestionId INT IDENTITY(1,1) PRIMARY KEY,
  PlayerId INT,
  Correct VARCHAR(200),
  Incorrect1 VARCHAR(200),
  Incorrect2 VARCHAR(200),
  Incorrect3 VARCHAR(200),
  QuestionText VARCHAR(200),
  SubjectId INT,
  StatusId INT,
  CONSTRAINT FK_PlayerId 
  FOREIGN KEY (PlayerId)	
  REFERENCES Players(PlayerId),
  CONSTRAINT FK_SubjectId
  FOREIGN KEY (SubjectId)
  REFERENCES Subjects(SubjectId),
  CONSTRAINT FK_StatusId
  FOREIGN KEY (StatusId)
  REFERENCES QuestionStatus(StatusId)
);
INSERT INTO Players (Email,[Password], PlayerName, Ranking, Points, QuestionsMade)
  VALUES ('Admin@yahoo.com','1234', 'Admin', 3, 0, 5)
INSERT INTO Rank (RankName)
  VALUES ('Trainee')
INSERT INTO Rank (RankName)
  VALUES ('Master')
INSERT INTO Rank (RankName)
  VALUES ('Admin')
INSERT INTO Subjects (SubjectName)
  VALUES ('Sports')
INSERT INTO Subjects (SubjectName)
  VALUES ('Politics')
INSERT INTO Subjects (SubjectName)
  VALUES ('History')
INSERT INTO Subjects (SubjectName)
  VALUES ('Science')
INSERT INTO Subjects (SubjectName)
 VALUES ('Ramon')
INSERT INTO QuestionStatus (Status)
  VALUES ('Pending')
INSERT INTO QuestionStatus (Status)
  VALUES ('Approved')
INSERT INTO QuestionStatus (Status)
  VALUES ('Declined')
  use Trivia
SET IDENTITY_INSERT  [Trivia].[dbo].[Questions]  ON 
INSERT INTO Questions (QuestionId,PlayerId, Correct, Incorrect1, Incorrect2, Incorrect3, QuestionText, SubjectId)
  VALUES (1,0, '90 minutes', '43 minutes','80 minutes','110 minutes','How long is a soccer game?', 1)
INSERT INTO Questions (QuestionId,PlayerId, Correct, Incorrect1, Incorrect2, Incorrect3, QuestionText, SubjectId)
  VALUES (2,0, 'Barack Hussein Obama', 'Obama Care','Donald Johnathan Trump','George Herbert Walker Bush','Who was the 44th PotUS?', 2)
INSERT INTO Questions (QuestionId,PlayerId, Correct, Incorrect1, Incorrect2, Incorrect3, QuestionText, SubjectId)
  VALUES (3,0, 'THE DUTCH', 'THE BRITS','THE GERMANS','THE MURICANS','Who started the slave trade?', 3)
INSERT INTO Questions (QuestionId,PlayerId, Correct, Incorrect1, Incorrect2, Incorrect3, QuestionText, SubjectId)
  VALUES (4,0, '299 792 458','3x10^8 ','9x10^23','1','What is the speed of light? (m/s)', 4)
INSERT INTO Questions (QuestionId,PlayerId, Correct, Incorrect1, Incorrect2, Incorrect3, QuestionText, SubjectId)
  VALUES (5,0, '2005', '2003','2010','1995','When was Ramon high school established?', 5)
  SET IDENTITY_INSERT [Trivia].[dbo].[Questions]  Off 
