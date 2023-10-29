using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using CNPM_NC_DoAnNhanh.Models;
using MongoDB.Driver;
using Microsoft.AspNetCore.Http;

public class ShoppingCartController : Controller
{
    private readonly IMemoryCache _cache;

    private readonly IMongoDatabase _database;

    public ShoppingCartController(IMemoryCache memoryCache, IMongoDatabase database)
    {
        _cache = memoryCache;
    
    }

    public IActionResult Index()
    {
        var cart = GetCart();
        return View(cart);
    }

    public IActionResult AddToCart(string productId, string productName, int price, int quantity)
    {
        var cart = GetCart();
        var existingItem = cart.FirstOrDefault(item => item.MaSP == productId);

        if (existingItem != null)
        {
            existingItem.Soluong += quantity;
        }
        else
        {
            cart.Add(new CartItem
            {
                MaSP = productId,
                Ten = productName,
                Dongia = price,
                Soluong = quantity
            });
        }

        SaveCart(cart);

        return RedirectToAction("Index");
    }

    public IActionResult RemoveFromCart(string productId)
    {
        var cart = GetCart();
        var itemToRemove = cart.FirstOrDefault(item => item.MaSP == productId);

        if (itemToRemove != null)
        {
            cart.Remove(itemToRemove);
        }

        SaveCart(cart);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult UpdateQuantity(string productId, int newQuantity)
    {
        Console.WriteLine($"productId: {productId}, newQuantity: {newQuantity}");

        var cart = GetCart(); 
        var cartItem = cart.FirstOrDefault(item => item.MaSP == productId);

        if (cartItem != null)
        {
            //cartItem.Soluong = newQuantity;
            cart.Add(new CartItem
            {
                MaSP = productId, 
                Soluong = newQuantity
            });
     
        }
        return RedirectToAction("Index"); 
    }





    private List<CartItem> GetCart()
    {
        if (!_cache.TryGetValue("Cart", out List<CartItem> cart))
        {
            cart = new List<CartItem>();
        }
        return cart;
    }

    private void SaveCart(List<CartItem> cart)
    {
        _cache.Set("Cart", cart, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        });
    }



    // hiển thị giỏ hàng trong layout
    public IActionResult GiohangPartial()
    {
        var cart = GetCart(); // Hàm này cần tự cài đặt để lấy thông tin giỏ hàng từ session
        var totalItems = cart.Sum(item => item.Soluong);
        return PartialView(totalItems);
    }

    // đặt hàng

    private double TinhTongTien()
    {
        double TongTien = 0;
        List<CartItem> gioHang = GetCart();
        if (gioHang != null)
        {
            TongTien = gioHang.Sum(sp => sp.Total());
        }
        return TongTien;
    }

    private int TinhTongSL()
    {
        int tongSL = 0;
        List<CartItem> gioHang = GetCart();
        if (gioHang != null)
            tongSL = gioHang.Sum(sp => sp.Soluong);
        return tongSL;
    }

    //dat hang
    public ActionResult DatHang()
    {
        //if (Session["taikhoan"] == null)
        //    return RedirectToAction("Login", "Account");
        List<CartItem> giohang = GetCart();
        if (giohang == null || giohang.Count == 0)
            return RedirectToAction("Index", "Food");
        ViewBag.TongSL = TinhTongSL();
        ViewBag.TongTien = TinhTongTien();
        return View(giohang);
    }
    //// xử lý đặt hàng
    //public ActionResult DatHang()
    //{  
    //    var collection = _database.GetCollection<DonHang>("DonHang");

    //    var collection_ = _database.GetCollection<CTDH>("CTDonHang");

    

    //    KhachHang kh = Session["taikhoan"] as KhachHang;
    //    List<CartItem> giohang = GetCart();
    //    // thêm dữ liệu vào đơn hàng
    //    DonHang donhang = new DonHang();
    //    donhang.MaKH = kh._id;
    //    donhang.NgayDatHang = DateTime.Now;
    //    donhang.TongTien= (Decimal)TinhTongTien();
    //    donhang.TinhTrangDonHang = false;
    //    donhang.TenNguoiNhan = kh.HoTenKH;
    //    donhang.DiaChiNhan = kh.DiaChi; 
    //    donhang.HinhThucThanhToan = false;
    //    donhang.HinhThucGiaoHang = false;
    //    collection.InsertOne(donhang);

    //    // thêm vào chi tiết đơn hàng
    //    foreach (var sanpham in giohang)
    //    {
    //        CTDH ct = new CTDH();
    //        ct.MaDH = donhang._id;
    //        ct.MaNuoc = sanpham.MaSP;
    //        ct.MaCombo = sanpham.MaSP;
    //        ct.MaMonAn = sanpham.MaSP;
    //        ct.SoLuong = sanpham.Soluong;
    //        ct.ThanhTien = (decimal)TinhTongTien();
    //        collection_.InsertOne(ct);

    //    }
 
    //    //xóa giỏ hàng
    //    Session["GioHang"] = null;
    //    return RedirectToAction("HoanThanhDonHang");
    //}

    public ActionResult HoanThanhDonHang()
    {
        return View();
    }

}
