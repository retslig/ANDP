﻿CREATE TABLE [emp].[ServiceX] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [ServiceId]         INT            NOT NULL,
    [ExternalServiceId] NVARCHAR (36)  NOT NULL,
    [OrderId]           INT            NOT NULL,
    [Priority]          INT            NOT NULL,
    [ProvisionSequence] INT            NOT NULL,
    [Xml]               XML            NOT NULL,
    [ProvisionDate]     DATETIME2 (7)  NOT NULL,
    [StatusTypeId]      INT            NOT NULL,
    [ActionTypeId]      INT            NOT NULL,
    [ResultMessage]     NVARCHAR (MAX) NULL,
    [CompletionDate]    DATETIME2 (7)  NULL,
    [StartDate]         DATETIME2 (7)  NULL,
    [Log]               NVARCHAR (MAX) NULL,
    [CreatedByUser]     NVARCHAR (20)  NOT NULL,
    [ModifiedByUser]    NVARCHAR (20)  NOT NULL,
    [DateCreated]       DATETIME2 (7)  NOT NULL,
    [DateModified]      DATETIME2 (7)  NOT NULL,
    [Version]           INT            NULL,
    [RecordModified]    DATETIME2 (7)  NOT NULL,
    [Action]            CHAR (1)       NOT NULL,
    CONSTRAINT [PK_emp_ServiceX] PRIMARY KEY CLUSTERED ([Id] ASC)
);
