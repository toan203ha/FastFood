using CNPM_NC_DoAnNhanh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver;

namespace CNPM_NC_DoAnNhanh.Controllers
{
    public class DoUongController : Controller
    {
        //khai báo
        private readonly IMongoDatabase _database;

        public DoUongController(IMongoDatabase database)
        {
            _database = database;
        }

        // tìm kiếm
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
        //hiển thị danh sách sản phẩm
        [HttpGet]
        public IActionResult Index()
        {
            var doUongCollection = _database.GetCollection<DoUong>("DoUong");
            var phanLoaiCollection = _database.GetCollection<PhanLoai>("PhanLoai");

            var danhSachDoUong = doUongCollection.Find(_ => true).ToList();
            var danhSachLoaiSanPham = phanLoaiCollection.Find(_ => true).ToList();
            // lay bằng tên
            foreach (var doUong in danhSachDoUong)
            {
                var loaiSanPham = danhSachLoaiSanPham.FirstOrDefault(l => l._id == doUong.LoaiSanPham);
                if (loaiSanPham != null)
                {
                    doUong.LoaiSanPham = loaiSanPham.TenLoai;
                }
            }
            return View(danhSachDoUong);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var phanLoaiCollection = _database.GetCollection<PhanLoai>("PhanLoai");
            var danhSachLoaiSanPham = phanLoaiCollection.Find(_ => true).ToList();

            ViewBag.LoaiSanPhamList = new SelectList(danhSachLoaiSanPham, "_id", "TenLoai");

            return View();
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DoUong doUong)
        {
            if (ModelState.IsValid)
            {
                var selectedLoaiSanPham = Request.Form["LoaiSanPham"];
                doUong.LoaiSanPham = selectedLoaiSanPham;
                var doUongCollection = _database.GetCollection<DoUong>("DoUong");
                doUongCollection.InsertOne(doUong);

                return RedirectToAction("Index");
            }
            var phanLoaiCollection = _database.GetCollection<PhanLoai>("PhanLoai");
            var danhSachLoaiSanPham = phanLoaiCollection.Find(_ => true).ToList();
            ViewBag.LoaiSanPhamList = new SelectList(danhSachLoaiSanPham, "_id", "TenLoai");

            return View(doUong);
        }


        [HttpGet]
        public IActionResult Edit(string id)
        {
            var collection = _database.GetCollection<DoUong>("DoUong");
            var monAn = collection.Find(x => x._id == id).FirstOrDefault();
            var phanLoaiCollection = _database.GetCollection<PhanLoai>("PhanLoai");
            var danhSachLoaiSanPham = phanLoaiCollection.Find(_ => true).ToList();

<<<<<<< HEAD
=======

            var phanLoaiCollection = _database.GetCollection<PhanLoai>("PhanLoai");
            var danhSachLoaiSanPham = phanLoaiCollection.Find(_ => true).ToList();

>>>>>>> dc570f7c3ab9b70b9d916b4baf05e4b44317fa1e
            ViewBag.LoaiSanPhamList = new SelectList(danhSachLoaiSanPham, "_id", "TenLoai");
            if (monAn == null)
            {
                return NotFound();
            }

            return View(monAn);
        }

        [HttpPost]
        public IActionResult Edit(DoUong monAn)
        {
            var phanLoaiCollection = _database.GetCollection<PhanLoai>("PhanLoai");
            var danhSachLoaiSanPham = phanLoaiCollection.Find(_ => true).ToList();
            ViewBag.LoaiSanPhamList = new SelectList(danhSachLoaiSanPham, "_id", "TenLoai");
            if (ModelState.IsValid)
            {
                var phanLoaiCollection = _database.GetCollection<PhanLoai>("PhanLoai");
                var danhSachLoaiSanPham = phanLoaiCollection.Find(_ => true).ToList();
                ViewBag.LoaiSanPhamList = new SelectList(danhSachLoaiSanPham, "_id", "TenLoai");


                var collection = _database.GetCollection<DoUong>("DoUong");
                var filter = Builders<DoUong>.Filter.Eq("_id", monAn._id);
                var update = Builders<DoUong>.Update
                    .Set("TenDoUong", monAn.TenDoUong)
                    .Set("Img", monAn.Img)
                    .Set("GiaTien", monAn.GiaTien)
                    .Set("SoLuong", monAn.SoLuong)
                    .Set("Size", monAn.Size)
                    .Set("LoaiSanPham", monAn.LoaiSanPham);

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
