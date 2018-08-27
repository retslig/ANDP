CREATE TABLE [test].[EquipmentSetupX] (
    [Id]                            INT           IDENTITY (1, 1) NOT NULL,
    [EquipmentSetupId]              INT           NOT NULL,
    [Name]                          NVARCHAR (50) NOT NULL,
    [EquipmentTypeId]               INT           NOT NULL,
    [CompanyId]                     INT           NOT NULL,
    [EquipmentConnectionSettingsId] INT           NOT NULL,
    [CreatedByUser]                 NVARCHAR (20) NOT NULL,
    [ModifiedByUser]                NVARCHAR (20) NOT NULL,
    [DateCreated]                   DATETIME2 (7) NOT NULL,
    [DateModified]                  DATETIME2 (7) NOT NULL,
    [Version]                       INT           NOT NULL,
    [EquipmentVersion]              INT           NULL,
    [RecordModified]                DATETIME2 (7) NOT NULL,
    [Action]                        CHAR (1)      NOT NULL,
    CONSTRAINT [PK_test_EquipmentSetupX] PRIMARY KEY CLUSTERED ([Id] ASC)
);

