using CNPM_NC_DoAnNhanh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CNPM_NC_DoAnNhanh.Admin.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly IMongoDatabase _database;

        public NhanVienController(IMongoDatabase database)
        {
            _database = database;
        }

        [HttpGet]
        public IActionResult Index(string keyword, string chucVuFilter)
        {
            var nhanVienCollection = _database.GetCollection<NhanVien>("NhanVien");
            var chucVuCollection = _database.GetCollection<ChucVu>("ChucVu");

            // Tạo bộ lọc cho từ khóa
            var keywordFilter = Builders<NhanVien>.Filter.Empty;
            if (!string.IsNullOrEmpty(keyword))
            {
                keywordFilter = Builders<NhanVien>.Filter.Where(nv => nv.TenNV.ToLower().Contains(keyword.ToLower()));
            }

            // Tạo bộ lọc cho chức vụ
            var chucVuIdFilter = Builders<NhanVien>.Filter.Empty;
            if (!string.IsNullOrEmpty(chucVuFilter))
            {
                chucVuIdFilter = Builders<NhanVien>.Filter.Eq("MaCV", chucVuFilter);
            }

            // Kết hợp cả hai bộ lọc
            var combinedFilter = Builders<NhanVien>.Filter.And(keywordFilter, chucVuIdFilter);

            // Lấy danh sách nhân viên
            var nhanVienList = nhanVienCollection.Find(combinedFilter).ToList();

            // Lấy danh sách chức vụ để hiển thị trong dropdown
            var chucVuList = chucVuCollection.Find(_ => true).ToList();
            ViewBag.ChucVuList = new SelectList(chucVuList, "_id", "TenCV");
            foreach (var nhanVien in nhanVienList)
            {
                // Kiểm tra xem MaCV có phải là ObjectId hợp lệ hay không
                if (ObjectId.TryParse(nhanVien.MaCV, out var objectId))
                {
                    var chucVu = chucVuCollection.Find(x => x._id == objectId.ToString()).FirstOrDefault();
                    if (chucVu != null)
                    {
                        nhanVien.MaCV = chucVu.TenCV;
                    }
                }
            }
            return View(nhanVienList);
        }


        [HttpGet]
        public IActionResult Create()
        {
            var ChucvuCollection = _database.GetCollection<ChucVu>("ChucVu");
            var danhSachchucvu = ChucvuCollection.Find(_ => true).ToList();
            ViewBag.Chucvu = new SelectList(danhSachchucvu, "_id", "TenCV");
            return View();
        }

        [HttpPost]
        public IActionResult Create(NhanVien cv)
        {
            if (ModelState.IsValid)
            {
                var collection = _database.GetCollection<NhanVien>("NhanVien");
                collection.InsertOne(cv);
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Lỗi");
            }
            var ChucvuCollection = _database.GetCollection<ChucVu>("ChucVu");
            var danhSachchucvu = ChucvuCollection.Find(_ => true).ToList();
            ViewBag.Chucvu = new SelectList(danhSachchucvu, "_id", "TenCV");
            return View(cv);
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var ChucvuCollection = _database.GetCollection<ChucVu>("ChucVu");
            var danhSachchucvu = ChucvuCollection.Find(_ => true).ToList();
            ViewBag.Chucvu = new SelectList(danhSachchucvu, "_id", "TenCV");

            var collection = _database.GetCollection<NhanVien>("NhanVien");
            var nv = collection.Find(x => x._id == id).FirstOrDefault();
            if (nv == null)
            {
                return NotFound();
            }

            return View(nv);
        }

        [HttpPost]
        public IActionResult Edit(NhanVien nv)
        {
            var ChucvuCollection = _database.GetCollection<ChucVu>("ChucVu");
            var danhSachchucvu = ChucvuCollection.Find(_ => true).ToList();
            ViewBag.LoaiSanPhamList = new SelectList(danhSachchucvu, "_id", "TenCV");
            var collection = _database.GetCollection<NhanVien>("NhanVien");
            if (ModelState.IsValid)
            {
                var filter = Builders<NhanVien>.Filter.Eq("_id", nv._id);
                var update = Builders<NhanVien>.Update.Set("TenNV", nv.TenNV)
                    .Set("TK", nv.TK)
                    .Set("PASS", nv.PASS);
       
                var result = collection.UpdateOne(filter, update);
                if (result.IsAcknowledged && result.ModifiedCount > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {

                }
            }

            return View(nv);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var collection = _database.GetCollection<NhanVien>("NhanVien");
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
         
            var collection = _database.GetCollection<NhanVien>("NhanVien");
            var nv = collection.Find(x => x._id == id).FirstOrDefault();
            if (nv == null)
            {
                return NotFound();
            }
            return View(nv);
        }



    }
}
