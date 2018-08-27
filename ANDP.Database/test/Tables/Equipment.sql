CREATE TABLE [test].[Equipment] (
    [Id]                       INT            IDENTITY (1, 1) NOT NULL,
    [ExternalEquipmentId]      NVARCHAR (36)  NOT NULL,
    [EquipmentSetupId]         INT            NOT NULL,
    [EquipmentItemId]          INT            NOT NULL,
    [EquipmentItemDescription] NVARCHAR (100) NULL,
    [ItemId]                   INT            NOT NULL,
    [Priority]                 INT            NOT NULL,
    [ProvisionSequence]        INT            NOT NULL,
    [Xml]                      XML            NOT NULL,
    [ProvisionDate]            DATETIME2 (7)  NOT NULL,
    [StatusTypeId]             INT            NOT NULL,
    [ActionTypeId]             INT            NOT NULL,
    [StartDate]                DATETIME2 (7)  NULL,
    [CompletionDate]           DATETIME2 (7)  NULL,
    [ResultMessage]            NVARCHAR (MAX) NULL,
    [Log]                      NVARCHAR (MAX) NULL,
    [CreatedByUser]            NVARCHAR (20)  NOT NULL,
    [ModifiedByUser]           NVARCHAR (20)  NOT NULL,
    [DateCreated]              DATETIME2 (7)  NOT NULL,
    [DateModified]             DATETIME2 (7)  NOT NULL,
    [Version]                  INT            NOT NULL,
    CONSTRAINT [PK_Equipment] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_test_Equipment_ActionType] FOREIGN KEY ([ActionTypeId]) REFERENCES [test].[ActionTypeEnum] ([Id]),
    CONSTRAINT [FK_test_Equipment_EquipmentSetup] FOREIGN KEY ([EquipmentSetupId]) REFERENCES [test].[EquipmentSetup] ([Id]),
    CONSTRAINT [FK_test_Equipment_Item] FOREIGN KEY ([ItemId]) REFERENCES [test].[Item] ([Id]),
    CONSTRAINT [FK_test_Equipment_StatusType] FOREIGN KEY ([StatusTypeId]) REFERENCES [test].[StatusTypeEnum] ([Id])
);


GO
CREATE STATISTICS [_test_dta_stat_382624406_3_12_6_11]
    ON [test].[Equipment]([EquipmentSetupId], [ActionTypeId], [ItemId], [StatusTypeId]);


GO
CREATE STATISTICS [_test_dta_stat_382624406_3_6_4]
    ON [test].[Equipment]([EquipmentSetupId], [ItemId], [EquipmentItemId]);


GO
CREATE STATISTICS [_test_dta_stat_382624406_4_3]
    ON [test].[Equipment]([EquipmentItemId], [EquipmentSetupId]);


GO
CREATE STATISTICS [_test_dta_stat_382624406_6_12]
    ON [test].[Equipment]([ItemId], [ActionTypeId]);


GO


CREATE TRIGGER [test].[T_IUD_test_Equipment] ON [test].[Equipment]
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
		INSERT INTO [test].[EquipmentX] 
		SELECT *, GETDATE(), @Action FROM CTE
	END    
	
	IF(@Action = 'I')
	BEGIN
		WITH CTE1 AS
		(
			SELECT I.Id AS Id, (CASE WHEN A.[Version] IS NULL THEN 1 ELSE MAX(A.[Version]) + 1 END) AS FinalVersion FROM inserted I
			INNER JOIN [test].[Equipment] A ON A.Id = I.Id
			GROUP BY I.Id, A.[Version]
		)
		UPDATE [test].[Equipment]
		SET [Version] = CTE1.FinalVersion
		FROM [test].[Equipment] Z
		INNER JOIN CTE1 ON CTE1.Id = Z.Id
	END
	
	IF(@Action = 'U')
	BEGIN
		WITH CTE2 AS
		(
			SELECT D.Id AS Id, (CASE WHEN A.[Version] IS NULL THEN 1 ELSE MAX(A.[Version]) + 1 END) AS FinalVersion FROM DELETED D
			INNER JOIN [test].[Equipment] A ON A.Id = D.Id
			GROUP BY D.Id, A.[Version]
		)
		UPDATE [test].[Equipment]
		SET [Version] = CTE2.FinalVersion
		FROM [test].[Equipment] Z
		INNER JOIN CTE2 ON CTE2.Id = Z.Id
	END
END




