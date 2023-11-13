using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CNPM_NC_DoAnNhanh.Models;
using MongoDB.Driver;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PayPal.Api;
using static System.Collections.Specialized.BitVector32;
using System.Globalization;
using System.Diagnostics;

namespace CNPM_NC_DoAnNhanh.Controllers
{
    public class GioHangController : Controller
    {

        private readonly IMongoDatabase _database;
        private VoucherRepository _voucherRepository;
        private readonly IConfiguration _configuration;

        public GioHangController(IMongoDatabase database, VoucherRepository voucherRepository, IConfiguration configuration)
        {
            _database = database;
            _voucherRepository = voucherRepository;
            _configuration = configuration;
        }
<<<<<<< HEAD

        //vourcher
=======


        // áp dụng mẫ giảm giá
>>>>>>> dc570f7c3ab9b70b9d916b4baf05e4b44317fa1e
        public IActionResult ApplyDiscountCode(string discountCode)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();     
            if (HttpContext.Session.Get<bool>("IsDiscountApplied"))
            {
                ModelState.AddModelError("DiscountCode", "Bạn đã áp dụng mã giảm giá rồi.");
                TempData["VoucherApplied"] = false;   
                return RedirectToAction("Index");
            }
            var appliedDiscountCode = TempData["AppliedDiscountCode"] as string;

            if (appliedDiscountCode != null && appliedDiscountCode.Equals(discountCode, StringComparison.OrdinalIgnoreCase))
            {             
                ModelState.AddModelError("DiscountCode", "Bạn đã áp dụng mã giảm giá này trước đó.");
                TempData["VoucherApplied"] = false;   
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
                TempData["AppliedDiscountCode"] = discountCode;            
                HttpContext.Session.Set("Cart", cart);
                TempData["VoucherApplied"] = true;   
                return RedirectToAction("Index", new { voucherApplied = true });
            }
            else
            {
                TempData["VoucherApplied"] = false;  
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

        //[HttpPost]
        //public IActionResult UpdateQuantity(string productId, int newQuantity)
        //{
        //    // Lấy giỏ hàng hiện tại từ Session (nếu có)
        //    var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
        //    // Tìm sản phẩm trong giỏ hàng
        //    var cartItem = cart.FirstOrDefault(item => item.MaSP == productId);
        //    if (cartItem != null)
        //    {
        //        // Cập nhật số lượng sản phẩm
        //        cartItem.Soluong = newQuantity;
        //    }
        //    // Lưu giỏ hàng đã cập nhật vào Session
        //    HttpContext.Session.Set("Cart", cart);
        //    return RedirectToAction("Index");
        //}
        private DoUong Laysp(string productId)
        {
            var sp = _database.GetCollection<DoUong>("DoUong");           
            var product = sp.Find(d => d._id == productId).FirstOrDefault();
            return product;
        }
        [HttpPost]
        public IActionResult UpdateQuantity(string productId, int newQuantity)
        {
             var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
             var cartItem = cart.FirstOrDefault(item => item.MaSP == productId);

            if (cartItem != null)
            {
                 var product = Laysp(productId);

                 if (newQuantity <= product.SoLuong)
                {
                     cartItem.Soluong = newQuantity;
                }
                else
                {
                     if(newQuantity > product.SoLuong)
                    {
                        ViewBag.QuantityErr = "Bạn Hãy nhập số lượng nhỏ hơn" + product.SoLuong;
                        cartItem.Soluong = product.SoLuong;
                    }
                    else 
                    {
                        ViewBag.Quantity = "Cập Nhật Thành Công";
                    }
                }
            }

            // Lưu giỏ hàng đã cập nhật vào Session
            HttpContext.Session.Set("Cart", cart);

            return RedirectToAction("Index");
        }

        public IActionResult ClearCart(string MaSP)
        {
            
            HttpContext.Session.Remove("Cart");
            HttpContext.Session.Remove("IsDiscountApplied");
            return RedirectToAction("Index");

        }
        public IActionResult DatHang()
        {
            DateTime currentUtcTime = DateTime.UtcNow;

            // Chuyển đổi thời gian từ UTC sang múi giờ của Việt Nam (GMT+7)
            DateTime currentDateVN = currentUtcTime.AddHours(7);


            ViewBag.CurrentDateVN = currentDateVN.ToString("dd/MM/yyyy HH:mm:ss");
            if ( HttpContext.Session.GetString("UserID") == null)
                return RedirectToAction("Login", "Login");
            List<CartItem> giohang = HttpContext.Session.Get<List<CartItem>>("Cart");
            if (giohang == null || giohang.Count == 0)
                return RedirectToAction("Index", "Food");
            KhachHang khach = HttpContext.Session.Get<KhachHang>("TaiKhoan");
            //  ViewBag.TongSL = TinhTongSL();
            // ViewBag.TongTien = TinhTongTien();
            ViewBag.TongTien = TinhTongTienvnd();
            ViewBag.TongT = TinhTongTien();
            return View(giohang);
        }
<<<<<<< HEAD
 
=======

    
     

         //đặt hàng
>>>>>>> dc570f7c3ab9b70b9d916b4baf05e4b44317fa1e
        public IActionResult HoanThanhDonHang()
        {
            return View();
        }

        //tính tổng tiền
        public decimal TinhTongTien()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            if (cart == null)
            {
                return 0;  
            }
            decimal total = (decimal)cart.Sum(item => item.Dongia * item.Soluong);

            return total;
        }

