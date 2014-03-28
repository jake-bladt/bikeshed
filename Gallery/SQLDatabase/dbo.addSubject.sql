CREATE PROCEDURE [dbo].[AddSubject]
	@name varchar(50), @displayName varchar(50), @imageCount int
AS
BEGIN
  INSERT INTO Subjects(Name, DisplayName, ImageCount) VALUES(@name, @displayName, @imageCount)
  RETURN @@IDENTITY
END