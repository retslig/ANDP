CREATE TABLE [srtc].[ProvisioningEngineOrderOrServiceActionTypesSettings] (
    [Id]                           INT           IDENTITY (1, 1) NOT NULL,
    [ProvisioningEngineSettingsId] INT           NOT NULL,
    [ActionTypeEnumId]             INT           NOT NULL,
    [CreatedByUser]                NVARCHAR (20) NOT NULL,
    [ModifiedByUser]               NVARCHAR (20) NOT NULL,
    [DateCreated]                  DATETIME2 (7) NOT NULL,
    [DateModified]                 DATETIME2 (7) NOT NULL,
    [Version]                      INT           NOT NULL,
    CONSTRAINT [PK_ProvisioningEngineOrderOrServiceActionTypesSettings] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_srtc_ProvisioningEngineOrderOrServiceActionTypesSettings_ActionTypeEnum] FOREIGN KEY ([ActionTypeEnumId]) REFERENCES [srtc].[ActionTypeEnum] ([Id]),
    CONSTRAINT [FK_srtc_ProvisioningEngineOrderOrServiceActionTypesSettings_ProvisioningEngineSettings] FOREIGN KEY ([ProvisioningEngineSettingsId]) REFERENCES [srtc].[ProvisioningEngineSettings] ([Id])
);

