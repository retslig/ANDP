CREATE TABLE [srtc].[ProvisioningEngineSettingsX] (
    [Id]                           INT            IDENTITY (1, 1) NOT NULL,
    [ProvisioningEngineSettingsId] INT            NOT NULL,
    [CompanyId]                    INT            NOT NULL,
    [ScriptName]                   NVARCHAR (100) NOT NULL,
    [LoadBalancingActive]          BIT            NOT NULL,
    [FailOverActive]               BIT            NOT NULL,
    [ProvisioningInterval]         INT            NOT NULL,
    [MaxThreadsPerDispatcher]      INT            NOT NULL,
    [ProvisionByMethodTypeId]      INT            NOT NULL,
    [ProvisioningPaused]           BIT            NOT NULL,
    [CreatedByUser]                NVARCHAR (20)  NOT NULL,
    [ModifiedByUser]               NVARCHAR (20)  NOT NULL,
    [DateCreated]                  DATETIME2 (7)  NOT NULL,
    [DateModified]                 DATETIME2 (7)  NOT NULL,
    [Version]                      INT            NOT NULL,
    [RecordModified]               DATETIME2 (7)  NOT NULL,
    [Action]                       CHAR (1)       NOT NULL,
    CONSTRAINT [PK_ProvisioningEngineSettingsX] PRIMARY KEY CLUSTERED ([Id] ASC)
);

