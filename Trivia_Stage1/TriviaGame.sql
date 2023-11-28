
CREATE TABLE Players (
  PlayerId INT PRIMARY KEY,
  Email VARCHAR(255),
  PlayerName VARCHAR(200),
  Ranking INT,
  Points INT,
  QuestionsMade INT
);
CREATE TABLE Rank (
  RankId INT PRIMARY KEY,
  RankName VARCHAR(200)
);

CREATE TABLE QuestionStatus (
  StatusId INT PRIMARY KEY,
  Status VARCHAR(200)
);

CREATE TABLE Subjects(
  SubjectId INT PRIMARY KEY,
  SubjectName VARCHAR(200)
);
CREATE TABLE Questions (
  QuestionId INT PRIMARY KEY,
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