CREATE TABLE [ctc].[EquipmentSettings] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [ItemType]       INT           NOT NULL,
    [MaxThreads]     BIT           NOT NULL,
    [CreatedByUser]  NVARCHAR (20) NOT NULL,
    [ModifiedByUser] NVARCHAR (20) NOT NULL,
    [DateCreated]    DATETIME2 (7) NOT NULL,
    [DateModified]   DATETIME2 (7) NOT NULL,
    [Version]        INT           NOT NULL,
    CONSTRAINT [PK_EquipmentSettings] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO

CREATE TRIGGER [ctc].[T_IUD_ctc_EquipmentSettings] ON [ctc].[EquipmentSettings]
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
		INSERT INTO [ctc].[EquipmentSettingsX] 
		SELECT *, GETDATE(), @Action FROM CTE
	END    
	
	IF(@Action = 'I')
	BEGIN
		WITH CTE1 AS
		(
			SELECT I.Id AS Id, (CASE WHEN A.[Version] IS NULL THEN 1 ELSE MAX(A.[Version]) + 1 END) AS FinalVersion FROM inserted I
			INNER JOIN [ctc].[EquipmentSettings] A ON A.Id = I.Id
			GROUP BY I.Id, A.[Version]
		)
		UPDATE [ctc].[EquipmentSettings]
		SET [Version] = CTE1.FinalVersion
		FROM [ctc].[EquipmentSettings] Z
		INNER JOIN CTE1 ON CTE1.Id = Z.Id
	END
	
	IF(@Action = 'U')
	BEGIN
		WITH CTE2 AS
		(
			SELECT D.Id AS Id, (CASE WHEN A.[Version] IS NULL THEN 1 ELSE MAX(A.[Version]) + 1 END) AS FinalVersion FROM DELETED D
			INNER JOIN [ctc].[EquipmentSettings] A ON A.Id = D.Id
			GROUP BY D.Id, A.[Version]
		)
		UPDATE [ctc].[EquipmentSettings]
		SET [Version] = CTE2.FinalVersion
		FROM [ctc].[EquipmentSettings] Z
		INNER JOIN CTE2 ON CTE2.Id = Z.Id
	END
END


