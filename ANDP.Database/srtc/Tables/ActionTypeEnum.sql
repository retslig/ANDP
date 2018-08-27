CREATE TABLE [srtc].[ActionTypeEnum] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ActionTypeEnum] PRIMARY KEY CLUSTERED ([Id] ASC)
);

