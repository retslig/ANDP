CREATE TABLE [ral].[DataDictionary] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [CompanyId]      INT            NOT NULL,
    [EquipmentId]    INT            NOT NULL,
    [Key1]           NVARCHAR (100) NOT NULL,
    [Key2]           NVARCHAR (100) NOT NULL,
    [Key3]           NVARCHAR (100) NOT NULL,
    [Key4]           NVARCHAR (100) NOT NULL,
    [Key5]           NVARCHAR (100) NOT NULL,
    [Key6]           NVARCHAR (100) NOT NULL,
    [Key7]           NVARCHAR (100) NOT NULL,
    [Key8]           NVARCHAR (100) NOT NULL,
    [Key9]           NVARCHAR (100) NOT NULL,
    [Value1]         NVARCHAR (100) NOT NULL,
    [Value2]         NVARCHAR (100) NOT NULL,
    [Value3]         NVARCHAR (100) NOT NULL,
    [Value4]         NVARCHAR (100) NOT NULL,
    [Value5]         NVARCHAR (100) NOT NULL,
    [Value6]         NVARCHAR (100) NOT NULL,
    [Value7]         NVARCHAR (100) NOT NULL,
    [Value8]         NVARCHAR (100) NOT NULL,
    [Value9]         NVARCHAR (100) NOT NULL,
    [Active]         BIT            NOT NULL,
    [CreatedByUser]  NVARCHAR (20)  NOT NULL,
    [ModifiedByUser] NVARCHAR (20)  NOT NULL,
    [DateCreated]    DATETIME2 (7)  NOT NULL,
    [DateModified]   DATETIME2 (7)  NOT NULL,
    [Version]        INT            NOT NULL,
    CONSTRAINT [PK_DataDictionary] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ral_DataDictionary_Companies] FOREIGN KEY ([CompanyId]) REFERENCES [ral].[Companies] ([Id]),
    CONSTRAINT [FK_ral_DataDictionary_Equipment] FOREIGN KEY ([EquipmentId]) REFERENCES [ral].[EquipmentSetup] ([Id])
);




GO

CREATE TRIGGER [ral].[T_IUD_ral_DataDictionary] ON [ral].[DataDictionary]
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
		INSERT INTO [ral].[DataDictionaryX] 
		SELECT *, GETDATE(), @Action FROM CTE
	END    
	
	IF(@Action = 'I')
	BEGIN
		WITH CTE1 AS
		(
			SELECT I.Id AS Id, (CASE WHEN A.[Version] IS NULL THEN 1 ELSE MAX(A.[Version]) + 1 END) AS FinalVersion FROM inserted I
			INNER JOIN [ral].[DataDictionary] A ON A.Id = I.Id
			GROUP BY I.Id, A.[Version]
		)
		UPDATE [ral].[DataDictionary]
		SET [Version] = CTE1.FinalVersion
		FROM [ral].[DataDictionary] Z
		INNER JOIN CTE1 ON CTE1.Id = Z.Id
	END
	
	IF(@Action = 'U')
	BEGIN
		WITH CTE2 AS
		(
			SELECT D.Id AS Id, (CASE WHEN A.[Version] IS NULL THEN 1 ELSE MAX(A.[Version]) + 1 END) AS FinalVersion FROM DELETED D
			INNER JOIN [ral].[DataDictionary] A ON A.Id = D.Id
			GROUP BY D.Id, A.[Version]
		)
		UPDATE [ral].[DataDictionary]
		SET [Version] = CTE2.FinalVersion
		FROM [ral].[DataDictionary] Z
		INNER JOIN CTE2 ON CTE2.Id = Z.Id
	END
END




GO
DISABLE TRIGGER [ral].[T_IUD_ral_DataDictionary]
    ON [ral].[DataDictionary];

