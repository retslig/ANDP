CREATE TABLE [common].[ApplicationLogs] (
    [LogID]                  INT              IDENTITY (1, 1) NOT NULL,
    [TenantId]               NVARCHAR (50)    NULL,
    [SearchKey]              UNIQUEIDENTIFIER NULL,
    [SourceMachineName]      VARCHAR (50)     NULL,
    [AppCode]                VARCHAR (50)     NOT NULL,
    [PID]                    INT              NOT NULL,
    [UserCode]               VARCHAR (20)     NOT NULL,
    [LoggedDateTime]         DATETIME2 (7)    NOT NULL,
    [MessageType]            VARCHAR (20)     NOT NULL,
    [MessageData]            XML              NULL,
    [StackTrace]             VARCHAR (2000)   NULL,
    [ExceptionMessage]       VARCHAR (2000)   NULL,
    [DestinationMachineName] VARCHAR (50)     NULL,
    CONSTRAINT [PK_ApplicationLog] PRIMARY KEY CLUSTERED ([LogID] ASC)
);

