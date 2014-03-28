CREATE TABLE [dbo].[Subjects] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50) NOT NULL,
    [DisplayName] VARCHAR (50) NOT NULL,
    [ImageCount]  INT          NOT NULL,
    [DateAdded] DATETIME NOT NULL DEFAULT GetDate(), 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

