CREATE TABLE [Identity].[ApplicationUser]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [UserName] NVARCHAR(256) NOT NULL,
    [NormalizedUserName] NVARCHAR(256) NOT NULL,
    [PasswordHash] NVARCHAR(MAX) NOT NULL
)
 
GO
 
CREATE INDEX [IX_ApplicationUser_NormalizedUserName] ON [Identity].[ApplicationUser] ([NormalizedUserName])