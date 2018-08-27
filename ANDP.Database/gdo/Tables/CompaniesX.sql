CREATE TABLE [gdo].[CompaniesX] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [CompanyId]         INT           NOT NULL,
    [Name]              NVARCHAR (50) NOT NULL,
    [ExternalCompanyId] NVARCHAR (36) NOT NULL,
    [CreatedByUser]     NVARCHAR (20) NOT NULL,
    [ModifiedByUser]    NVARCHAR (20) NOT NULL,
    [DateCreated]       DATETIME2 (7) NOT NULL,
    [DateModified]      DATETIME2 (7) NOT NULL,
    [Version]           INT           NOT NULL,
    [RecordModified]    DATETIME2 (7) NOT NULL,
    [Action]            CHAR (1)      NOT NULL,
    CONSTRAINT [PK_gdo_CompaniesX] PRIMARY KEY CLUSTERED ([Id] ASC)
);

