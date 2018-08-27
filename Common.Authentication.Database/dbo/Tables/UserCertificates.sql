CREATE TABLE [dbo].[UserCertificates] (
    [Key]        INT            IDENTITY (1, 1) NOT NULL,
    [ParentKey]  INT            NOT NULL,
    [Thumbprint] NVARCHAR (150) NOT NULL,
    [Subject]    NVARCHAR (250) NULL,
    CONSTRAINT [PK_dbo.UserCertificates] PRIMARY KEY CLUSTERED ([Key] ASC),
    CONSTRAINT [FK_dbo.UserCertificates_dbo.UserAccounts_ParentKey] FOREIGN KEY ([ParentKey]) REFERENCES [dbo].[UserAccounts] ([Key]) ON DELETE CASCADE,
    CONSTRAINT [UK_ParentKey_Thumbprint] UNIQUE NONCLUSTERED ([ParentKey] ASC, [Thumbprint] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ParentKey]
    ON [dbo].[UserCertificates]([ParentKey] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Thumbprint]
    ON [dbo].[UserCertificates]([Thumbprint] ASC);

