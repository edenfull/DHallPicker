using System.Web.Mvc;
using DHallPicker.Data.Repositories;
using DHallPicker.Models;
using DHallPicker.Models.Models;
using DHallPicker.Operations.Operations;

namespace DHallPicker.Controllers
{
    public class PickerController : Controller
    {
        //
        // GET: /Picker/AllMenus
        public ActionResult AllMenus()
        {
            DiningHallLookupResult nicelySortedData = new DiningHallLookupResult();
            nicelySortedData.MenuData = new DiningHallRepository().SelectAllHalls();

            DishRepository dataContext = new DishRepository();

            if (dataContext.CheckIfTodayInDatabase(nicelySortedData) == false)
                new WebScraper().Run();

            nicelySortedData = dataContext.SelectAllDishes(nicelySortedData);

            return View(nicelySortedData);
        }

        //
        // GET: /Picker/SelectFilters
        public ActionResult SelectFilters()
        {
            return View(new DietWrapper());
        }

        [HttpPost]
        public ActionResult SelectFilters(DietWrapper currentModel, int[] diets)
        {
            DiningHallLookupResult nicelySortedData = new DiningHallLookupResult();
            nicelySortedData.MenuData = new DiningHallRepository().SelectAllHalls();

            DishRepository dataContext = new DishRepository();

            if (dataContext.CheckIfTodayInDatabase(nicelySortedData) == false)
                new WebScraper().Run();

            nicelySortedData = dataContext.SelectAllDishes(nicelySortedData);

            MenuFilter filterOps = new MenuFilter();
            nicelySortedData = filterOps.GetFilteredMenus(nicelySortedData, diets);
            nicelySortedData = filterOps.GetRecommendations(nicelySortedData);
            
            return View("FilteredMenu", nicelySortedData);
        }
    }
}
