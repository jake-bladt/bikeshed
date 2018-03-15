ALTER PROCEDURE [dbo].[getSubjectWins]
  @subjectName varchar(50),
  @pointTotal INT OUTPUT,
  @rank INT OUTPUT
AS
BEGIN

SELECT * INTO #ranks FROM (
    SELECT s.Name, RANK() OVER (ORDER BY SUM(w.PointValue) DESC, s.ImageCount DESC, s.Name) As SubjectRank
    FROM Subjects s
    INNER JOIN ElectionWinner w
      ON s.Id = w.SubjectId
    GROUP BY s.Name, s.ImageCount
) AS notUsed

SELECT @rank = SubjectRank FROM #ranks WHERE Name = @subjectName

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
