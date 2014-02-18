CREATE PROCEDURE procGetWeeklySteps
(
	@userLogin varchar(50),
	@yearId INT,
	@weekId INT
)
AS
BEGIN

SELECT wk.DateRange, wk.Goal, d.DayOfWeek2Char, sd
FROM Weeks wk INNER JOIN
	StepDay sd ON (sd.WDID = wk.WDID) INNER JOIN
	Day d ON (sd.DayID = d.DayID)
WHERE wk.UserLogin = @userLogin AND
	wk.YearID = @yearID AND
	wk.WeekInYearID = @weekId

END