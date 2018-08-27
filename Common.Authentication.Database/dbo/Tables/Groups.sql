CREATE TABLE [dbo].[Groups] (
    [Key]         INT              IDENTITY (1, 1) NOT NULL,
    [ID]          UNIQUEIDENTIFIER NOT NULL,
    [Tenant]      NVARCHAR (50)    NOT NULL,
    [Name]        NVARCHAR (100)   NOT NULL,
    [Created]     DATETIME         NOT NULL,
    [LastUpdated] DATETIME         NOT NULL,
    CONSTRAINT [PK_dbo.Groups] PRIMARY KEY CLUSTERED ([Key] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ID]
    ON [dbo].[Groups]([ID] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Tenant_Name]
    ON [dbo].[Groups]([Tenant] ASC, [Name] ASC);

