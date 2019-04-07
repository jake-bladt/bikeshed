USE [master]
GO
/****** Object:  Database [gal00]    Script Date: 10/20/2014 8:37:50 AM ******/
CREATE DATABASE [gal00]
GO
ALTER DATABASE [gal00] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [gal00].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [gal00] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [gal00] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [gal00] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [gal00] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [gal00] SET ARITHABORT OFF 
GO
ALTER DATABASE [gal00] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [gal00] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [gal00] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [gal00] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [gal00] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [gal00] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [gal00] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [gal00] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [gal00] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [gal00] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [gal00] SET  ENABLE_BROKER 
GO
ALTER DATABASE [gal00] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [gal00] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [gal00] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [gal00] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [gal00] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [gal00] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [gal00] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [gal00] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [gal00] SET  MULTI_USER 
GO
ALTER DATABASE [gal00] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [gal00] SET DB_CHAINING OFF 
GO
ALTER DATABASE [gal00] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [gal00] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [gal00]
GO
/****** Object:  StoredProcedure [dbo].[addElection]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[addElection]
	@date DateTime,
	@name varchar(50),
	@winnerCount int,
	@typeName varchar(50) = NULL,
	@typeId int = NULL

AS
BEGIN
    IF @typeId IS NULL
	BEGIN
	  SELECT @typeId = Id FROM ElectionType Where Name = @typeName
	END
	INSERT INTO Elections(EventDate, Name, WinnerCount, ElectionTypeId) 
	  VALUES(@date, @name, @winnerCount, @typeId)
	SELECT @@IDENTITY
END
GO
/****** Object:  StoredProcedure [dbo].[addElectionWinner]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[addElectionWinner]
  @electionId int,
  @winnerId int,
  @rank int,
  @points int
AS
BEGIN
  INSERT INTO ElectionWinner(ElectionId, SubjectId, OrdinalRank, PointValue)
    VALUES(@electionId, @winnerId, @rank, @points)
END
GO
/****** Object:  StoredProcedure [dbo].[AddSubject]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddSubject]
	@name varchar(50), @displayName varchar(50), @imageCount int
AS
BEGIN
  INSERT INTO Subjects(Name, DisplayName, ImageCount) VALUES(@name, @displayName, @imageCount)
  SELECT @@IDENTITY
END
GO
/****** Object:  StoredProcedure [dbo].[addSubjectSet]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[addSubjectSet]
	@name varchar(50)
AS
BEGIN
	INSERT INTo SubjectSet(SetName) VALUES(@name)
	SELECT @@IDENTITY
END
GO
/****** Object:  StoredProcedure [dbo].[addSubjectSetMember]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[addSubjectSetMember]
	@setId int,
	@subjectId int = NULL,
	@subjectName varchar(50) = NULL
AS
BEGIN
	IF(@subjectId IS NULL)
	  SELECT @subjectId = Id FROM Subjects WHERE Name = @subjectName

   INSERT INTO SubjectSetMembers(SubjectSetId, SubjectId)
     VALUES(@setId, @subjectId)

	SELECT @@IDENTITY
END
GO
/****** Object:  StoredProcedure [dbo].[clearElectionWinners]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[clearElectionWinners]
	@electionId INT
AS
BEGIN
	DELETE FROM ElectionWinner WHERE ElectionId = @electionId
END
GO
/****** Object:  StoredProcedure [dbo].[getAllSubjects]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getAllSubjects]
AS
BEGIN
	SELECT id, Name, DisplayName, ImageCount FROM Subjects ORDER BY Name
END
GO
/****** Object:  StoredProcedure [dbo].[getElection]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getElection]
	@name varchar(50) = NULL,
	@id int = NULL
AS
BEGIN
	SELECT e.Id, e.EventDate, e.Name AS ElectionName, WinnerCount, t.Name AS ElectionType, e.ElectionTypeId
	  FROM Elections e INNER JOIN
	  ElectionType t ON (e.ElectionTypeId = t.Id)
	  WHERE (e.Name = @name OR @name IS NULL) AND
	        (e.Id = @id OR @id IS NULL)
END
GO
/****** Object:  StoredProcedure [dbo].[getElections]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getElections]
AS
BEGIN
	SELECT e.Id AS ElectionID, e.EventDate, e.Name, e.WinnerCount, 
	  et.Name AS ElectionType, et.Id AS ElectionTypeId 
	FROM Elections e
	INNER JOIN ElectionType et ON e.ElectionTypeId = et.Id 
	ORDER BY e.EventDate, e.Id
END

GO
/****** Object:  StoredProcedure [dbo].[getElectionWinners]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[getElectionWinners]
	@electionId int
AS
BEGIN
  SELECT s.Name AS WinnerName, s.DisplayName, s.ImageCount, s.Id AS WinnerId, w.OrdinalRank, w.PointValue
    FROM Subjects s
	INNER JOIN ElectionWinner w
	  ON w.SubjectId = s.Id
	WHERE w.ElectionId = @electionId
	ORDER BY w.OrdinalRank 
END

GO
/****** Object:  StoredProcedure [dbo].[getRookies]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[getRookies]
AS
BEGIN
  SELECT Id, Name, DisplayName, ImageCount
  FROM Subjects
  WHERE Id NOT IN (SELECT Distinct SubjectId FROM ElectionWinner)

END

GO
/****** Object:  StoredProcedure [dbo].[getSubject]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[getSubject]
(
	@name VARCHAR(50) = NULL,
	@id INT = NULL
)
AS
BEGIN
	SELECT Id, Name, DisplayName, ImageCount, DateAdded
	FROM Subjects
	WHERE (@name IS NULL OR Name = @name)
	  AND (@id IS NULL OR Id = @id)
END


GO
/****** Object:  StoredProcedure [dbo].[getSubjects]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[getSubjects]
AS
BEGIN
	SELECT Id, Name, DisplayName, ImageCount, DateAdded
	FROM Subjects
	ORDER BY DisplayName ASC
END

GO
/****** Object:  StoredProcedure [dbo].[getSubjectSetMembers]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[getSubjectSetMembers]
	@setId INT = NULL,
	@setName VARCHAR(50) = NULL
AS
BEGIN
    IF(@setId IS NULL)
	  SELECT @setId = Id FROM SubjectSet WHERE SetName = @setName
	SELECT s.Id, s.Name, s.DisplayName, s.ImageCount, s.DateAdded
	  FROM SubjectSetMembers ssm
	    INNER JOIN Subjects s ON s.Id = ssm.SubjectId
		WHERE ssm.SubjectSetId = @setId
END

GO
/****** Object:  StoredProcedure [dbo].[updateElection]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[updateElection]
	@id int,
	@date DateTime,
	@name varchar(50),
	@winnerCount int,
	@type varchar(50) = NULL,
	@typeId int
AS
BEGIN
	IF(@typeId IS NULL)
	BEGIN
    	SELECT @typeId = Id FROM ElectionType Where Name = @type
	END
	UPDATE Elections SET EventDate = @date, Name = @name, WinnerCount = @winnerCount, ElectionTypeId = @typeId 
	  WHERE Id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[updateSubject]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[updateSubject]
	@id int = NULL, 
	@name varchar(50), 
	@displayName varchar(50) = NULL, 
	@imageCount int
AS
BEGIN
	IF(@id IS NULL)
	BEGIN
	  SELECT @id = Id FROM Subjects WHERE Name = @name
	END

	UPDATE Subjects 
	  SET Name = @name, DisplayName = COALESCE(@displayName, DisplayName), ImageCount = @imageCount
	  WHERE id = @id
END
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Elections]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Elections](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EventDate] [datetime] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[WinnerCount] [int] NOT NULL,
	[ElectionTypeId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ElectionType]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ElectionType](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ElectionWinner]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ElectionWinner](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrdinalRank] [int] NOT NULL,
	[PointValue] [int] NOT NULL,
	[ElectionId] [int] NOT NULL,
	[SubjectId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SubjectCategories]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubjectCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SubjectId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Subjects]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Subjects](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[DisplayName] [varchar](50) NOT NULL,
	[ImageCount] [int] NOT NULL,
	[DateAdded] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SubjectSet]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SubjectSet](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SetName] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SubjectSetMembers]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubjectSetMembers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SubjectSetId] [int] NOT NULL,
	[SubjectId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Touches]    Script Date: 10/20/2014 8:37:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Touches](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[touchDate] [datetime] NOT NULL,
	[subjectId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Subjects] ADD  DEFAULT (getdate()) FOR [DateAdded]
GO
ALTER TABLE [dbo].[Touches] ADD  DEFAULT (getdate()) FOR [touchDate]
GO
ALTER TABLE [dbo].[Elections]  WITH CHECK ADD  CONSTRAINT [FK_Elections_ToElectionType] FOREIGN KEY([ElectionTypeId])
REFERENCES [dbo].[ElectionType] ([Id])
GO
ALTER TABLE [dbo].[Elections] CHECK CONSTRAINT [FK_Elections_ToElectionType]
GO
ALTER TABLE [dbo].[ElectionWinner]  WITH CHECK ADD  CONSTRAINT [FK_ElectionWinner_ToElection] FOREIGN KEY([ElectionId])
REFERENCES [dbo].[Elections] ([Id])
GO
ALTER TABLE [dbo].[ElectionWinner] CHECK CONSTRAINT [FK_ElectionWinner_ToElection]
GO
ALTER TABLE [dbo].[ElectionWinner]  WITH CHECK ADD  CONSTRAINT [FK_ElectionWinner_ToSubject] FOREIGN KEY([SubjectId])
REFERENCES [dbo].[Subjects] ([Id])
GO
ALTER TABLE [dbo].[ElectionWinner] CHECK CONSTRAINT [FK_ElectionWinner_ToSubject]
GO
ALTER TABLE [dbo].[SubjectCategories]  WITH CHECK ADD  CONSTRAINT [FK_SubjectCategories_ToCategories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
GO
ALTER TABLE [dbo].[SubjectCategories] CHECK CONSTRAINT [FK_SubjectCategories_ToCategories]
GO
ALTER TABLE [dbo].[SubjectCategories]  WITH CHECK ADD  CONSTRAINT [FK_SubjectCategories_ToSubjects] FOREIGN KEY([SubjectId])
REFERENCES [dbo].[Subjects] ([Id])
GO
ALTER TABLE [dbo].[SubjectCategories] CHECK CONSTRAINT [FK_SubjectCategories_ToSubjects]
GO
ALTER TABLE [dbo].[SubjectSetMembers]  WITH CHECK ADD  CONSTRAINT [FK_SubjectSetMembers_ToSubect] FOREIGN KEY([SubjectId])
REFERENCES [dbo].[Subjects] ([Id])
GO
ALTER TABLE [dbo].[SubjectSetMembers] CHECK CONSTRAINT [FK_SubjectSetMembers_ToSubect]
GO
ALTER TABLE [dbo].[SubjectSetMembers]  WITH CHECK ADD  CONSTRAINT [FK_SubjectSetMembers_ToSubectSet] FOREIGN KEY([SubjectSetId])
REFERENCES [dbo].[SubjectSet] ([Id])
GO
ALTER TABLE [dbo].[SubjectSetMembers] CHECK CONSTRAINT [FK_SubjectSetMembers_ToSubectSet]
GO
ALTER TABLE [dbo].[Touches]  WITH CHECK ADD  CONSTRAINT [FK_Touches_ToSubjects] FOREIGN KEY([subjectId])
REFERENCES [dbo].[Subjects] ([Id])
GO
ALTER TABLE [dbo].[Touches] CHECK CONSTRAINT [FK_Touches_ToSubjects]
GO
USE [master]
GO
ALTER DATABASE [gal00] SET  READ_WRITE 
GO

INSERT INTO ElectionType(Id, Name) VALUES(1, 'Travel')
INSERT INTO ElectionType(Id, Name) VALUES(2, 'Rookie')
INSERT INTO ElectionType(Id, Name) VALUES(3, 'Star')
INSERT INTO ElectionType(Id, Name) VALUES(4, 'WalkIn')
INSERT INTO ElectionType(Id, Name) VALUES(5, 'Wonder')
INSERT INTO ElectionType(Id, Name) VALUES(6, 'Runoff')
INSERT INTO ElectionType(Id, Name) VALUES(7, 'Special')
GO

CREATE PROCEDURE [dbo].[getStars]
AS
BEGIN
  SELECT TOP 500 s.Id, s.Name, s.DisplayName, s.ImageCount, SUM(w.PointValue) AS TotalPoints
    FROM Subjects s
    INNER JOIN ElectionWinner w
      ON s.Id = w.SubjectId
    GROUP BY s.Id, s.Name, s.DisplayName, s.ImageCount
    ORDER BY SUM(w.PointValue) DESC, s.ImageCount DESC
END

CREATE PROCEDURE listElections @count INT
AS
BEGIN

SELECT TOP(@count) Id, Name FROM Elections
  ORDER BY Id DESC

END
GO

CREATE PROCEDURE getSubjectWins
  @subjectName varchar(50)
AS
BEGIN
SELECT 
  e.Name AS ElectionName, w.OrdinalRank, w.PointValue
FROM 
  ElectionWinner w
INNER JOIN 
  Subjects s ON (s.Id = w.SubjectId)
INNER JOIN
  Elections e ON (e.Id = w.ElectionId)
WHERE 
  s.Name = @subjectName
ORDER BY
	e.EventDate, e.Name
END


ALTER PROCEDURE getSubjectWins
  @subjectName varchar(50),
  @pointTotal INT OUTPUT
AS
BEGIN
SELECT 
  e.Name AS ElectionName, w.OrdinalRank, w.PointValue
FROM 
  ElectionWinner w
INNER JOIN 
  Subjects s ON (s.Id = w.SubjectId)
INNER JOIN
  Elections e ON (e.Id = w.ElectionId)
WHERE 
  s.Name = @subjectName
ORDER BY
  e.EventDate, e.Name

SET NOCOUNT ON;
DECLARE @subjectId INT

SELECT @subjectId = Id FROM Subjects s WHERE s.Name = @subjectName
SELECT @pointTotal = SUM(w.PointValue) FROM ElectionWinner w WHERE w.SubjectId = @subjectId

RETURN

END
GO

CREATE PROCEDURE getYearSubjects(@year char(4) )
AS
BEGIN

SELECT DISTINCT mem.SubjectId, j.Name, j.DisplayName, COUNT(mem.SubjectId) AS Appears
  FROM SubjectSetMembers mem 
  JOIN Subjects j ON mem.SubjectId = j.Id
  WHERE SubjectSetId IN (
    SELECT Id
    FROM SubjectSet ss
    WHERE ss.SetName LIKE @year + '[0-1][0-9]'
  )
GROUP BY mem.SubjectId, j.Name, j.DisplayName
ORDER BY COUNT(mem.SubjectId) DESC, mem.SubjectId 

END
GO

CREATE PROCEDURE getRookieAging
AS
BEGIN
  SELECT e.Name AS ElectionName, ew.OrdinalRank, s.DisplayName, SUM(ewCumulative.PointValue) AS TotalPoints
    FROM Subjects s INNER JOIN
    ElectionWinner ew ON ew.SubjectId = s.Id INNER JOIN
    Elections e ON e.Id = ew.ElectionId INNER JOIN
    ElectionWinner ewCumulative ON
    ewCumulative.SubjectId = s.Id 
    WHERE e.ElectionTypeId = 2
    GROUP BY s.DisplayName, e.Name, e.EventDate, ew.OrdinalRank
    ORDER BY e.EventDate, ew.OrdinalRank
END

CREATE PROCEDURE getSneakIns AS
BEGIN

	WITH rankedWins AS (
	  SELECT 
		ew.SubjectId, ew.ElectionId, e.ElectionTypeId, e.EventDate, ew.OrdinalRank AS Position,
		  RANK() OVER (PARTITION BY ew.SubjectId ORDER BY e.EventDate ASC) AS timeRank
	  FROM ElectionWinner ew
	  INNER JOIN Elections e ON ew.ElectionId = e.Id 
	)
	SELECT s.Name AS SubjectName, e.Name AS ElectionName, rw.Position
	  FROM rankedWins rw
	  INNER JOIN Subjects s ON s.Id = rw.SubjectId
	  INNER JOIN Elections e ON e.Id = rw.ElectionId
	  WHERE rw.timeRank = 1 AND rw.ElectionTypeId = 4
	  ORDER BY rw.EventDate, s.Name

END
GO

CREATE PROCEDURE getProspects
AS
BEGIN

SELECT * INTO #ranks FROM (
    SELECT s.Id, s.Name, RANK() OVER (ORDER BY SUM(w.PointValue) DESC, s.ImageCount DESC, s.Name) As SubjectRank
    FROM Subjects s
    INNER JOIN ElectionWinner w
      ON s.Id = w.SubjectId
    GROUP BY s.Id, s.Name, s.ImageCount
) AS ua1

SELECT * INTO #slWinners FROM (
SELECT DISTINCT ew.SubjectId, s.Name AS SubjectName
  FROM ElectionWinner ew
  INNER JOIN Elections eSL ON ew.ElectionId = eSL.Id
  INNER JOIN Subjects s ON ew.SubjectId = s.Id
  WHERE eSL.Name LIKE 'secondlook%'
) AS ua2

SELECT TOP 300
  s.Id, s.Name, r.SubjectRank
  FROM Subjects s
    INNER JOIN #ranks r
    	ON s.Id = r.Id
	WHERE r.SubjectRank > 500
	  AND s.Id IN (SELECT SubjectId FROM #slWinners)
	ORDER BY SubjectRank ASC

END
GO

INSERT INTO ElectionType(Id, Name) VALUES(8, 'Prospect')
GO

CREATE PROCEDURE setSubjectCategory
(
	@subjectName varchar(50),
	@categoryName varchar(50),
	@relationshipId INT OUTPUT 
)
AS
BEGIN

DECLARE @categoryId INT
DECLARE @subjectId INT
SET @relationshipId = -1

IF(NOT EXISTS(SELECT Id FROM Categories WHERE CategoryName = @categoryName))
BEGIN
	INSERT INTO Categories(CategoryName) VALUES(@categoryName)
END
SELECT @categoryId = Id FROM Categories WHERE CategoryName = @categoryName

IF(EXISTS(SELECT Id FROM Subjects WHERE Name = @subjectName))
BEGIN
    SELECT @subjectId = Id FROM Subjects WHERE Name = @subjectName
	IF(NOT EXISTS(SELECT Id FROM SubjectCategories WHERE SubjectId = @subjectId AND CategoryId = @categoryId))
	BEGIN
		INSERT INTO SubjectCategories(SubjectId, CategoryId) VALUES(@subjectId, @categoryId)
	END
END

SELECT @relationshipId = Id FROM SubjectCategories WHERE SubjectId = @subjectId AND CategoryId = @categoryId

END
GO

CREATE PROCEDURE getSubjectCategories
(
  @name varchar(50)
)
AS
BEGIN
  SELECT c.CategoryName
    FROM SubjectCategories sc
	  INNER JOIN Categories c ON sc.CategoryId = c.Id
	  INNER JOIN Subjects s ON sc.SubjectId = s.Id
	WHERE s.Name = @name
	ORDER BY c.CategoryName
END
GO

CREATE PROCEDURE getUncats
AS
BEGIN

SELECT * INTO #ranks FROM (
    SELECT s.Id, s.Name, RANK() OVER (ORDER BY SUM(w.PointValue) DESC, s.ImageCount DESC, s.Name) As SubjectRank
    FROM Subjects s
    INNER JOIN ElectionWinner w
      ON s.Id = w.SubjectId
    GROUP BY s.Id, s.Name, s.ImageCount
) AS ua1

SELECT s.Id, s.Name, r.SubjectRank
  FROM Subjects s
    INNER JOIN #ranks r ON r.Id = s.Id
  WHERE s.Id NOT IN (
    SELECT DISTINCT SubjectId FROM SubjectCategories
  )
  ORDER BY r.SubjectRank ASC

END
GO

CREATE PROCEDURE getCategoryCounts
AS
BEGIN
	SELECT c.CategoryName, COUNT(sc.SubjectId) AS SubjectCount
	  FROM SubjectCategories sc
	  INNER JOIN Categories c ON sc.CategoryId = c.Id
	  GROUP BY sc.CategoryId, c.CategoryName
	  ORDER BY c.CategoryName
END
GO

CREATE PROCEDURE getSubjectsByCategory (
  @name VARCHAR(50)
)
AS
BEGIN
  SELECT DISTINCT s.Id, s.Name, s.DisplayName, s.ImageCount
    FROM Subjects s
	INNER JOIN SubjectCategories sc ON s.Id = sc.SubjectId
	INNER JOIN Categories c ON sc.CategoryId = c.Id
	WHERE c.CategoryName = @name
	ORDER BY s.Name
END
GO

INSERT INTO ElectionType(Id, Name) VALUES(9, 'Rider')
GO

CREATE PROCEDURE getOneAndOuts AS
BEGIN

	SELECT s.Id, s.DisplayName, s.Name
	  FROM Subjects s
	  INNER JOIN ElectionWinner ew
		ON s.Id = ew.SubjectId
	  INNER JOIN Elections e
		ON ew.ElectionId = e.Id
	  WHERE e.ElectionTypeId NOT IN (6, 7)
	  GROUP BY s.Id, s.DisplayName, s.Name
	  HAVING COUNT(ew.Id) = 1
	  ORDER BY s.Name

END
GO

CREATE PROCEDURE getSuperannuatedRookies AS
BEGIN

SELECT Id, Name, DisplayName, ImageCount, DateAdded
  FROM Subjects
  WHERE Id NOT IN (SELECT Distinct SubjectId FROM ElectionWinner)
    AND Id NOT IN (SELECT TOP 200 Id FROM Subjects WHERE Id NOT IN (SELECT Distinct SubjectId FROM ElectionWinner) ORDER BY DateAdded DESC)
  ORDER BY Name ASC

END
GO

CREATE FUNCTION getMonthStamp(@dt DATETIME)
RETURNS INT
AS
BEGIN
  DECLARE @ret INT;
	SELECT @ret = YEAR(@dt) * 100 + MONTH(@dt)
	RETURN @ret
END
GO

CREATE PROCEDURE getRisingStars AS
BEGIN

  SELECT TOP 500 s.Id, s.Name, s.DisplayName, s.ImageCount, SUM(w.PointValue) AS TotalPoints, COUNT(e.Id) AS Appearances, SUM(w.PointValue) / COUNT(e.Id) AS AveragePoints
    FROM Subjects s
    INNER JOIN ElectionWinner w
      ON s.Id = w.SubjectId
	INNER JOIN Elections e
	  ON w.ElectionId = e.Id
	WHERE e.Name LIKE 'secondlook%'
	  AND s.Id NOT IN (
		  SELECT TOP 500 s.Id
			FROM Subjects s
			INNER JOIN ElectionWinner w
			  ON s.Id = w.SubjectId
			GROUP BY s.Id, s.ImageCount
			ORDER BY SUM(w.PointValue) DESC, s.ImageCount DESC
	  )
    GROUP BY s.Id, s.Name, s.DisplayName, s.ImageCount
    ORDER BY COUNT(e.Id) + (SUM(w.PointValue) / COUNT(e.Id)) DESC, SUM(w.PointValue) DESC, s.ImageCount DESC, s.Name ASC

END
GO

ALTER PROCEDURE [dbo].[getOneAndOuts] AS
BEGIN

	SELECT s.Id, s.DisplayName, s.Name, s.ImageCount
	  FROM Subjects s
	  INNER JOIN ElectionWinner ew
		ON s.Id = ew.SubjectId
	  INNER JOIN Elections e
		ON ew.ElectionId = e.Id
	  WHERE e.ElectionTypeId NOT IN (6, 7)
	  GROUP BY s.Id, s.DisplayName, s.Name, s.ImageCount
	  HAVING COUNT(ew.Id) = 1
	  ORDER BY s.Name

END
GO

ALTER PROCEDURE [dbo].[getSneakIns] AS
BEGIN

	WITH rankedWins AS (
	  SELECT 
		ew.SubjectId, ew.ElectionId, e.ElectionTypeId, e.EventDate, ew.OrdinalRank AS Position,
		  RANK() OVER (PARTITION BY ew.SubjectId ORDER BY e.EventDate ASC) AS timeRank
	  FROM ElectionWinner ew
	  INNER JOIN Elections e ON ew.ElectionId = e.Id 
	)
	SELECT s.Id, s.Name AS SubjectName, e.Name AS ElectionName, rw.Position, s.Name, s.DisplayName, s.ImageCount
	  FROM rankedWins rw
	  INNER JOIN Subjects s ON s.Id = rw.SubjectId
	  INNER JOIN Elections e ON e.Id = rw.ElectionId
	  WHERE rw.timeRank = 1 AND rw.ElectionTypeId = 4
	  ORDER BY rw.EventDate, s.Name

END
GO

ALTER PROCEDURE [dbo].[getProspects]
AS
BEGIN

SELECT * INTO #ranks FROM (
    SELECT s.Id, s.Name, RANK() OVER (ORDER BY SUM(w.PointValue) DESC, s.ImageCount DESC, s.Name) As SubjectRank
    FROM Subjects s
    INNER JOIN ElectionWinner w
      ON s.Id = w.SubjectId
    GROUP BY s.Id, s.Name, s.ImageCount
) AS ua1

SELECT * INTO #slWinners FROM (
SELECT DISTINCT ew.SubjectId, s.Name AS SubjectName
  FROM ElectionWinner ew
  INNER JOIN Elections eSL ON ew.ElectionId = eSL.Id
  INNER JOIN Subjects s ON ew.SubjectId = s.Id
  WHERE eSL.Name LIKE 'thirdlook%'
) AS ua2

SELECT TOP 300
  s.Id, s.Name, s.DisplayName, s.ImageCount, r.SubjectRank
  FROM Subjects s
    INNER JOIN #ranks r
    	ON s.Id = r.Id
	WHERE r.SubjectRank > 500
	  AND s.Id IN (SELECT SubjectId FROM #slWinners)
	ORDER BY SubjectRank ASC

END
GO

CREATE PROCEDURE getSubjectCategoryList AS
BEGIN
  SELECT s.Name, c.CategoryName 
  FROM Subjects s
    INNER JOIN SubjectCategories sc ON s.Id = sc.SubjectId
	INNER JOIN Categories c ON sc.CategoryId = c.Id
  ORDER BY Name
END
GO

CREATE PROCEDURE getTopHistoricRookies
AS
BEGIN
SELECT s.Id, s.Name, s.DisplayName, s.ImageCount
  FROM ElectionWinner ew
  INNER JOIN Elections e ON (ew.ElectionId = e.Id)
  INNER JOIN Subjects s ON (ew.SubjectId = s.Id) 
  WHERE (ew.OrdinalRank <= 5) AND e.ElectionTypeId = 2
  ORDER BY s.Name
END
GO
