CREATE TABLE [ral].[Order] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [ExternalOrderId]   NVARCHAR (36)  NOT NULL,
    [ExternalAccountId] NVARCHAR (36)  NOT NULL,
    [ExternalCompanyId] NVARCHAR (36)  NOT NULL,
    [Priority]          INT            NOT NULL,
    [Xml]               XML            NOT NULL,
    [ProvisionDate]     DATETIME2 (7)  NOT NULL,
    [StatusTypeId]      INT            NOT NULL,
    [OrginatingIp]      NVARCHAR (15)  NULL,
    [ActionTypeId]      INT            NOT NULL,
    [ResultMessage]     NVARCHAR (MAX) NULL,
    [CompletionDate]    DATETIME2 (7)  NULL,
    [StartDate]         DATETIME2 (7)  NULL,
    [Log]               NVARCHAR (MAX) NULL,
    [ResponseSent]      BIT            NOT NULL,
    [CreatedByUser]     NVARCHAR (20)  NOT NULL,
    [ModifiedByUser]    NVARCHAR (20)  NOT NULL,
    [DateCreated]       DATETIME2 (7)  NOT NULL,
    [DateModified]      DATETIME2 (7)  NOT NULL,
    [Version]           INT            NOT NULL,
    CONSTRAINT [PK_ProvisionOrder] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ral_Order_ActionType] FOREIGN KEY ([ActionTypeId]) REFERENCES [ral].[ActionTypeEnum] ([Id]),
    CONSTRAINT [FK_ral_Order_StatusType] FOREIGN KEY ([StatusTypeId]) REFERENCES [ral].[StatusTypeEnum] ([Id]),
    CONSTRAINT [UK_Order_ExternalOrderId] UNIQUE NONCLUSTERED ([ExternalOrderId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [_ral_dta_index_Order_8_1109578991__K8_K10_K4_K5_K7_K18_1_2_3_6_9_11_12_13_14_15_16_17_19_20]
    ON [ral].[Order]([StatusTypeId] ASC, [ActionTypeId] ASC, [ExternalCompanyId] ASC, [Priority] ASC, [ProvisionDate] ASC, [DateCreated] ASC)
    INCLUDE([Id], [ExternalOrderId], [ExternalAccountId], [Xml], [OrginatingIp], [ResultMessage], [CompletionDate], [StartDate], [Log], [ResponseSent], [CreatedByUser], [ModifiedByUser], [DateModified], [Version]);


GO
CREATE STATISTICS [_ral_dta_stat_1109578991_1_20_10_4]
    ON [ral].[Order]([Id], [Version], [ActionTypeId], [ExternalCompanyId]);


GO
CREATE STATISTICS [_ral_dta_stat_1109578991_10_4]
    ON [ral].[Order]([ActionTypeId], [ExternalCompanyId]);


GO
CREATE STATISTICS [_ral_dta_stat_1109578991_10_4_8_1_20]
    ON [ral].[Order]([ActionTypeId], [ExternalCompanyId], [StatusTypeId], [Id], [Version]);


GO
CREATE STATISTICS [_ral_dta_stat_1109578991_18]
    ON [ral].[Order]([DateCreated]);


GO
CREATE STATISTICS [_ral_dta_stat_1109578991_2_1]
    ON [ral].[Order]([ExternalOrderId], [Id]);


GO
CREATE STATISTICS [_ral_dta_stat_1109578991_4_8]
    ON [ral].[Order]([ExternalCompanyId], [StatusTypeId]);


GO
CREATE STATISTICS [_ral_dta_stat_1109578991_5_7_18_8_10]
    ON [ral].[Order]([Priority], [ProvisionDate], [DateCreated], [StatusTypeId], [ActionTypeId]);


GO
CREATE STATISTICS [_ral_dta_stat_1109578991_8_1_20]
    ON [ral].[Order]([StatusTypeId], [Id], [Version]);


GO
CREATE STATISTICS [_ral_dta_stat_1109578991_8_5_7]
    ON [ral].[Order]([StatusTypeId], [Priority], [ProvisionDate]);


GO




CREATE TRIGGER [ral].[T_IUD_ral_Order] ON [ral].[Order]
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
		INSERT INTO [ral].[OrderX] 
		SELECT *, GETDATE(), @Action FROM CTE
	END    
	
	IF(@Action = 'I')
	BEGIN
		WITH CTE1 AS
		(
			SELECT I.Id AS Id, (CASE WHEN A.[Version] IS NULL THEN 1 ELSE MAX(A.[Version]) + 1 END) AS FinalVersion FROM inserted I
			INNER JOIN [ral].[Order] A ON A.Id = I.Id
			GROUP BY I.Id, A.[Version]
		)
		UPDATE [ral].[Order]
		SET [Version] = CTE1.FinalVersion
		FROM [ral].[Order] Z
		INNER JOIN CTE1 ON CTE1.Id = Z.Id
	END
	
	IF(@Action = 'U')
	BEGIN
		WITH CTE2 AS
		(
			SELECT D.Id AS Id, (CASE WHEN A.[Version] IS NULL THEN 1 ELSE MAX(A.[Version]) + 1 END) AS FinalVersion FROM DELETED D
			INNER JOIN [ral].[Order] A ON A.Id = D.Id
			GROUP BY D.Id, A.[Version]
		)
		UPDATE [ral].[Order]
		SET [Version] = CTE2.FinalVersion
		FROM [ral].[Order] Z
		INNER JOIN CTE2 ON CTE2.Id = Z.Id
	END
END





