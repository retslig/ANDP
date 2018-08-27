CREATE TABLE [ctc].[ProvisioningEngineOrderOrServiceActionTypesSettings] (
    [Id]                           INT           IDENTITY (1, 1) NOT NULL,
    [ProvisioningEngineSettingsId] INT           NOT NULL,
    [ActionTypeEnumId]             INT           NOT NULL,
    [CreatedByUser]                NVARCHAR (20) NOT NULL,
    [ModifiedByUser]               NVARCHAR (20) NOT NULL,
    [DateCreated]                  DATETIME2 (7) NOT NULL,
    [DateModified]                 DATETIME2 (7) NOT NULL,
    [Version]                      INT           NOT NULL,
    CONSTRAINT [PK_ProvisioningEngineOrderOrServiceActionTypesSettings] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ctc_ProvisioningEngineOrderOrServiceActionTypesSettings_ActionTypeEnum] FOREIGN KEY ([ActionTypeEnumId]) REFERENCES [ctc].[ActionTypeEnum] ([Id]),
    CONSTRAINT [FK_ctc_ProvisioningEngineOrderOrServiceActionTypesSettings_ProvisioningEngineSettings] FOREIGN KEY ([ProvisioningEngineSettingsId]) REFERENCES [ctc].[ProvisioningEngineSettings] ([Id])
);

