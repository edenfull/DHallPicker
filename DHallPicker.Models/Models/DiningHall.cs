using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHallPicker.Models.Models
{
    public class DiningHall
    {
        public int DiningHallID { get; set; }
        public string DiningHallName { get; set; }
        public string URL { get; set; }

        public List<Dish> Breakfast { get; set; }
        public List<Dish> Lunch { get; set; }
        public List<Dish> Dinner { get; set; } 
    }
}
