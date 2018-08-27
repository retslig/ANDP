CREATE TABLE [test].[EquipmentAuthenticationTypeEnum] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_EquipmentAuthenticationTypeEnum] PRIMARY KEY CLUSTERED ([Id] ASC)
);

