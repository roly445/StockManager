CREATE PROCEDURE [Identity].[uspCheckForPresenceOfUserByNormalizedUserNameAndId]
		@normalizedUserName NVARCHAR(256)
	,	@userId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT u.Id
	FROM [Identity].ApplicationUser u
	WHERE 
			u.NormalizedUserName = @normalizedUserName
		AND	u.Id <> @userId
END