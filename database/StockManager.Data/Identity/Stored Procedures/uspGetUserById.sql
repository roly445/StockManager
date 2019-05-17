CREATE PROCEDURE [Identity].[uspGetUserById]
	@userId uniqueidentifier
AS
BEGIN
	SELECT 
			Id
		,	UserName
		,	NormalizedUserName
		,	PasswordHash
	FROM [Identity].[ApplicationUser]
	WHERE Id = @userId
END
