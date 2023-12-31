﻿using CNPM_NC_DoAnNhanh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace CNPM_NC_DoAnNhanh.Controllers
{
    public class PhanLoaiController : Controller
    {
        private readonly IMongoDatabase _database;

        public PhanLoaiController(IMongoDatabase database)
        {
            _database = database;
        }
        //hiển thị danh sách
        public IActionResult Index()
        {
            var phanLoaiCollection = _database.GetCollection<PhanLoai>("PhanLoai");
            var danhSachLoaiSanPham = phanLoaiCollection.Find(_ => true).ToList();

            return View(danhSachLoaiSanPham);
        }
        public IActionResult Create()
        {
            return View();
        }

        // POST: Xử lý tạo loại sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhanLoai phanLoai)
        {
            if (ModelState.IsValid)
            {
                var phanLoaiCollection = _database.GetCollection<PhanLoai>("PhanLoai");
                phanLoaiCollection.InsertOne(phanLoai);
                return RedirectToAction("Index");
            }
            return View(phanLoai);
        }
        public IActionResult phanloai()
        {
            var phanLoaiCollection = _database.GetCollection<PhanLoai>("PhanLoai");
            var danhSachLoaiSanPham = phanLoaiCollection.Find(_ => true).ToList();

            return PartialView("DanhSachPartial", danhSachLoaiSanPham);
        }

        public IActionResult DanhSachSanPhamTheoDanhMuc(string id, string sortBy)
        {
            var productCollection = _database.GetCollection<DoUong>("DoUong");
            var danhSachSanPham = productCollection.Find(x => x.LoaiSanPham == id).ToList();
            ViewBag.Id=id;
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "name":
                        danhSachSanPham = danhSachSanPham.OrderBy(p => p.TenDoUong).ToList();
                        break;
                    case "price":
                        danhSachSanPham = danhSachSanPham.OrderBy(p => p.GiaTien).ToList();
                        break;
                    case "name_desc":
                        danhSachSanPham = danhSachSanPham.OrderByDescending(p => p.TenDoUong).ToList();
                        break;
                    case "price_desc":
                        danhSachSanPham = danhSachSanPham.OrderByDescending(p => p.GiaTien).ToList();
                        break;
                    default:
                        break;
                }
            }

            return View("DanhSachSanPham", danhSachSanPham);
        }

    }
}
