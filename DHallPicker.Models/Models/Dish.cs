using System;

namespace DHallPicker.Models.Models
{
    public class Dish
    {
        public string DishName { get; set; }
        public int DiningHallID { get; set; }
        public int MealTypeID { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
