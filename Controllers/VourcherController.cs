using CNPM_NC_DoAnNhanh.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CNPM_NC_DoAnNhanh.Controllers
{
    public class VourcherController : Controller
    {
        private readonly IMongoDatabase _database;

        public VourcherController(IMongoDatabase database)
        {
            _database = database;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var collection = _database.GetCollection<Vourcher>("Vourcher");
            var monAnList = collection.Find(_ => true).ToList();
            return View(monAnList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Vourcher vc)
        {
            if (ModelState.IsValid)
            {
                var collection = _database.GetCollection<Vourcher>("Vourcher");
                collection.InsertOne(vc);
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Lỗi");
            }
            return View(vc);
        }
    }
}
