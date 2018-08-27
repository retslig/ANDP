﻿CREATE TABLE [gdo].[EquipmentSetup] (
    [Id]                            INT           IDENTITY (1, 1) NOT NULL,
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
    CONSTRAINT [PK_EquipmentSetup] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_gdo_EquipmentSetup_Companies] FOREIGN KEY ([CompanyId]) REFERENCES [gdo].[Companies] ([Id]),
    CONSTRAINT [FK_gdo_EquipmentSetup_EquipmentConnectionSettings] FOREIGN KEY ([EquipmentConnectionSettingsId]) REFERENCES [gdo].[EquipmentConnectionSettings] ([Id]),
    CONSTRAINT [FK_gdo_EquipmentSetup_EquipmentTypeId] FOREIGN KEY ([EquipmentTypeId]) REFERENCES [gdo].[EquipmentTypeEnum] ([Id])
);


GO

CREATE TRIGGER [gdo].[T_IUD_gdo_EquipmentSetup] ON [gdo].[EquipmentSetup]
AFTER INSERT, UPDATE, DELETE
AS BEGIN

	--Determine if this is an INSERT, UPDATE, or DELETE Action or a "failed delete".
	DECLARE @Action as char(1);
	SET @Action = (CASE WHEN EXISTS(SELECT Id FROM INSERTED)
						 AND EXISTS(SELECT Id FROM DELETED)
						THEN 'U'  -- Set Action to Updated.
						WHEN EXISTS(SELECT Id FROM INSERTED)
						THEN 'I'  -- Set Action to Insert.
						WHEN EXISTS(SELECT Id FROM DELETED)
						THEN 'D'  -- Set Action to Deleted.
						ELSE NULL -- Skip. It may have been a "failed delete".   
					END)

	IF(@Action = 'D' OR @Action = 'U')
	BEGIN
		WITH CTE AS
		(
			SELECT * FROM DELETED
		)
		INSERT INTO [gdo].[EquipmentSetupX] 
		SELECT *, GETDATE(), @Action FROM CTE
	END    
	
	IF(@Action = 'I')
	BEGIN
		WITH CTE1 AS
		(
			SELECT I.Id AS Id, (CASE WHEN A.[Version] IS NULL THEN 1 ELSE MAX(A.[Version]) + 1 END) AS FinalVersion FROM inserted I
			INNER JOIN [gdo].[EquipmentSetup] A ON A.Id = I.Id
			GROUP BY I.Id, A.[Version]
		)
		UPDATE [gdo].[EquipmentSetup]
		SET [Version] = CTE1.FinalVersion
		FROM [gdo].[EquipmentSetup] Z
		INNER JOIN CTE1 ON CTE1.Id = Z.Id
	END
	
	IF(@Action = 'U')
	BEGIN
		WITH CTE2 AS
		(
			SELECT D.Id AS Id, (CASE WHEN A.[Version] IS NULL THEN 1 ELSE MAX(A.[Version]) + 1 END) AS FinalVersion FROM DELETED D
			INNER JOIN [gdo].[EquipmentSetup] A ON A.Id = D.Id
			GROUP BY D.Id, A.[Version]
		)
		UPDATE [gdo].[EquipmentSetup]
		SET [Version] = CTE2.FinalVersion
		FROM [gdo].[EquipmentSetup] Z
		INNER JOIN CTE2 ON CTE2.Id = Z.Id
	END
END



GO
DISABLE TRIGGER [gdo].[T_IUD_gdo_EquipmentSetup]
    ON [gdo].[EquipmentSetup];

