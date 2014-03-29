CREATE TABLE [dbo].[Elections]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [EventDate] DATETIME NOT NULL, 
    [Name] VARCHAR(50) NOT NULL, 
    [WinnerCount] INT NOT NULL, 
    [ElectionTypeId] INT NOT NULL, 
    CONSTRAINT [FK_Elections_ToElectionType] FOREIGN KEY ([ElectionTypeId]) REFERENCES [ElectionType]([Id])
)
