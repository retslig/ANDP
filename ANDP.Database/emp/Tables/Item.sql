﻿CREATE TABLE [emp].[Item] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [ExternalItemId]    NVARCHAR (36)  NOT NULL,
    [ServiceId]         INT            NOT NULL,
    [Priority]          INT            NOT NULL,
    [ProvisionSequence] INT            NOT NULL,
    [Xml]               XML            NOT NULL,
    [ProvisionDate]     DATETIME2 (7)  NOT NULL,
    [StatusTypeId]      INT            NOT NULL,
    [ActionTypeId]      INT            NOT NULL,
    [ItemTypeId]        INT            NOT NULL,
    [ResultMessage]     NVARCHAR (MAX) NULL,
    [CompletionDate]    DATETIME2 (7)  NULL,
    [StartDate]         DATETIME2 (7)  NULL,
    [Log]               NVARCHAR (MAX) NULL,
    [CreatedByUser]     NVARCHAR (20)  NOT NULL,
    [ModifiedByUser]    NVARCHAR (20)  NOT NULL,
    [DateCreated]       DATETIME2 (7)  NOT NULL,
    [DateModified]      DATETIME2 (7)  NOT NULL,
    [Version]           INT            NULL,
    CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_emp_Item_ActionType] FOREIGN KEY ([ActionTypeId]) REFERENCES [emp].[ActionTypeEnum] ([Id]),
    CONSTRAINT [FK_emp_Item_ItemType] FOREIGN KEY ([ItemTypeId]) REFERENCES [emp].[ItemTypeEnum] ([Id]),
    CONSTRAINT [FK_emp_Item_Service] FOREIGN KEY ([ServiceId]) REFERENCES [emp].[Service] ([Id]),
    CONSTRAINT [FK_emp_Item_StatusType] FOREIGN KEY ([StatusTypeId]) REFERENCES [emp].[StatusTypeEnum] ([Id]),
    CONSTRAINT [uc_ExternalItemId_ServiceId] UNIQUE NONCLUSTERED ([ExternalItemId] ASC, [ServiceId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [_emp_dta_index_Item_8_270624007__K3_1_2_4_5_6_7_8_9_10_11_12_13_14_15_16_17_18_19]
    ON [emp].[Item]([ServiceId] ASC)
    INCLUDE([Id], [ExternalItemId], [Priority], [ProvisionSequence], [Xml], [ProvisionDate], [StatusTypeId], [ActionTypeId], [ItemTypeId], [ResultMessage], [CompletionDate], [StartDate], [Log], [CreatedByUser], [ModifiedByUser], [DateCreated], [DateModified], [Version]);


GO
CREATE STATISTICS [_emp_dta_stat_270624007_1_19]
    ON [emp].[Item]([Id], [Version]);


GO
CREATE STATISTICS [_emp_dta_stat_270624007_10_8_9_3_12]
    ON [emp].[Item]([ItemTypeId], [StatusTypeId], [ActionTypeId], [ServiceId], [CompletionDate]);


GO
CREATE STATISTICS [_emp_dta_stat_270624007_12_10_8_9]
    ON [emp].[Item]([CompletionDate], [ItemTypeId], [StatusTypeId], [ActionTypeId]);


GO
CREATE STATISTICS [_emp_dta_stat_270624007_3_10_8]
    ON [emp].[Item]([ServiceId], [ItemTypeId], [StatusTypeId]);


GO
CREATE STATISTICS [_emp_dta_stat_270624007_8_1_19]
    ON [emp].[Item]([StatusTypeId], [Id], [Version]);


GO



CREATE TRIGGER [emp].[T_IUD_emp_Item] ON [emp].[Item]
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
		INSERT INTO [emp].[ItemX] 
		SELECT *, GETDATE(), @Action FROM CTE
	END    
	
	IF(@Action = 'I')
	BEGIN
		WITH CTE1 AS
		(
			SELECT I.Id AS Id, (CASE WHEN A.[Version] IS NULL THEN 1 ELSE MAX(A.[Version]) + 1 END) AS FinalVersion FROM inserted I
			INNER JOIN [emp].[Item] A ON A.Id = I.Id
			GROUP BY I.Id, A.[Version]
		)
		UPDATE [emp].[Item]
		SET [Version] = CTE1.FinalVersion
		FROM [emp].[Item] Z
		INNER JOIN CTE1 ON CTE1.Id = Z.Id
	END
	
	IF(@Action = 'U')
	BEGIN
		WITH CTE2 AS
		(
			SELECT D.Id AS Id, (CASE WHEN A.[Version] IS NULL THEN 1 ELSE MAX(A.[Version]) + 1 END) AS FinalVersion FROM DELETED D
			INNER JOIN [emp].[Item] A ON A.Id = D.Id
			GROUP BY D.Id, A.[Version]
		)
		UPDATE [emp].[Item]
		SET [Version] = CTE2.FinalVersion
		FROM [emp].[Item] Z
		INNER JOIN CTE2 ON CTE2.Id = Z.Id
	END
END



