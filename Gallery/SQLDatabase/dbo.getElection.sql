CREATE PROCEDURE [dbo].[getElection]
	@name varchar(50) = NULL,
	@id int = NULL
AS
	SELECT e.EventDate, e.Name AS ElectionName, WinnerCount, t.Name AS ElectionType
	  FROM Elections e INNER JOIN
	  ElectionType t ON (e.ElectionTypeId = t.Id)
	  WHERE (e.Name = @name OR @name IS NULL) AND
	        (e.Id = @id OR @id IS NULL)
RETURN 0
