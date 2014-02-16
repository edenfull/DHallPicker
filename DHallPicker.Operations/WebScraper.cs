using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DHallPicker.Data.Repositories;
using DHallPicker.Models.Models;
using Dapper;
using HtmlAgilityPack;

namespace DHallPicker.Operations.Operations
{
    public class WebScraper
    {
        public void Run()
        {
            DiningHallLookupResult result = new DiningHallLookupResult();
            PullHallDataFromDatabase(result);

            foreach (DiningHall scrapedHall in result.MenuData)
            {
                GetMenus(scrapedHall);
            }
        }

        public DiningHallLookupResult PullHallDataFromDatabase(DiningHallLookupResult result)
        {
            result.MenuData = new List<DiningHall>();
            var dataContext = new DiningHallRepository();

            foreach (DiningHall dHall in dataContext.SelectAllHalls())
            {
                DiningHall currentHall = new DiningHall();

                currentHall.DiningHallID = dHall.DiningHallID;
                currentHall.URL = dHall.URL;

                result.MenuData.Add(currentHall);
            }

            return result;
        }

        public void GetMenus(DiningHall scrapedHall)
        {
            var webGet = new HtmlWeb();
            var document = webGet.Load(scrapedHall.URL);

            List<KeyValuePair<int, string>> temporaryMenuItems = new List<KeyValuePair<int, string>>();

            if (document.DocumentNode.SelectSingleNode("//a[@name=\"Recipe_Desc\"]") != null)
            {
                foreach (HtmlNode node in document.DocumentNode.SelectNodes("//a[@name=\"Recipe_Desc\"]"))
                {
                    temporaryMenuItems.Add(new KeyValuePair<int, string>(node.Line, node.InnerText.Trim()));
                }

                foreach (HtmlNode node in document.DocumentNode.SelectNodes("//div[@id=\"menusampmeals\"]"))
                {
                    temporaryMenuItems.Add(new KeyValuePair<int, string>(node.Line, node.InnerText.Trim()));
                }

                temporaryMenuItems = temporaryMenuItems.OrderBy(x => x.Key).ToList();
            }

            SeparateMenus(temporaryMenuItems, scrapedHall);
        }

        private void SeparateMenus(List<KeyValuePair<int, string>> temporaryMenuItems, DiningHall scrapedHall)
        {
            string[] mealTimes = { "Breakfast", "Lunch", "Dinner" };
            int currentMealIndex = 0, numOfMeals = 0;

            foreach (KeyValuePair<int, string> pair in temporaryMenuItems)
            {
                if (mealTimes.Contains(pair.Value))
                    numOfMeals++;
            }

            if (numOfMeals == 2)
            {
                foreach (KeyValuePair<int, string> pair in temporaryMenuItems)
                {
                    if (mealTimes.Contains(pair.Value))
                        currentMealIndex++;
                    else
                    {
                        Dish currentDish = new Dish()
                        {
                            DishName = pair.Value
                        };

                        if (currentMealIndex == 1)
                            AddMealToDatabase(currentDish, scrapedHall, 2);

                        if (currentMealIndex == 2)
                            AddMealToDatabase(currentDish, scrapedHall, 3);
                    }
                }
            }

            if (numOfMeals == 3)
            {
                foreach (KeyValuePair<int, string> pair in temporaryMenuItems)
                {
                    if (mealTimes.Contains(pair.Value))
                        currentMealIndex++;
                    else
                    {
                        Dish currentDish = new Dish()
                        {
                            DishName = pair.Value
                        };

                        if (currentMealIndex == 1)
                            AddMealToDatabase(currentDish, scrapedHall, 1);

                        if (currentMealIndex == 2)
                            AddMealToDatabase(currentDish, scrapedHall, 2);

                        if (currentMealIndex == 3)
                            AddMealToDatabase(currentDish, scrapedHall, 3);
                    }
                }
            }
        }

        private void AddMealToDatabase(Dish currentDish, DiningHall scrapedHall, int mealTypeID)
        {
            using (var connection = new SqlConnection(Data.ConfigurationSettings.GetConnectionString()))
            {
                DynamicParameters dyn = new DynamicParameters();
                dyn.Add("@DishName", currentDish.DishName);
                dyn.Add("@DiningHallID", scrapedHall.DiningHallID);
                dyn.Add("@MealTypeID", mealTypeID);
                dyn.Add("@DateAdded", DateTime.Today);

                connection.Execute("AddTodaysMenu", dyn, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
