CREATE TABLE [common].[Tenants] (
    [Guid]           UNIQUEIDENTIFIER NOT NULL,
    [Name]           NVARCHAR (50)    NOT NULL,
    [Schema]         NVARCHAR (36)    NOT NULL,
    [ExternalSchema] NVARCHAR (36)    NULL,
    CONSTRAINT [PK_Tenants] PRIMARY KEY CLUSTERED ([Guid] ASC),
    CONSTRAINT [UK_Tenants_Guid] UNIQUE NONCLUSTERED ([Guid] ASC),
    CONSTRAINT [UK_Tenants_Name] UNIQUE NONCLUSTERED ([Name] ASC),
    CONSTRAINT [UK_Tenants_Schema] UNIQUE NONCLUSTERED ([Schema] ASC)
);

