using CNPM_NC_DoAnNhanh.Models;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult Info()
        {
            return View();
        }
        public ActionResult ViewKhuyenmai()
        {
            return View();
        }
    }
}
