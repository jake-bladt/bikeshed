CREATE PROCEDURE [dbo].[CreateElection]
	@date DateTime,
	@name varchar(50),
	@winnerCount int,
	@type varchar(50)
AS
BEGIN
	DECLARE @typeId INT
	SELECT @typeId = Id FROM ElectionType Where Name = @type
	INSERT INTO Elections(EventDate, Name, WinnerCount, ElectionTypeId) 
	  VALUES(@date, @name, @winnerCount, @typeId)
	SELECT @@IDENTITY
END
