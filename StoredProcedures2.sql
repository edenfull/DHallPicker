USE DHallPicker
GO

SELECT MealTypeName, DishName, DiningHallName
FROM DiningHall dh 
	INNER JOIN Dish d ON dh.DiningHallID = d.DiningHallID
	INNER JOIN MealType mt on d.MealTypeID = mt.MealTypeID
WHERE d.DishName NOT LIKE (SELECT Keyword FROM ExclusionKeywords WHERE DishFilterID = 1)
