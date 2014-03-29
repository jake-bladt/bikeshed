CREATE PROCEDURE [dbo].[getElectionWinners]
	@electionId int
AS
BEGIN
  SELECT s.Name AS WinnerName, s.Id AS WinnerId, w.OrdinalRank, w.PointValue
    FROM Subjects s
	INNER JOIN ElectionWinner w
	  ON w.SubjectId = s.Id
	WHERE w.ElectionId = @electionId 
END
