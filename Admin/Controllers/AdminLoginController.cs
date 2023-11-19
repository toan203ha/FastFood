using Microsoft.AspNetCore.Mvc;
using CNPM_NC_DoAnNhanh.Models;
using MongoDB.Driver;
using CNPM_NC_DoAnNhanh.Controllers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Cache;

namespace CNPM_NC_DoAnNhanh.Admin.Controllers
{
    public class AdminLoginController : Controller
    {
        private readonly IMongoCollection<NhanVien> _nhanVienCollection;
        private readonly IMongoCollection<ChucVu> _chucVuCollection;
        public AdminLoginController(IMongoDatabase database)
        {
            _nhanVienCollection = database.GetCollection<NhanVien>("NhanVien");
            _chucVuCollection = database.GetCollection<ChucVu>("ChucVu");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(NhanVien nv)
        {
            var nhanVien = await _nhanVienCollection.Find(k => k.TK == nv.TK && k.PASS == nv.PASS).FirstOrDefaultAsync();
            if (nhanVien != null)
            {
                ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                HttpContext.Session.SetString("TK", JsonSerializer.Serialize(nhanVien));
                HttpContext.Session.SetString("UserID", nhanVien._id.ToString());
                HttpContext.Session.SetString("TenDN", nhanVien.TenNV);
                HttpContext.Session.SetString("MaCV", nhanVien.MaCV);

                if (HttpContext.Session.GetString("MaCV") == "65533b2cec6323eefbc519fc")
                    return RedirectToAction("Index", "KhachHangAdmin");
                else
                    return RedirectToAction("Index", "DoUong");
            }
            ViewBag.ErrorInfo = "Sai thông tin đăng nhập";
            return View(nv);
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUser(NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(nhanVien.TK))
                {
                    ModelState.AddModelError("TK", "Vui lòng nhập Tài Khoản.");
                }
                if (ModelState.IsValid)
                {
                    var existingUser = await _nhanVienCollection.Find(u => u.TK == nhanVien.TK).FirstOrDefaultAsync();
                    if (existingUser == null)
                    {
                        // Đăng ký thành công, thêm khách hàng vào MongoDB
                        await _nhanVienCollection.InsertOneAsync(nhanVien);
                        return RedirectToAction("Login", "Login");
                    }
                    else
                    {
                        ModelState.AddModelError("TK", "Tên đăng nhập đã tồn tại.");
                    }
                }
            }
            return View("RegisterUser", nhanVien);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("TK");
            HttpContext.Session.Remove("UserID");
            HttpContext.Session.Remove("TenDN");
            HttpContext.Session.Remove("MaCV");
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
