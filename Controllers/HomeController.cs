using CNPM_NC_DoAnNhanh.Models;
using Microsoft.AspNetCore.Mvc;
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


        public ActionResult details()
        {
            return View();
        }
        // trang chu
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
        //tìm kiếm
        public List<DoUong> SearchUsers(IMongoCollection<DoUong> collection, string keyword)
        {
            var filter = Builders<DoUong>.Filter.Where(u => u.TenDoUong.ToLower().Contains(keyword));
            return collection.Find(filter).ToList();
        }

        [HttpGet]
        public IActionResult Search(string keyword)
        {
            var collection = _database.GetCollection<DoUong>("DoUong");
            var users = SearchUsers(collection, keyword);
            if (users == null)
            {

            }
            return PartialView(users);
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