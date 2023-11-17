using CNPM_NC_DoAnNhanh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver;
using System.Diagnostics;

namespace CNPM_NC_DoAnNhanh.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMongoDatabase _database;

        public HomeController(IMongoDatabase database)
        {
            _database = database;
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
        // trang chu
        [HttpGet]
        public IActionResult Index(string keyword, string loaiSanPham)
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
            return View(danhSachDoUong);
        }   
        //Thong Tin San Pham
        public IActionResult ThongTinSP(string id)
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