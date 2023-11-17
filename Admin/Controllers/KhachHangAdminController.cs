using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver;
using CNPM_NC_DoAnNhanh.Models;

namespace CNPM_NC_DoAnNhanh.Admin.Controllers
{
    public class KhachHangAdminController : Controller
    {
        private readonly IMongoDatabase _database;
        public KhachHangAdminController(IMongoDatabase database)
        {
            _database = database;
        }
        [HttpGet]
        public IActionResult Index(string keyword)
        {
            var collection = _database.GetCollection<KhachHang>("KhachHang");
            var listKH = collection.Find(_ => true).ToList();
            if (!string.IsNullOrEmpty(keyword))
            {
                listKH = search(collection, keyword);

            }
            return View(listKH);
        }
        public List<KhachHang> search(IMongoCollection<KhachHang> collection, string keyword)
        {
            var filter = Builders<KhachHang>.Filter.Where(u => u.HoTenKH.ToLower().Contains(keyword.ToLower()));
            return collection.Find(filter).ToList();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(KhachHang khach)
        {
            if (ModelState.IsValid)
            {
                var khachHangCollection = _database.GetCollection<KhachHang>("KhachHang");
                khachHangCollection.InsertOne(khach);

                return RedirectToAction("Index");
            }
            return View(khach);
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var collection = _database.GetCollection<KhachHang>("KhachHang");
            var khachHang = collection.Find(x => x._id == id).FirstOrDefault();

            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        [HttpPost]
        public IActionResult Edit (KhachHang khach)
        {
            if (ModelState.IsValid)
            {
                var collection = _database.GetCollection<KhachHang>("KhachHang");
                var filter = Builders<KhachHang>.Filter.Eq("_id", khach._id);
                var update = Builders<KhachHang>.Update
                    .Set("HoTenKH", khach.HoTenKH)
                    .Set("SDT", khach.SDT)
                    .Set("DiaChi", khach.DiaChi)
                    .Set("Vi", khach.Vi)
                    .Set("TK", khach.TK)
                    .Set("PASS", khach.PASS);
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
            return View(khach);
        }

        public IActionResult Details(string id)
        {
            var collection = _database.GetCollection<KhachHang>("KhachHang");
            var khach = collection.Find(x => x._id == id).FirstOrDefault();

            if (khach == null)
            {
                return NotFound();
            }
            return View(khach);
        }
    }
}
