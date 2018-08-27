﻿CREATE TABLE [ctc].[ProvisioningEngineSchedulesX] (
    [Id]                            INT            IDENTITY (1, 1) NOT NULL,
    [ProvisioningEngineSchedulesId] INT            NOT NULL,
    [ProvisioningEngineSettingsId]  INT            NOT NULL,
    [Active]                        BIT            NOT NULL,
    [Name]                          NVARCHAR (100) NOT NULL,
    [Sunday]                        BIT            NOT NULL,
    [SundayStartTime]               TIME (7)       NOT NULL,
    [SundayEndtime]                 TIME (7)       NOT NULL,
    [Monday]                        BIT            NOT NULL,
    [MondayStartTime]               TIME (7)       NOT NULL,
    [MondayEndtime]                 TIME (7)       NOT NULL,
    [Tuesday]                       BIT            NOT NULL,
    [TuesdayStartTime]              TIME (7)       NOT NULL,
    [TuesdayEndtime]                TIME (7)       NOT NULL,
    [Wednesday]                     BIT            NOT NULL,
    [WednesdayStartTime]            TIME (7)       NOT NULL,
    [WednesdayEndtime]              TIME (7)       NOT NULL,
    [Thursday]                      BIT            NOT NULL,
    [ThursdayStartTime]             TIME (7)       NOT NULL,
    [ThursdayEndtime]               TIME (7)       NOT NULL,
    [Friday]                        BIT            NOT NULL,
    [FridayStartTime]               TIME (7)       NOT NULL,
    [FridayEndtime]                 TIME (7)       NOT NULL,
    [Saturday]                      BIT            NOT NULL,
    [SaturdayStartTime]             TIME (7)       NOT NULL,
    [SaturdayEndtime]               TIME (7)       NOT NULL,
    [CreatedByUser]                 NVARCHAR (20)  NOT NULL,
    [ModifiedByUser]                NVARCHAR (20)  NOT NULL,
    [DateCreated]                   DATETIME2 (7)  NOT NULL,
    [DateModified]                  DATETIME2 (7)  NOT NULL,
    [Version]                       INT            NOT NULL,
    [RecordModified]                DATETIME2 (7)  NOT NULL,
    [Action]                        CHAR (1)       NOT NULL,
    CONSTRAINT [PK_ctc_ProvisioningEngineSchedulesX] PRIMARY KEY CLUSTERED ([Id] ASC)
);

