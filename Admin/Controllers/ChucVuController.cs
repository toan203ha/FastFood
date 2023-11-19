using CNPM_NC_DoAnNhanh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver;

namespace CNPM_NC_DoAnNhanh.Admin.Controllers
{
    public class ChucVuController : Controller
    {
        private readonly IMongoDatabase _database;

        public ChucVuController(IMongoDatabase database)
        {
            _database = database;
        }
        [HttpGet]
        public IActionResult Index(string keyword)
        {
            var collection = _database.GetCollection<ChucVu>("ChucVu");
            var ChucvuList = collection.Find(_ => true).ToList();
             if (!string.IsNullOrEmpty(keyword))
            {
                ChucvuList = search(collection, keyword);
            }
            return View(ChucvuList);
        }
        public List<ChucVu> search(IMongoCollection<ChucVu> collection, string keyword)
        {
            var filter = Builders<ChucVu>.Filter.Where(u => u.TenCV.ToLower().Contains(keyword.ToLower()));
            return collection.Find(filter).ToList();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ChucVu cv)
        {

            if (ModelState.IsValid)
            {
                var collection = _database.GetCollection<ChucVu>("ChucVu");
                var existingChucVu = collection.Find(x => x.TenCV == cv.TenCV).FirstOrDefault();
                if (existingChucVu != null)
                {
                    ModelState.AddModelError("TenCV", "Tên đã tồn tại. Chọn tên khác.");
                    return View(cv);
                }
                collection.InsertOne(cv);
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Lỗi");
            }
            return View(cv);
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var collection = _database.GetCollection<ChucVu>("ChucVu");
            var monAn = collection.Find(x => x._id == id).FirstOrDefault();
            if (monAn == null)
            {
                return NotFound();
            }

            return View(monAn);
        }

        [HttpPost]
        public IActionResult Edit(ChucVu chucVu)
        {
            var collection = _database.GetCollection<ChucVu>("ChucVu");
            var existingChucVu = collection.Find(x => x.TenCV == chucVu.TenCV && x._id != chucVu._id).FirstOrDefault();
            if (existingChucVu != null)
            {
                ModelState.AddModelError("TenCV", "Tên đã tồn tại. Chọn tên khác.");
            }

            if (ModelState.IsValid)
            {
                var filter = Builders<ChucVu>.Filter.Eq("_id", chucVu._id);
                var update = Builders<ChucVu>.Update.Set("TenCV", chucVu.TenCV);

                var result = collection.UpdateOne(filter, update);

                if (result.IsAcknowledged && result.ModifiedCount > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
 
                }
            }

            return View(chucVu);
        }
        [HttpGet]
        public IActionResult Delete(string id)
        {
            var collection = _database.GetCollection<ChucVu>("ChucVu");
            var result = collection.DeleteOne(x => x._id == id);

            if (result.IsAcknowledged && result.DeletedCount > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {

            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(string id)
        {
            var collection = _database.GetCollection<ChucVu>("ChucVu");
            var cv = collection.Find(x => x._id == id).FirstOrDefault();

            if (cv == null)
            {
                return NotFound();
            }
            return View(cv);
        }

    }
}
