using CNPM_NC_DoAnNhanh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver;
using System.Drawing;
using System.IO;
using System.Text;
using X.PagedList;

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
        public IActionResult Index(string keyword, string loaiSanPham, int? page)
        {
            var doUongCollection = _database.GetCollection<DoUong>("DoUong");
            var phanLoaiCollection = _database.GetCollection<PhanLoai>("PhanLoai");
            // Tìm danh mục theo tên
            var danhMuc = phanLoaiCollection.Find(x => x.TenLoai == loaiSanPham).FirstOrDefault();
            var keywordFilter = Builders<DoUong>.Filter.Empty;
            if (!string.IsNullOrEmpty(keyword))
            {
                   keywordFilter = Builders<DoUong>.Filter.Where(u => u.TenDoUong.ToLower().Contains(keyword));
            }
            // Tạo bộ lọc cho loại sản phẩm
            var loaiSanPhamFilter = Builders<DoUong>.Filter.Empty;
            if (danhMuc != null)
            {
                loaiSanPhamFilter = Builders<DoUong>.Filter.Eq("LoaiSanPham", danhMuc._id);
            }
            // Kết hợp cả hai bộ lọc
            var combinedFilter = Builders<DoUong>.Filter.And(keywordFilter, loaiSanPhamFilter);
            var danhSachDoUong = doUongCollection.Find(combinedFilter).ToList();
            var danhSachLoaiSanPham = phanLoaiCollection.Find(_ => true).ToList();
            ViewBag.LoaiSanPhamList = new SelectList(danhSachLoaiSanPham, "_id", "TenLoai");
            foreach (var doUong in danhSachDoUong)
            {
                var loaiSanPham_ = danhSachLoaiSanPham.FirstOrDefault(l => l._id == doUong.LoaiSanPham);
                if (loaiSanPham_ != null)
                {
                    doUong.LoaiSanPham = loaiSanPham_.TenLoai;
                }
            }

            int pageSize = 10; // Số lượng phần tử trên mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại, mặc định là trang 1

            // Thực hiện phân trang cho danh sách sản phẩm
            var pagedList = danhSachDoUong.ToPagedList(pageNumber, pageSize);
            return View(pagedList);
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
        public IActionResult upimage()
        {
            var phanLoaiCollection = _database.GetCollection<PhanLoai>("PhanLoai");
            var danhSachLoaiSanPham = phanLoaiCollection.Find(_ => true).ToList();

            ViewBag.LoaiSanPhamList = new SelectList(danhSachLoaiSanPham, "_id", "TenLoai");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> upimage(DoUong doUong, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await image.CopyToAsync(stream);
                        doUong.Img = Convert.ToBase64String(stream.ToArray());
                    }
                }

                var selectedLoaiSanPham = Request.Form["LoaiSanPham"];
                doUong.LoaiSanPham = selectedLoaiSanPham;

                var doUongCollection = _database.GetCollection<DoUong>("DoUong");
                await doUongCollection.InsertOneAsync(doUong);

                return RedirectToAction("Index");
            }

            // Xử lý trường hợp ModelState không hợp lệ hoặc các xác nhận khác

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
