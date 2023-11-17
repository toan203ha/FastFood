using CNPM_NC_DoAnNhanh.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Text.Json;
using System.Threading.Tasks;

namespace CNPM_NC_DoAnNhanh.Controllers
{
    public class LoginController : Controller
    {
        private readonly IMongoCollection<KhachHang> _khachHangCollection;

        public LoginController(IMongoDatabase database)
        {
            _khachHangCollection = database.GetCollection<KhachHang>("KhachHang");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(KhachHang model)
        {
            var khachHang = await _khachHangCollection.Find(k => k.TK == model.TK && k.PASS == model.PASS).FirstOrDefaultAsync();

            if (khachHang != null)
            {
                ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                // Đăng nhập thành công, lưu thông tin người dùng vào Session
                //HttpContext.Session.SetString("taikhoan", khachHang.ToString());
                HttpContext.Session.SetString("taikhoan", JsonSerializer.Serialize(khachHang));
                HttpContext.Session.SetString("UserID", khachHang._id.ToString());
                HttpContext.Session.SetString("UserName", khachHang.HoTenKH);

                ViewBag.UserName = khachHang.HoTenKH;
                ViewBag.UserID = khachHang._id;
                return RedirectToAction("Index", "Home");
            }
            // Đăng nhập thất bại, hiển thị thông báo lỗi
            ViewBag.ErrorInfo = "Sai thông tin đăng nhập";
            return View(model);
        }


        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUser(KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(khachHang.TK))
                {
                    ModelState.AddModelError("TK", "Vui lòng nhập Tài Khoản.");
                }
                if (ModelState.IsValid)
                {
                    var existingUser = await _khachHangCollection.Find(u => u.TK == khachHang.TK).FirstOrDefaultAsync();
                    if (existingUser == null)
                    {
                        // Đăng ký thành công, thêm khách hàng vào MongoDB
                        await _khachHangCollection.InsertOneAsync(khachHang);
                        return RedirectToAction("Login", "Login");
                    }
                    else
                    {
                        ModelState.AddModelError("TK", "Tên đăng nhập đã tồn tại.");
                    }
                }
            }
            return View("RegisterUser", khachHang);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("taikhoan");
            HttpContext.Session.Remove("UserID");
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(KhachHang monAn)
        {
            if (ModelState.IsValid)
            {
                

                _khachHangCollection.InsertOne(monAn);
                return RedirectToAction("Login");
            }
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Lỗi");
            }
            return View(monAn);
        }


    }
}
