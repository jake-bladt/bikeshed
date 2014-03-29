CREATE PROCEDURE [dbo].[clearElectionWinners]
	@electionId INT
AS
BEGIN
	DELETE FROM ElectionWinner WHERE ElectionId = @electionId
END
