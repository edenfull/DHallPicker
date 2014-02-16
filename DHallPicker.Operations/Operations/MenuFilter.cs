using System.Collections.Generic;
using DHallPicker.Data.Repositories;
using DHallPicker.Models.Models;

namespace DHallPicker.Operations.Operations
{
    public class MenuFilter
    {
        public DiningHallLookupResult GetRecommendations(DiningHallLookupResult nicelySortedData)
        {
            int currentBreakfastCount = 0, currentLunchCount = 0, currentDinnerCount = 0;

            foreach (DiningHall dHall in nicelySortedData.MenuData)
            {
                if (dHall.Breakfast.Count > currentBreakfastCount)
                {
                    currentBreakfastCount = dHall.Breakfast.Count;
                    nicelySortedData.RecommendedBreakfast = dHall.DiningHallName;
                }
                if (dHall.Lunch.Count > currentLunchCount)
                {
                    currentLunchCount = dHall.Lunch.Count;
                    nicelySortedData.RecommendedLunch = dHall.DiningHallName;
                }
                if (dHall.Dinner.Count > currentDinnerCount)
                {
                    currentDinnerCount = dHall.Dinner.Count;
                    nicelySortedData.RecommendedDinner = dHall.DiningHallName;
                }
            }

            return nicelySortedData;
        }

        public DiningHallLookupResult GetFilteredMenus(DiningHallLookupResult originalData, int[] selectedDiets)
        {
            var dataContext = new KeywordRepository();
            List<DishKeyword> allKeywords = dataContext.SelectFilteredKeywords(selectedDiets);

            List<string> keywordsAsStrings = new List<string>();
            foreach (DishKeyword menuDish in allKeywords)
            {
                keywordsAsStrings.Add(menuDish.Keyword.ToLower());
            }

            foreach (DiningHall dHall in originalData.MenuData)
            {
                foreach (string keyword in keywordsAsStrings)
                {
                    dHall.Breakfast.RemoveAll(x => HasSubstring(x, keyword));
                    dHall.Lunch.RemoveAll(x => HasSubstring(x, keyword));
                    dHall.Dinner.RemoveAll(x => HasSubstring(x, keyword));
                }
            }

            return originalData;
        }

        private bool HasSubstring(Dish dish, string keyword)
        {
            return dish.DishName.ToLower().IndexOf(keyword) >= 0;
        }
    }
}
