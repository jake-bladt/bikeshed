CREATE TABLE [dbo].[Subjects]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(50) NOT NULL, 
    [DisplayName] VARCHAR(50) NOT NULL, 
    [ImageCount] INT NOT NULL
)
