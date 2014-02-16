USE DHallPickerFinal
GO

CREATE PROCEDURE AddTodaysMenu(
	@DishName varchar(100), 
	@DiningHallID int, 
	@MealTypeID int, 
	@DateAdded date)
AS
	INSERT INTO Dish (DishName, DiningHallID, MealTypeID, DateAdded)
	VALUES (@DishName, @DiningHallID, @MealTypeID, @DateAdded)
GO

CREATE PROCEDURE SelectAllDishes(
	@DiningHallID int,
	@DateAdded date)
AS
	SELECT DishName, MealTypeID, DateAdded
	FROM Dish
	WHERE DiningHallID = @DiningHallID AND DateAdded = @DateAdded

GO

CREATE PROCEDURE SelectAllHalls
AS
	SELECT DiningHallID, DiningHallName, URL
	FROM DiningHall

GO

CREATE PROCEDURE SelectAllDiets
AS
	SELECT DishFilterID, FilterName, FilterDescription
	FROM DishFilters

GO

CREATE PROCEDURE SelectAllKeywords
AS
	SELECT ExclusionKeywordID, Keyword, DishFilterID
	FROM ExclusionKeywords

GO
