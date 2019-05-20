CREATE PROCEDURE [Identity].[uspCheckForPresenceOfUserByNormalizedUserName]
	@normalizedUserName NVARCHAR(256)
AS
BEGIN
	SELECT u.Id
	FROM [Identity].ApplicationUser u
	WHERE u.NormalizedUserName = @normalizedUserName
END