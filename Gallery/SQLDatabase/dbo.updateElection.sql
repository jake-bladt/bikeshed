CREATE PROCEDURE [dbo].[updateElection]
	@id int,
	@date DateTime,
	@name varchar(50),
	@winnerCount int,
	@type varchar(50)
AS
BEGIN
	DECLARE @typeId INT
	SELECT @typeId = Id FROM ElectionType Where Name = @type
	UPDATE Elections SET EventDate = @date, Name = @name, WinnerCount = @winnerCount, ElectionTypeId = @typeId 
	  WHERE Id = @id
END
