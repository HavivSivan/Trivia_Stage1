Create Database "Trivia";
use Trivia
CREATE TABLE Players (
  PlayerId INT IDENTITY(0,1) PRIMARY KEY,
  Email VARCHAR(255),
  PlayerName VARCHAR(200),
  Ranking INT,
  Points INT,
  QuestionsMade INT
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
  CONSTRAINT FK_PlayerId 
  FOREIGN KEY (PlayerId)	
  REFERENCES Players(PlayerId),
  CONSTRAINT FK_SubjectId
  FOREIGN KEY (SubjectId)
  REFERENCES Subjects(SubjectId)
);
INSERT INTO Players (Email, PlayerName, Ranking, Points, QuestionsMade)
VALUES (Admin@yahoo.com, Admin, 3, 0, 5),
INSERT INTO Rank (RankName)
VALUES ("Trainee")
VALUES ("Master")
VALUES ("Admin"),
INSERT INTO Subjects (SubjectName)
VALUES ("
