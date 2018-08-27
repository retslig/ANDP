CREATE TABLE [srtc].[ProvisioningEngineSchedules] (
    [Id]                           INT            IDENTITY (1, 1) NOT NULL,
    [ProvisioningEngineSettingsId] INT            NOT NULL,
    [Active]                       BIT            NOT NULL,
    [Name]                         NVARCHAR (100) NOT NULL,
    [Sunday]                       BIT            NOT NULL,
    [SundayStartTime]              TIME (7)       NOT NULL,
    [SundayEndtime]                TIME (7)       NOT NULL,
    [Monday]                       BIT            NOT NULL,
    [MondayStartTime]              TIME (7)       NOT NULL,
    [MondayEndtime]                TIME (7)       NOT NULL,
    [Tuesday]                      BIT            NOT NULL,
    [TuesdayStartTime]             TIME (7)       NOT NULL,
    [TuesdayEndtime]               TIME (7)       NOT NULL,
    [Wednesday]                    BIT            NOT NULL,
    [WednesdayStartTime]           TIME (7)       NOT NULL,
    [WednesdayEndtime]             TIME (7)       NOT NULL,
    [Thursday]                     BIT            NOT NULL,
    [ThursdayStartTime]            TIME (7)       NOT NULL,
    [ThursdayEndtime]              TIME (7)       NOT NULL,
    [Friday]                       BIT            NOT NULL,
    [FridayStartTime]              TIME (7)       NOT NULL,
    [FridayEndtime]                TIME (7)       NOT NULL,
    [Saturday]                     BIT            NOT NULL,
    [SaturdayStartTime]            TIME (7)       NOT NULL,
    [SaturdayEndtime]              TIME (7)       NOT NULL,
    [CreatedByUser]                NVARCHAR (20)  NOT NULL,
    [ModifiedByUser]               NVARCHAR (20)  NOT NULL,
    [DateCreated]                  DATETIME2 (7)  NOT NULL,
    [DateModified]                 DATETIME2 (7)  NOT NULL,
    [Version]                      INT            NOT NULL,
    CONSTRAINT [PK_ProvisioningEngineSchedules] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_srtc_ProvisioningEngineSchedules_ProvisioningEngineSettings] FOREIGN KEY ([ProvisioningEngineSettingsId]) REFERENCES [srtc].[ProvisioningEngineSettings] ([Id])
);




GO




CREATE TRIGGER [srtc].[T_IUD_srtc_ProvisioningEngineSchedules] ON [srtc].[ProvisioningEngineSchedules]
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
		INSERT INTO [srtc].[ProvisioningEngineSchedulesX] 
		SELECT *, GETDATE(), @Action FROM CTE
	END    
	
	IF(@Action = 'I')
	BEGIN
		WITH CTE1 AS
		(
			SELECT I.Id AS Id, (CASE WHEN A.[Version] IS NULL THEN 1 ELSE MAX(A.[Version]) + 1 END) AS FinalVersion FROM inserted I
			INNER JOIN [srtc].[ProvisioningEngineSchedules] A ON A.Id = I.Id
			GROUP BY I.Id, A.[Version]
		)
		UPDATE [srtc].[ProvisioningEngineSchedules]
		SET [Version] = CTE1.FinalVersion
		FROM [srtc].[ProvisioningEngineSchedules] Z
		INNER JOIN CTE1 ON CTE1.Id = Z.Id
	END
	
	IF(@Action = 'U')
	BEGIN
		WITH CTE2 AS
		(
			SELECT D.Id AS Id, (CASE WHEN A.[Version] IS NULL THEN 1 ELSE MAX(A.[Version]) + 1 END) AS FinalVersion FROM DELETED D
			INNER JOIN [srtc].[ProvisioningEngineSchedules] A ON A.Id = D.Id
			GROUP BY D.Id, A.[Version]
		)
		UPDATE [srtc].[ProvisioningEngineSchedules]
		SET [Version] = CTE2.FinalVersion
		FROM [srtc].[ProvisioningEngineSchedules] Z
		INNER JOIN CTE2 ON CTE2.Id = Z.Id
	END
END


