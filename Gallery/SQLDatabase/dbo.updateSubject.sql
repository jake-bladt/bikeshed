CREATE PROCEDURE [dbo].[updateSubject]
	@id int, @name varchar(50), @displayName varchar(50), @imageCount int
AS
BEGIN
	UPDATE Subjects 
	  SET Name = @name, DisplayName = @displayName, ImageCount = @imageCount
	  WHERE id = @id
END
