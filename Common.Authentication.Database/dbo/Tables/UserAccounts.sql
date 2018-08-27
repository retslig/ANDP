CREATE TABLE [dbo].[UserAccounts] (
    [Key]                        INT              IDENTITY (1, 1) NOT NULL,
    [ID]                         UNIQUEIDENTIFIER NOT NULL,
    [Tenant]                     NVARCHAR (50)    NOT NULL,
    [Username]                   NVARCHAR (254)   NOT NULL,
    [Created]                    DATETIME         NOT NULL,
    [LastUpdated]                DATETIME         NOT NULL,
    [IsAccountClosed]            BIT              NOT NULL,
    [AccountClosed]              DATETIME         NULL,
    [IsLoginAllowed]             BIT              NOT NULL,
    [LastLogin]                  DATETIME         NULL,
    [LastFailedLogin]            DATETIME         NULL,
    [FailedLoginCount]           INT              NOT NULL,
    [PasswordChanged]            DATETIME         NULL,
    [RequiresPasswordReset]      BIT              NOT NULL,
    [Email]                      NVARCHAR (254)   NULL,
    [IsAccountVerified]          BIT              NOT NULL,
    [LastFailedPasswordReset]    DATETIME         NULL,
    [FailedPasswordResetCount]   INT              NOT NULL,
    [MobileCode]                 NVARCHAR (100)   NULL,
    [MobileCodeSent]             DATETIME         NULL,
    [MobilePhoneNumber]          NVARCHAR (20)    NULL,
    [MobilePhoneNumberChanged]   DATETIME         NULL,
    [AccountTwoFactorAuthMode]   INT              NOT NULL,
    [CurrentTwoFactorAuthStatus] INT              NOT NULL,
    [VerificationKey]            NVARCHAR (100)   NULL,
    [VerificationPurpose]        INT              NULL,
    [VerificationKeySent]        DATETIME         NULL,
    [VerificationStorage]        NVARCHAR (100)   NULL,
    [HashedPassword]             NVARCHAR (200)   NULL,
    CONSTRAINT [PK_dbo.UserAccounts] PRIMARY KEY CLUSTERED ([Key] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ID]
    ON [dbo].[UserAccounts]([ID] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Tenant_Username]
    ON [dbo].[UserAccounts]([Tenant] ASC, [Username] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Tenant_Email]
    ON [dbo].[UserAccounts]([Tenant] ASC, [Email] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_VerificationKey]
    ON [dbo].[UserAccounts]([VerificationKey] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Username]
    ON [dbo].[UserAccounts]([Username] ASC);

