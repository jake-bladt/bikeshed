CREATE PROCEDURE [dbo].[getAllSubjects]
AS
BEGIN
	SELECT id, Name, DisplayName, ImageCount FROM Subjects ORDER BY Name
END
