CREATE TABLE [gdo].[EquipmentEncodingTypeEnum] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_EquipmentEncodingTypeEnum] PRIMARY KEY CLUSTERED ([Id] ASC)
);

