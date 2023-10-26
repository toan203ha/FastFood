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
        public IActionResult ApplyVoucherToCart(int maKH, string maVoucher)
        {
            // Kiểm tra xem Voucher có hợp lệ không
            var voucher = _voucherRepository.GetVoucherByCode(maVoucher);

            if (voucher != null)
            {
                // Lấy giỏ hàng của khách hàng từ Session
                var gioHang = HttpContext.Session.Get<List<CartItem>>("Cart");

                if (gioHang != null && gioHang.Count > 0)
                {
                    // Cập nhật giá sản phẩm trong giỏ hàng sau khi áp dụng Voucher
                    foreach (var item in gioHang)
                    {
                        item.Dongia -= item.Dongia * (int)voucher.GiaTriVoucher/100;
                    }

                    // Lưu lại giỏ hàng sau khi cập nhật giá sản phẩm vào Session
                    HttpContext.Session.Set("Cart", gioHang);
                }

                return RedirectToAction("Index"); // Điều hướng quay lại trang giỏ hàng
            }
            else
            {
                TempData["Error"] = "Mã Voucher không hợp lệ!";
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
            // Lấy giỏ hàng hiện tại từ Session (nếu có)
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();

            // Kiểm tra xem sản phẩm đã tồn tại trong giỏ hàng chưa
            var existingItem = cart.FirstOrDefault(item => item.MaSP == productId);

            if (existingItem != null)
            {
                // Nếu sản phẩm đã tồn tại, cập nhật số lượng
                existingItem.Soluong += quantity;
            }
            else
            {
                // Nếu sản phẩm chưa tồn tại, thêm sản phẩm mới vào giỏ hàng
                var newItem = new CartItem(productId, productName, price, quantity);
                cart.Add(newItem);
            }

            // Lưu giỏ hàng vào Session
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

            return RedirectToAction("Index");
        }

 

        public IActionResult DatHang()
        {
            if (HttpContext.Session.Get<KhachHang>("taikhoan") == null)
                return RedirectToAction("Login", "Login");

            List<CartItem> giohang = HttpContext.Session.Get<List<CartItem>>("Cart");

            if (giohang == null || giohang.Count == 0)
                return RedirectToAction("Index", "Food");

            return View(giohang);
        }


 
        public IActionResult DongYDatHang()
            {
                var collection = _database.GetCollection<DonHang>("DonHang");
                var collection_ = _database.GetCollection<CTDH>("CTDonHang");
                KhachHang kh = HttpContext.Session.Get<KhachHang>("taikhoan");

                List<CartItem> giohang = (List<CartItem>)Index();
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
                    ct.MaMonAn = sanpham.MaSP;
                    ct.SoLuong = sanpham.Soluong;
                    ct.ThanhTien = 1000;
                    collection_.InsertOne(ct);
                }
                foreach (var sanpham in giohang)
                {
                    CTDH ct = new CTDH();
                    ct.MaDH = donhang._id;
                    ct.MaCombo = sanpham.MaSP;
                    ct.SoLuong = sanpham.Soluong;
                    ct.ThanhTien = 1000;
                    collection_.InsertOne(ct);
                }
                foreach (var sanpham in giohang)
                {
                    CTDH ct = new CTDH();
                    ct.MaDH = donhang._id;
                    ct.MaNuoc = sanpham.MaSP;
                    ct.SoLuong = sanpham.Soluong;
                    ct.ThanhTien = 1000;
                    collection_.InsertOne(ct);
                }
                HttpContext.Session.Remove("GioHang");
                return RedirectToAction("HoanThanhDonHang");
            }

        public IActionResult HoanThanhDonHang()
        {
            return View();
        }
    }
}
