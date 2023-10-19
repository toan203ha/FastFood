using CNPM_NC_DoAnNhanh.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CNPM_NC_DoAnNhanh.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public ActionResult details()
        {
            return View();
        }
        // trang chu
        public ActionResult Index()
        {
            return View();
        }
        // gio hang chua co san pham nao
        public ActionResult CartNoProduct()
        {
            return View();
        }
        // gio hang da co san pham
        public ActionResult CartProduct()
        {
            return View();
        }

        // danh sách sản phẩm
        public ActionResult ViewSanPham()
        {
            return View();
        }
    }
}