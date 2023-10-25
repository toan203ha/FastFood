using CNPM_NC_DoAnNhanh.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CNPM_NC_DoAnNhanh.Controllers
{
    public class DoUongController : Controller
    {
        private readonly IMongoDatabase _database;

        public DoUongController(IMongoDatabase database)
        {
            _database = database;
        }

        public List<DoUong> SearchUsers(IMongoCollection<DoUong> collection, string keyword)
        {
            var filter = Builders<DoUong>.Filter.Where(u => u.TenDoUong.ToLower().Contains(keyword));
            return collection.Find(filter).ToList();
        }

        // tìm kiếm 
        [HttpGet]
        public IActionResult Search(string keyword)
        {
            var collection = _database.GetCollection<DoUong>("DoUong");
            var users = SearchUsers(collection, keyword);
            if(users == null)
            {

            }
            return PartialView(users);
        }
        [HttpGet]
        public IActionResult Index()
        {
            var collection = _database.GetCollection<DoUong>("DoUong");
            var monAnList = collection.Find(_ => true).ToList();
            return View(monAnList);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DoUong monAn)
        {
            if (ModelState.IsValid)
            {
                var collection = _database.GetCollection<DoUong>("DoUong");
                collection.InsertOne(monAn);
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Lỗi");
            }
            return View(monAn);
        }


        [HttpGet]
        public IActionResult Edit(string id)
        {
            var collection = _database.GetCollection<DoUong>("DoUong");
            var monAn = collection.Find(x => x._id == id).FirstOrDefault();

            if (monAn == null)
            {
                return NotFound();
            }

            return View(monAn);
        }

        [HttpPost]
        public IActionResult Edit(DoUong monAn)
        {
            if (ModelState.IsValid)
            {
                var collection = _database.GetCollection<DoUong>("DoUong");
                var filter = Builders<DoUong>.Filter.Eq("_id", monAn._id);
                var update = Builders<DoUong>.Update
                    .Set("TenDoUong", monAn.TenDoUong)
                    .Set("Img", monAn.Img)
                    .Set("GiaTien", monAn.GiaTien)
                    .Set("SoLuong", monAn.SoLuong)
                    .Set("Size", monAn.Size);

                var result = collection.UpdateOne(filter, update);

                if (result.IsAcknowledged && result.ModifiedCount > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // Xử lý lỗi
                }
            }
            return View(monAn);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var collection = _database.GetCollection<DoUong>("DoUong");
            var result = collection.DeleteOne(x => x._id == id);

            if (result.IsAcknowledged && result.DeletedCount > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Xử lý lỗi
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(string id)
        {
            var collection = _database.GetCollection<DoUong>("DoUong");
            var moAn = collection.Find(x => x._id == id).FirstOrDefault();

            if (moAn == null)
            {
                return NotFound();
            }
            return View(moAn);
        }

    }
}
