using Microsoft.AspNetCore.Mvc;

namespace CNPM_NC_DoAnNhanh.Controllers
{
    public class DrinkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Drink()
        {
            return View();
        }
    }
}
