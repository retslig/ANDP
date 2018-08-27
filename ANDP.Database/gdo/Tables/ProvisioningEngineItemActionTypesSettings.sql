CREATE TABLE [gdo].[ProvisioningEngineItemActionTypesSettings] (
    [Id]                           INT           IDENTITY (1, 1) NOT NULL,
    [ProvisioningEngineSettingsId] INT           NOT NULL,
    [ActionTypeEnumId]             INT           NOT NULL,
    [ItemTypeEnumId]               INT           NOT NULL,
    [CreatedByUser]                NVARCHAR (20) NOT NULL,
    [ModifiedByUser]               NVARCHAR (20) NOT NULL,
    [DateCreated]                  DATETIME2 (7) NOT NULL,
    [DateModified]                 DATETIME2 (7) NOT NULL,
    [Version]                      INT           NOT NULL,
    CONSTRAINT [PK_ProvisioningEngineItemActionTypesSettings] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_gdo_ProvisioningEngineItemActionTypesSettings_ActionTypeEnum] FOREIGN KEY ([ActionTypeEnumId]) REFERENCES [gdo].[ActionTypeEnum] ([Id]),
    CONSTRAINT [FK_gdo_ProvisioningEngineItemActionTypesSettings_ItemTypeEnum] FOREIGN KEY ([ItemTypeEnumId]) REFERENCES [gdo].[ItemTypeEnum] ([Id]),
    CONSTRAINT [FK_gdo_ProvisioningEngineItemActionTypesSettings_ProvisioningEngineSettings] FOREIGN KEY ([ProvisioningEngineSettingsId]) REFERENCES [gdo].[ProvisioningEngineSettings] ([Id])
);

