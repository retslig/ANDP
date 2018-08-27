CREATE TABLE [dbo].[UserClaims] (
    [Key]       INT            IDENTITY (1, 1) NOT NULL,
    [ParentKey] INT            NOT NULL,
    [Type]      NVARCHAR (150) NOT NULL,
    [Value]     NVARCHAR (150) NOT NULL,
    CONSTRAINT [PK_dbo.UserClaims] PRIMARY KEY CLUSTERED ([Key] ASC),
    CONSTRAINT [FK_dbo.UserClaims_dbo.UserAccounts_ParentKey] FOREIGN KEY ([ParentKey]) REFERENCES [dbo].[UserAccounts] ([Key]) ON DELETE CASCADE,
    CONSTRAINT [UK_ParentKey_Type_Value] UNIQUE NONCLUSTERED ([ParentKey] ASC, [Type] ASC, [Value] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ParentKey]
    ON [dbo].[UserClaims]([ParentKey] ASC);

