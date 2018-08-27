CREATE TABLE [test].[ItemTypeEnum] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ItemTypeEnum] PRIMARY KEY CLUSTERED ([Id] ASC)
);

