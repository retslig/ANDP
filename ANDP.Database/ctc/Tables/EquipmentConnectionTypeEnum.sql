CREATE TABLE [ctc].[EquipmentConnectionTypeEnum] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_EquipmentConnectionTypeEnum] PRIMARY KEY CLUSTERED ([Id] ASC)
);

