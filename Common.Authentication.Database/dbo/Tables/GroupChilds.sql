CREATE TABLE [dbo].[GroupChilds] (
    [Key]          INT              IDENTITY (1, 1) NOT NULL,
    [ParentKey]    INT              NOT NULL,
    [ChildGroupID] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_dbo.GroupChilds] PRIMARY KEY CLUSTERED ([Key] ASC),
    CONSTRAINT [FK_dbo.GroupChilds_dbo.Groups_ParentKey] FOREIGN KEY ([ParentKey]) REFERENCES [dbo].[Groups] ([Key]) ON DELETE CASCADE,
    CONSTRAINT [UK_ParentKey_ChildGroupID] UNIQUE NONCLUSTERED ([ParentKey] ASC, [ChildGroupID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ParentKey]
    ON [dbo].[GroupChilds]([ParentKey] ASC);

