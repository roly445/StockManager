CREATE PROCEDURE [Identity].[uspGetUserByNormalizedUserName]
	@normalizedUserName NVARCHAR(256)
AS
BEGIN
	SELECT 
			Id
		,	UserName
		,	NormalizedUserName
		,	PasswordHash
	FROM [Identity].[ApplicationUser]
	WHERE NormalizedUserName = @normalizedUserName
END

