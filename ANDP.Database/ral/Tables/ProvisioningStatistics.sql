CREATE TABLE [ral].[ProvisioningStatistics] (
    [Id]                    INT           IDENTITY (1, 1) NOT NULL,
    [ItemType]              INT           NOT NULL,
    [AverageProcessingTime] INT           NOT NULL,
    [Succussful]            INT           NOT NULL,
    [Failed]                INT           NOT NULL,
    [Total]                 INT           NOT NULL,
    [CreatedByUser]         NVARCHAR (20) NOT NULL,
    [ModifiedByUser]        NVARCHAR (20) NOT NULL,
    [DateCreated]           DATETIME2 (7) NOT NULL,
    [DateModified]          DATETIME2 (7) NOT NULL,
    [Version]               INT           NOT NULL,
    CONSTRAINT [PK_ProvisioningStatistics] PRIMARY KEY CLUSTERED ([Id] ASC)
);

