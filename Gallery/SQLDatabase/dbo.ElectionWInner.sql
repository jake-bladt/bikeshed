CREATE TABLE [dbo].ElectionWinner
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [OrdinalRank] INT NOT NULL, 
    [PointValue] INT NOT NULL, 
    [ElectionId] INT NOT NULL, 
    [SubjectId] INT NOT NULL, 
    CONSTRAINT [FK_ElectionWinner_ToElection] FOREIGN KEY ([ElectionId]) REFERENCES [Elections]([Id]),
	CONSTRAINT [FK_ElectionWinner_ToSubject] FOREIGN KEY ([SubjectId]) REFERENCES [Subjects]([Id])
)
