CREATE PROCEDURE [Stock].[uspCheckForPresenceOfTypeByCategoryAndName]
		@name NVARCHAR(100)
	,	@categoryId UNIQUEIDENTIFIER
AS
BEGIN
	SELECT t.Id
	FROM Stock.Type t
	WHERE
			t.CategoryId = @categoryId
		AND	t.Name = @name
END
