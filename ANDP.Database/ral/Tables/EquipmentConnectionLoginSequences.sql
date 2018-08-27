CREATE TABLE [ral].[EquipmentConnectionLoginSequences] (
    [Id]                            INT            IDENTITY (1, 1) NOT NULL,
    [EquipmentConnectionSettingsId] INT            NOT NULL,
    [SequenceNumber]                INT            NOT NULL,
    [Command]                       NVARCHAR (100) NULL,
    [ExpectedResponse]              NVARCHAR (100) NULL,
    [Timeout]                       INT            NOT NULL,
    [CreatedByUser]                 NVARCHAR (20)  NOT NULL,
    [ModifiedByUser]                NVARCHAR (20)  NOT NULL,
    [DateCreated]                   DATETIME2 (7)  NOT NULL,
    [DateModified]                  DATETIME2 (7)  NOT NULL,
    [Version]                       INT            NOT NULL,
    CONSTRAINT [PK_EquipmentConnectionLoginSequences] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ral_EquipmentConnectionLoginSequences_EquipmentConnectionSettings] FOREIGN KEY ([EquipmentConnectionSettingsId]) REFERENCES [ral].[EquipmentConnectionSettings] ([Id]),
    CONSTRAINT [AK_ral_EquipmentConnectionLoginSequences_EquipmentConnectionSettingsId_SequenceNumber] UNIQUE NONCLUSTERED ([EquipmentConnectionSettingsId] ASC, [SequenceNumber] ASC)
);

