CREATE TABLE [dbo].[LinkedAccountClaims] (
    [Key]               INT            IDENTITY (1, 1) NOT NULL,
    [ParentKey]         INT            NOT NULL,
    [ProviderName]      NVARCHAR (30)  NOT NULL,
    [ProviderAccountID] NVARCHAR (100) NOT NULL,
    [Type]              NVARCHAR (150) NOT NULL,
    [Value]             NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_dbo.LinkedAccountClaims] PRIMARY KEY CLUSTERED ([Key] ASC),
    CONSTRAINT [FK_dbo.LinkedAccountClaims_dbo.UserAccounts_ParentKey] FOREIGN KEY ([ParentKey]) REFERENCES [dbo].[UserAccounts] ([Key]) ON DELETE CASCADE,
    CONSTRAINT [UK_ParentKey_ProviderName_ProviderAccountID_Type_Value] UNIQUE NONCLUSTERED ([ParentKey] ASC, [ProviderName] ASC, [ProviderAccountID] ASC, [Type] ASC, [Value] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ParentKey]
    ON [dbo].[LinkedAccountClaims]([ParentKey] ASC);

