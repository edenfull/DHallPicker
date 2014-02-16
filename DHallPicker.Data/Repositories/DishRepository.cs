using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DHallPicker.Models.Models;
using Dapper;

namespace DHallPicker.Data.Repositories
{
    public class DishRepository
    {
        public bool CheckIfTodayInDatabase(DiningHallLookupResult nicelySortedData)
        {
            foreach (DiningHall dHall in nicelySortedData.MenuData)
            {
                using (var connection = new SqlConnection(ConfigurationSettings.GetConnectionString()))
                {
                    DynamicParameters dyn = new DynamicParameters();
                    dyn.Add("@DiningHallID", dHall.DiningHallID);
                    dyn.Add("@DateAdded", DateTime.Today);

                    List<Dish> temporaryAllMeals = new List<Dish>();
                    temporaryAllMeals =
                        connection.Query<Dish>("SelectAllDishes", dyn, commandType: CommandType.StoredProcedure)
                                  .ToList();

                    if (temporaryAllMeals.Count != 0)
                        return true;
                }
            }

            return false;
        }

        public DiningHallLookupResult SelectAllDishes(DiningHallLookupResult nicelySortedData)
        {
            foreach (DiningHall dHall in nicelySortedData.MenuData)
            {
                dHall.Breakfast = new List<Dish>();
                dHall.Lunch = new List<Dish>();
                dHall.Dinner = new List<Dish>();

                using (var connection = new SqlConnection(ConfigurationSettings.GetConnectionString()))
                {
                    DynamicParameters dyn = new DynamicParameters();
                    dyn.Add("@DiningHallID", dHall.DiningHallID);
                    dyn.Add("@DateAdded", DateTime.Today);

                    List<Dish> temporaryAllMeals = new List<Dish>();
                    temporaryAllMeals = connection.Query<Dish>("SelectAllDishes", dyn, commandType: CommandType.StoredProcedure).ToList();

                    dHall.Breakfast = temporaryAllMeals.Where(m => m.MealTypeID == (int)MealTypeList.Breakfast).ToList();
                    dHall.Lunch = temporaryAllMeals.Where(m => m.MealTypeID == (int)MealTypeList.Lunch).ToList();
                    dHall.Dinner = temporaryAllMeals.Where(m => m.MealTypeID == (int)MealTypeList.Dinner).ToList();
                }
            }

            return nicelySortedData;
        }
    }
}
