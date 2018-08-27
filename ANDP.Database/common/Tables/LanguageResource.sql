CREATE TABLE [common].[LanguageResource] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [ResourceType]  VARCHAR (50)   NOT NULL,
    [CultureCode]   VARCHAR (10)   NOT NULL,
    [ResourceKey]   VARCHAR (20)   NOT NULL,
    [ResourceValue] NVARCHAR (MAX) NOT NULL,
    [CreatedById]   INT            NOT NULL,
    [ModifiedById]  INT            NOT NULL,
    [DateCreated]   DATETIME2 (7)  NOT NULL,
    [DateModified]  DATETIME2 (7)  NOT NULL,
    [Version]       INT            NOT NULL,
    CONSTRAINT [PK_LanguageResource] PRIMARY KEY CLUSTERED ([Id] ASC)
);

