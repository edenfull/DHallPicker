using System.Collections.Generic;

namespace DHallPicker.Models.Models
{
    public class DiningHallLookupResult
    {
        public List<DiningHall> MenuData { get; set; }

        public string RecommendedBreakfast { get; set; }
        public string RecommendedLunch { get; set; }
        public string RecommendedDinner { get; set; }
    }
}