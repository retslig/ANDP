CREATE TABLE [srtc].[StatusTypeEnum] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_StatusTypeEnum] PRIMARY KEY CLUSTERED ([Id] ASC)
);

