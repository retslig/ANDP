CREATE TABLE [test].[UsocToCommandTranslationX] (
    [Id]                         INT            IDENTITY (1, 1) NOT NULL,
    [UsocToCommandTranslationId] INT            NOT NULL,
    [CompanyId]                  INT            NOT NULL,
    [EquipmentId]                INT            NOT NULL,
    [UsocName]                   NVARCHAR (100) NOT NULL,
    [AddCommand]                 NVARCHAR (500) NOT NULL,
    [DeleteCommand]              NVARCHAR (500) NOT NULL,
    [Active]                     BIT            NOT NULL,
    [CreatedByUser]              NVARCHAR (20)  NOT NULL,
    [ModifiedByUser]             NVARCHAR (20)  NOT NULL,
    [DateCreated]                DATETIME2 (7)  NOT NULL,
    [DateModified]               DATETIME2 (7)  NOT NULL,
    [Version]                    INT            NOT NULL,
    [RecordModified]             DATETIME2 (7)  NOT NULL,
    [Action]                     CHAR (1)       NOT NULL,
    CONSTRAINT [PK_test_UsocToCommandTranslationX] PRIMARY KEY CLUSTERED ([Id] ASC)
);

