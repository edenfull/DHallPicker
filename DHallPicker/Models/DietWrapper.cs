using System.Collections.Generic;
using DHallPicker.Data.Repositories;
using DHallPicker.Models.Models;

namespace DHallPicker.Models
{
    public class DietWrapper
    {
        public List<Diet> AllFilters { get; set; }
 
        public DietWrapper()
        {
            var dataContext = new DietRepository();
            AllFilters = new List<Diet>();

            foreach (Diet filterType in dataContext.SelectAllDiets())
            {
                AllFilters.Add(new Diet()
                    {
                        DishFilterID = filterType.DishFilterID,
                        FilterName = filterType.FilterName,
                        FilterDescription = filterType.FilterDescription
                    });
            }
        }
    }
}