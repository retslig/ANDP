CREATE TABLE [gdo].[EquipmentTypeEnum] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_EquipmentTypeEnum] PRIMARY KEY CLUSTERED ([Id] ASC)
);

