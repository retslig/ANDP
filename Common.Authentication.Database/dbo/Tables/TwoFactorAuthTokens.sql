CREATE TABLE [dbo].[TwoFactorAuthTokens] (
    [Key]       INT            IDENTITY (1, 1) NOT NULL,
    [ParentKey] INT            NOT NULL,
    [Token]     NVARCHAR (100) NOT NULL,
    [Issued]    DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.TwoFactorAuthTokens] PRIMARY KEY CLUSTERED ([Key] ASC),
    CONSTRAINT [FK_dbo.TwoFactorAuthTokens_dbo.UserAccounts_ParentKey] FOREIGN KEY ([ParentKey]) REFERENCES [dbo].[UserAccounts] ([Key]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ParentKey]
    ON [dbo].[TwoFactorAuthTokens]([ParentKey] ASC);

