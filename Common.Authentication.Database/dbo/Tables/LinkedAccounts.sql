CREATE TABLE [dbo].[LinkedAccounts] (
    [Key]               INT            IDENTITY (1, 1) NOT NULL,
    [ParentKey]         INT            NOT NULL,
    [ProviderName]      NVARCHAR (30)  NOT NULL,
    [ProviderAccountID] NVARCHAR (100) NOT NULL,
    [LastLogin]         DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.LinkedAccounts] PRIMARY KEY CLUSTERED ([Key] ASC),
    CONSTRAINT [FK_dbo.LinkedAccounts_dbo.UserAccounts_ParentKey] FOREIGN KEY ([ParentKey]) REFERENCES [dbo].[UserAccounts] ([Key]) ON DELETE CASCADE,
    CONSTRAINT [UK_ParentKey_ProviderName_ProviderAccountID] UNIQUE NONCLUSTERED ([ParentKey] ASC, [ProviderName] ASC, [ProviderAccountID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ParentKey]
    ON [dbo].[LinkedAccounts]([ParentKey] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProviderName_ProviderAccountID]
    ON [dbo].[LinkedAccounts]([ProviderName] ASC, [ProviderAccountID] ASC);

