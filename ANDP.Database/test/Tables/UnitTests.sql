CREATE TABLE [test].[UnitTests] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [OrderId]          INT            NOT NULL,
    [ExpectedCommands] NVARCHAR (MAX) NOT NULL,
    [CreatedByUser]    NVARCHAR (20)  NOT NULL,
    [ModifiedByUser]   NVARCHAR (20)  NOT NULL,
    [DateCreated]      DATETIME2 (7)  NOT NULL,
    [DateModified]     DATETIME2 (7)  NOT NULL,
    [Version]          INT            NOT NULL,
    CONSTRAINT [PK_UnitTests] PRIMARY KEY CLUSTERED ([Id] ASC)
);

