using Microsoft.AspNetCore.Mvc;

namespace CNPM_NC_DoAnNhanh.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // GET: User
        public ActionResult Info()
        {
            return View();
        }
        public ActionResult ViewKhuyenmai()
        {
            return View();
        }
    }
}
