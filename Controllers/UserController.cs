using CNPM_NC_DoAnNhanh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver;
using System;

namespace CNPM_NC_DoAnNhanh.Controllers
{
    public class UserController : Controller
    {
        private readonly IMongoDatabase _database;

        public UserController(IMongoDatabase database)
        {
            _database = database;
        }
        //đơn hàng khách hàng
     
        [HttpGet]
        public IActionResult Index_DH(string mauser)
        {
            mauser = HttpContext.Session.GetString("UserID");
            var Donhang = _database.GetCollection<DonHang>("DonHang");
            var danhsachdh = Donhang.Find(x => x.MaKH == mauser).ToList();
            return View(danhsachdh);
        }
        public IActionResult ChiTietDonHang(string madh)
        {
            if (string.IsNullOrEmpty(madh))
            {
               
                return BadRequest("Mã đơn hàng không hợp lệ");
            }

            var CTDH = _database.GetCollection<CTDH>("ChiTietDonHang");
            var danhsachCTDH = CTDH.Find(x => x.MaDH == madh).ToList();

            if (danhsachCTDH.Count == 0)
            {
               
                return NotFound("Không tìm thấy chi tiết đơn hàng");
            }

            return View(danhsachCTDH);
        }
        // GET: User
        //public ActionResult Info() 


        //public UserController(IMongoDatabase database)
        //{
        //    _database = database;
        //}
        //đơn hàng khách hàng
     
        //[HttpGet]
        //public IActionResult Index_DH(string mauser)
        //{
        //    mauser = HttpContext.Session.GetString("UserID");
        //    var Donhang = _database.GetCollection<DonHang>("DonHang");
        //    var danhsachdh = Donhang.Find(x => x.MaKH == mauser).ToList();
        //    return View(danhsachdh);
        //}
        //public IActionResult ChiTietDonHang(string madh)
        //{
        //    if (string.IsNullOrEmpty(madh))
        //    {
               
        //        return BadRequest("Mã đơn hàng không hợp lệ");
        //    }

        //    var CTDH = _database.GetCollection<CTDH>("ChiTietDonHang");
        //    var danhsachCTDH = CTDH.Find(x => x.MaDH == madh).ToList();

        //    if (danhsachCTDH.Count == 0)
        //    {
               
        //        return NotFound("Không tìm thấy chi tiết đơn hàng");
        //    }

        //    return View(danhsachCTDH);
        //}
        // GET: User
        [HttpGet]
        public IActionResult Info(string id)
        {
            var collection = _database.GetCollection<KhachHang>("KhachHang");
            var user = collection.Find(x => x._id == id).FirstOrDefault();
            return View(user);
        }

        [HttpPost]
        public IActionResult Info(KhachHang kh)
        {
            if (ModelState.IsValid)
            {
                var collection = _database.GetCollection<KhachHang>("KhachHang");
                var filter = Builders<KhachHang>.Filter.Eq("_id", kh._id);
                var update = Builders<KhachHang>.Update
                    .Set("HoTenKH", kh.HoTenKH)
                    .Set("SDT", kh.SDT)
                    .Set("DiaChi", kh.DiaChi);
                var result = collection.UpdateOne(filter, update);
                if (result.IsAcknowledged && result.ModifiedCount > 0)
                {
                    return RedirectToAction("Info");
                }
                else
                {

                }
            }
            return View(kh);
        }




        public ActionResult ViewKhuyenmai()
        {
            return View();
        }
    }
}
