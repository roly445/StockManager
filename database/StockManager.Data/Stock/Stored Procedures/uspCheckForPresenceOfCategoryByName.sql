CREATE PROCEDURE [Stock].[uspCheckForPresenceOfCategoryByName]
	@name NVARCHAR(100)
AS
BEGIN
	SELECT c.Id
	FROM Stock.Category c
	WHERE c.Name = @name
END
