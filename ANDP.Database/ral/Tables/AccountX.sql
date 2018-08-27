CREATE TABLE [ral].[AccountX] (
    [Id]                     INT           IDENTITY (1, 1) NOT NULL,
    [AccountId]              INT           NOT NULL,
    [ExternalAccountId]      NVARCHAR (36) NOT NULL,
    [ExternalAccountGroupId] NVARCHAR (36) NULL,
    [CompanyId]              INT           NOT NULL,
    [StatusTypeId]           INT           NOT NULL,
    [CreatedByUser]          NVARCHAR (20) NOT NULL,
    [ModifiedByUser]         NVARCHAR (20) NOT NULL,
    [DateCreated]            DATETIME2 (7) NOT NULL,
    [DateModified]           DATETIME2 (7) NOT NULL,
    [Version]                INT           NOT NULL,
    [RecordModified]         DATETIME2 (7) NOT NULL,
    [Action]                 CHAR (1)      NOT NULL,
    CONSTRAINT [PK_ral_AccountX] PRIMARY KEY CLUSTERED ([Id] ASC)
);

