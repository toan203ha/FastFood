using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CNPM_NC_DoAnNhanh.Models;
using MongoDB.Driver;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace CNPM_NC_DoAnNhanh.Controllers
{
    public class GioHangController : Controller
    {

        private readonly IMongoDatabase _database;
        private VoucherRepository _voucherRepository;

        public GioHangController(IMongoDatabase database, VoucherRepository voucherRepository)
        {
            _database = database;
            _voucherRepository = voucherRepository;
        }
        // vourcher


        // Trong Controller
        //public IActionResult ApplyDiscountCode(string discountCode)
        //{
        //    var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
        //    var voucherCollection = _database.GetCollection<Vourcher>("Vourcher");
        //    var voucher = voucherCollection.Find(v => v.TenVoucher == discountCode).FirstOrDefault();

        //    if (voucher != null)
        //    {
        //        foreach (var item in cart)
        //        {
        //            item.Dongia -= item.Dongia * (voucher.GiaTriVoucher / 100);
        //        }

        //        var totalDiscount = cart.Sum(item => item.Dongia * (1 - voucher.GiaTriVoucher / 100) * item.Soluong);
        //        //DonHang donhang = new DonHang();
        //        /* donhang.TongTien = (decimal)(1000 - totalDiscount);*/ // Cập nhật tổng tiền giảm giá vào đơn hàng

        //        // Đánh dấu rằng mã giảm giá đã được sử dụng
        //        voucher.IsUsed = true;
        //        // Cập nhật trạng thái mã giảm giá
        //        voucherCollection.ReplaceOne(v => v.TenVoucher == voucher.TenVoucher, voucher); 
        //        HttpContext.Session.Set("Cart", cart);
        //        return RedirectToAction("Index", new { voucherApplied = true });
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("DiscountCode", "Mã giảm giá không hợp lệ.");
        //        return RedirectToAction("Index");
        //    }
        //}


        public IActionResult ApplyDiscountCode(string discountCode)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();

            // Kiểm tra xem mã giảm giá đã được sử dụng cho đơn hàng hiện tại hay chưa
            if (HttpContext.Session.Get<bool>("IsDiscountApplied"))
            {
                ModelState.AddModelError("DiscountCode", "Bạn đã áp dụng mã giảm giá rồi.");
                return RedirectToAction("Index");
            }
            var voucherCollection = _database.GetCollection<Vourcher>("Vourcher");
            var voucher = voucherCollection.Find(v => v.TenVoucher == discountCode).FirstOrDefault();
            if (voucher != null)
            {
                HttpContext.Session.Set("IsDiscountApplied", true);
                foreach (var item in cart)
                {
                    item.Dongia -= item.Dongia * (voucher.GiaTriVoucher / 100);
                }

                var totalDiscount = cart.Sum(item => item.Dongia * (1 - voucher.GiaTriVoucher / 100) * item.Soluong);

                // Đánh dấu rằng mã giảm giá đã được sử dụng cho đơn hàng hiện tại
                ViewBag.VoucherApplied = true; 
                // Cập nhật trạng thái mã giảm giá (ví dụ: đánh dấu mã giảm giá đã được sử dụng trong cơ sở dữ liệu)
                HttpContext.Session.Set("Cart", cart);

                return RedirectToAction("Index", new { voucherApplied = true });
            }
            else
            {
                ViewBag.VoucherApplied = false; 
                return RedirectToAction("Index");
            }
        }


        public IActionResult Index()
        {
            // Lấy giỏ hàng từ Session
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        public IActionResult AddToCart(string productId, string productName, int price, int quantity)
        {

            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            var existingItem = cart.FirstOrDefault(item => item.MaSP == productId);

            if (existingItem != null)
            {
                existingItem.Soluong += quantity;
            }
            else
            {
                var newItem = new CartItem(productId, productName, price, quantity);
                cart.Add(newItem);
            }
            HttpContext.Session.Set("Cart", cart);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(string productId, int newQuantity)
        {
            // Lấy giỏ hàng hiện tại từ Session (nếu có)
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            // Tìm sản phẩm trong giỏ hàng
            var cartItem = cart.FirstOrDefault(item => item.MaSP == productId);
            if (cartItem != null)
            {
                // Cập nhật số lượng sản phẩm
                cartItem.Soluong = newQuantity;
            }
            // Lưu giỏ hàng đã cập nhật vào Session
            HttpContext.Session.Set("Cart", cart);
            return RedirectToAction("Index");
        }
        public IActionResult ClearCart()
        {
            // Xóa toàn bộ giỏ hàng khỏi Session
            HttpContext.Session.Remove("Cart");
            HttpContext.Session.Remove("IsDiscountApplied");
            return RedirectToAction("Index");
        }
        public IActionResult DatHang()
        {
            if ( HttpContext.Session.GetString("UserID") == null)
                return RedirectToAction("Login", "Login");
            List<CartItem> giohang = HttpContext.Session.Get<List<CartItem>>("Cart");
            if (giohang == null || giohang.Count == 0)
                return RedirectToAction("Index", "Food");
            KhachHang khach = HttpContext.Session.Get<KhachHang>("TaiKhoan");
          //  ViewBag.TongSL = TinhTongSL();
           // ViewBag.TongTien = TinhTongTien();
            return View(giohang);
        }

    
     

        //public IActionResult DongYDatHang()
        //    {

        //        var collection = _database.GetCollection<DonHang>("DonHang");
        //        var collection_ = _database.GetCollection<CTDH>("CTDonHang");
        //        KhachHang kh = HttpContext.Session.Get<KhachHang>("taikhoan");
        //        List<CartItem> giohang = (List<CartItem>)Index();
        //        DonHang donhang = new DonHang();
        //        donhang.MaKH = kh._id;
        //        donhang.NgayDatHang = DateTime.Now;
        //        donhang.TongTien = 100;
        //        donhang.TinhTrangDonHang = false;
        //        donhang.TenNguoiNhan = kh.HoTenKH;
        //        donhang.DiaChiNhan = kh.DiaChi;
        //        donhang.HinhThucThanhToan = false;
        //        donhang.HinhThucGiaoHang = false;
        //        collection.InsertOne(donhang);

        //        foreach (var sanpham in giohang)
        //        {
        //            CTDH ct = new CTDH();
        //            ct.MaDH = donhang._id;
        //            ct.MaSP = sanpham.MaSP;
        //            ct.SoLuong = sanpham.Soluong;
        //            ct.ThanhTien = 100;
        //            collection_.InsertOne(ct);
        //        }
    
        //        HttpContext.Session.Remove("GioHang");
        //        return RedirectToAction("HoanThanhDonHang");
        //    }
        public IActionResult HoanThanhDonHang()
        {
            return View();
        }


        public IActionResult DongYDatHang()
        {
            // Gọi phương thức Index để lấy danh sách CartItem
            IActionResult result = Index();
            if (result is ViewResult viewResult)
            {
                List<CartItem> giohang = viewResult.Model as List<CartItem>;

                if (giohang == null)
                {
                    return View("Error");
                }

                // Tiếp tục xử lý giohang
                var collection = _database.GetCollection<DonHang>("DonHang");
                var collection_ = _database.GetCollection<CTDH>("ChiTietDonHang");
                KhachHang kh = HttpContext.Session.Get<KhachHang>("taikhoan");
                DonHang donhang = new DonHang();
                donhang.MaKH = kh._id;
                donhang.NgayDatHang = DateTime.Now;
                donhang.TongTien = 1000;
                donhang.TinhTrangDonHang = false;
                donhang.TenNguoiNhan = kh.HoTenKH;
                donhang.DiaChiNhan = kh.DiaChi;
                donhang.HinhThucThanhToan = false;
                donhang.HinhThucGiaoHang = false;
                collection.InsertOne(donhang);

                foreach (var sanpham in giohang)
                {
                    CTDH ct = new CTDH();
                    ct.MaDH = donhang._id;
                    ct.MaSP = sanpham.MaSP;
                    ct.SoLuong = sanpham.Soluong;
                    ct.ThanhTien = 1000;
                    collection_.InsertOne(ct);
                }

                HttpContext.Session.Remove("GioHang");
                return RedirectToAction("HoanThanhDonHang");
            }
            else
            {
                // Xử lý trường hợp result không phải là ViewResult
                // Ví dụ: Redirect hoặc trả về trang lỗi
                return View("Error");
            }
        }

    }
}
