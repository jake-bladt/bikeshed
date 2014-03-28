CREATE PROCEDURE [dbo].[addSubject]
	@name varchar(50), @displayName varchar(50), @imageCount int
AS
BEGIN
  INSERT INTO Subjects(Name, DisplayName, ImageCount) VALUES(@name, @displayName, @imageCount)
  SELECT @@IDENTITY
END