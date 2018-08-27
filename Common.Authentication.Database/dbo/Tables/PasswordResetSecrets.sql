CREATE TABLE [dbo].[PasswordResetSecrets] (
    [Key]                   INT              IDENTITY (1, 1) NOT NULL,
    [ParentKey]             INT              NOT NULL,
    [PasswordResetSecretID] UNIQUEIDENTIFIER NOT NULL,
    [Question]              NVARCHAR (150)   NOT NULL,
    [Answer]                NVARCHAR (150)   NOT NULL,
    CONSTRAINT [PK_dbo.PasswordResetSecrets] PRIMARY KEY CLUSTERED ([Key] ASC),
    CONSTRAINT [FK_dbo.PasswordResetSecrets_dbo.UserAccounts_ParentKey] FOREIGN KEY ([ParentKey]) REFERENCES [dbo].[UserAccounts] ([Key]) ON DELETE CASCADE,
    CONSTRAINT [UK_ParentKey_Question] UNIQUE NONCLUSTERED ([ParentKey] ASC, [Question] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ParentKey]
    ON [dbo].[PasswordResetSecrets]([ParentKey] ASC);

