CREATE TABLE [ctc].[EquipmentSettingsX] (
    [Id]                  INT           IDENTITY (1, 1) NOT NULL,
    [EquipmentSettingsId] INT           NOT NULL,
    [ItemType]            INT           NOT NULL,
    [MaxThreads]          BIT           NOT NULL,
    [CreatedByUser]       NVARCHAR (20) NOT NULL,
    [ModifiedByUser]      NVARCHAR (20) NOT NULL,
    [DateCreated]         DATETIME2 (7) NOT NULL,
    [DateModified]        DATETIME2 (7) NOT NULL,
    [Version]             INT           NOT NULL,
    [RecordModified]      DATETIME2 (7) NOT NULL,
    [Action]              CHAR (1)      NOT NULL,
    CONSTRAINT [PK_ctc_EquipmentSettingsX] PRIMARY KEY CLUSTERED ([Id] ASC)
);