        private double TinhTongTienvnd()
        {
            double TongTien = 0;
            decimal b = 23000;
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart");
            if (cart != null)
            {
                TongTien = (double)cart.Sum(sp => sp.Total());
            }
            decimal c = (decimal)TongTien / b;
 
            int roundedResult = (int)Math.Round((double)c);
            return roundedResult;
        }


        public IActionResult DongYDatHang(bool isDirectPayment)
        {
            // Gọi phương thức Index để lấy danh sách CartItem
            // thanh toán trực tiếp
            if (isDirectPayment)
            {
                IActionResult result = Index();
                if (result is ViewResult viewResult)
                {
                    List<CartItem> giohang = viewResult.Model as List<CartItem>;

                    if (giohang == null)
                    {
                        return View("Error");
                    }
                    DateTime currentUtcTime = DateTime.UtcNow;
                    DateTime currentDateVN = currentUtcTime.AddHours(7);
                    // Tiếp tục xử lý giohang
                    var collection = _database.GetCollection<DonHang>("DonHang");
                    var collection_ = _database.GetCollection<CTDH>("ChiTietDonHang");
                    KhachHang kh = HttpContext.Session.Get<KhachHang>("taikhoan");
                    DonHang donhang = new DonHang();
                    donhang.MaKH = kh._id;
                    donhang.NgayDatHang = currentDateVN;
                    donhang.TongTien = TinhTongTien();
                    donhang.TinhTrangDonHang = false;
                    donhang.TenNguoiNhan = kh.HoTenKH;
                    donhang.DiaChiNhan = kh.DiaChi;
                    donhang.HinhThucThanhToan = true; // tiền mặt
                    donhang.HinhThucGiaoHang = false;
                    donhang.DaGiao = false; // shiper xác nhận đã giao hay chưa
                    collection.InsertOne(donhang);

                    foreach (var sanpham in giohang)
                    {
                        CTDH ct = new CTDH();
                        ct.MaDH = donhang._id;
                        ct.MaSP = sanpham.MaSP;
                        ct.SoLuong = sanpham.Soluong;
                        ct.GiaSP = sanpham.Dongia;
                        ct.ThanhTien = TinhTongTien();
                        collection_.InsertOne(ct);
                    }

                    HttpContext.Session.Remove("Cart");
         
                    return RedirectToAction("HoanThanhDonHang");
                }
                else
                {

                    return View("Error");

                }
            }
            else
            {
                return RedirectToAction("PaymentWithPaypal", "GioHang");
            }
        }
        //paypal
        private PayPal.Api.Payment payment;

        public IActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
 
                string payerId = Request.Query["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
  
                    string baseURI = $"{Request.Scheme}://{Request.Host}/GioHang/PaymentWithPayPal?";
 
                    var guid = Convert.ToString((new Random()).Next(100000));
  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                             paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    HttpContext.Session.SetString(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                     var guid = Request.Query["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, HttpContext.Session.GetString(guid));
                     if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            IActionResult result = Index();
            if (result is ViewResult viewResult)
            {
                List<CartItem> giohang = viewResult.Model as List<CartItem>;

                if (giohang == null)
                {
                    return View("Error");
                }

                // Tiếp tục xử lý giohang
                DateTime currentUtcTime = DateTime.UtcNow;
                DateTime currentDateVN = currentUtcTime.AddHours(7);
                // Tiếp tục xử lý giohang
                var collection = _database.GetCollection<DonHang>("DonHang");
                var collection_ = _database.GetCollection<CTDH>("ChiTietDonHang");
                KhachHang kh = HttpContext.Session.Get<KhachHang>("taikhoan");
                DonHang donhang = new DonHang();
                donhang.MaKH = kh._id;
                donhang.NgayDatHang = currentDateVN;
                donhang.TongTien = TinhTongTien();
                donhang.TinhTrangDonHang = true;
                donhang.TenNguoiNhan = kh.HoTenKH;
                donhang.DiaChiNhan = kh.DiaChi;
                donhang.HinhThucThanhToan = false;  
                donhang.HinhThucGiaoHang = false;
                donhang.DaGiao = false; // shiper xác nhận đã giao hay chưa
                collection.InsertOne(donhang);

                foreach (var sanpham in giohang)
                {
                    CTDH ct = new CTDH();
                    ct.MaDH = donhang._id;
                    ct.MaSP = sanpham.MaSP;
                    ct.SoLuong = sanpham.Soluong;
                    ct.GiaSP = sanpham.Dongia;
                    ct.ThanhTien = TinhTongTien();
                    collection_.InsertOne(ct);
                }

                HttpContext.Session.Remove("Cart");               
            }
            return View("HoanThanhDonHang");
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var totalAmount = TinhTongTienvnd().ToString("F2", CultureInfo.InvariantCulture);

            var itemList = new ItemList()
            {
                items = new List<Item>()
            };

            itemList.items.Add(new Item()
            {
                name = "Product",
                currency = "USD",  
                price = totalAmount,
                quantity = "1",
                sku = "sku"
            });

            var payer = new Payer()
            {
                payment_method = "paypal"
            };

            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };



            var details = new Details()
            {
                tax = "0.00", 
                shipping = "0.00",  
                subtotal = totalAmount 
            };

            var amount = new Amount()
            {
                currency = "USD",
                total = totalAmount,
                details = details
            };

            var transactionList = new List<Transaction>();
            var paypalOrderId = DateTime.Now.Ticks;

            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(),
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            return this.payment.Create(apiContext);
        }

    }
}
