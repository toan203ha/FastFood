using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using CNPM_NC_DoAnNhanh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using MongoDB.Driver;

namespace CNPM_NC_DoAnNhanh.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly IMongoCollection<DonHang> _donHangCollection;
        private readonly IMongoCollection<KhachHang> _khachHangCollection;

        public ThongKeController(IMongoDatabase database)
        {
            _donHangCollection = database.GetCollection<DonHang>("DonHang");
            _khachHangCollection = database.GetCollection<KhachHang>("KhachHang");
        }
        [HttpGet]
        public IActionResult TopKhachHangMuaNhieuNhat(DateTime? fromDate, DateTime? toDate)
        {
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;
            var query = _donHangCollection.AsQueryable();

         
            if (fromDate.HasValue)
            {
                query = (MongoDB.Driver.Linq.IMongoQueryable<DonHang>)query.Where(dh => dh.NgayDatHang >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = (MongoDB.Driver.Linq.IMongoQueryable<DonHang>)query.Where(dh => dh.NgayDatHang < toDate.Value.AddDays(1));
            }
            
            var topKhachHang = query
                .GroupBy(dh => dh.MaKH)
                .Select(group => new TopKhachHangModel
                {
                    MaKH = group.Key,
                    SoLuongDonHang = group.Count(),
                    TongTienMua = group.Sum(d => d.TongTien)
                })
                .OrderByDescending(kh => kh.SoLuongDonHang)
                .Take(10)
                .ToList();

            foreach (var khachHang in topKhachHang)
            {
                var thongTinKhachHang = _khachHangCollection.AsQueryable()
                    .FirstOrDefault(kh => kh._id == khachHang.MaKH);

                if (thongTinKhachHang != null)
                {
                    khachHang.TenKhachHang = thongTinKhachHang.HoTenKH;
                }
            }
  
             return View(topKhachHang.Any() ? topKhachHang : null);
        }
        public IActionResult BieuDo()
        {
            var tongTienData = _donHangCollection.Find(_ => true).ToList()
                .Select(donHang => new { donHang.NgayDatHang.Month, donHang.TongTien });

            return View(tongTienData);
        }


        public IActionResult ThongKeTopKhachHangVaBieuDo(DateTime? fromDate, DateTime? toDate)
        {
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

            var query = _donHangCollection.AsQueryable();

            if (fromDate.HasValue)
            {
                query = (MongoDB.Driver.Linq.IMongoQueryable<DonHang>)query.Where(dh => dh.NgayDatHang >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = (MongoDB.Driver.Linq.IMongoQueryable<DonHang>)query.Where(dh => dh.NgayDatHang < toDate.Value.AddDays(1));
            }

            var topKhachHang = query
                .GroupBy(dh => dh.MaKH)
                .Select(group => new TopKhachHangModel
                {
                    MaKH = group.Key,
                    SoLuongDonHang = group.Count(),
                    TongTienMua = group.Sum(d => d.TongTien)
                })
                .OrderByDescending(kh => kh.SoLuongDonHang)
                .Take(10)
                .ToList();

            foreach (var khachHang in topKhachHang)
            {
                var thongTinKhachHang = _khachHangCollection.AsQueryable()
                    .FirstOrDefault(kh => kh._id == khachHang.MaKH);

                if (thongTinKhachHang != null)
                {
                    khachHang.TenKhachHang = thongTinKhachHang.HoTenKH;
                }

            }

            var tongTienData = _donHangCollection.Find(_ => true)
     .SortByDescending(donHang => donHang.NgayDatHang)
     .ToList()
     .Select(donHang => new { NgayDatHang = donHang.NgayDatHang.Month, TongTien = donHang.TongTien });

            ViewBag.GroupedData = Newtonsoft.Json.JsonConvert.SerializeObject(tongTienData);


            var model = new BieuDo
            {
                TopKhachHang = topKhachHang,
                TongTienData = tongTienData.ToList()
            };
            ViewBag.TongTien = TinhTongTien();
            ViewBag.TongDH = TinhSoLuong();
            ViewBag.TongKH = TinhSLKH();

            return View("ThongKeTopKhachHangVaBieuDo", model);
        }

        public decimal TinhTongTien()
        {
 
            var donhang = _donHangCollection.Find(_ => true).ToList();
            decimal x = donhang.Sum(i => i.TongTien);
            return x;
        }
        public int TinhSoLuong()
        {
            var donhang = _donHangCollection.Find(_ => true).ToList();
            int x = donhang.Count();
            return x;
        }
        public int TinhSLKH()
        {
            var khachhang = _donHangCollection.AsQueryable().Select(dh => dh.MaKH).Distinct().ToList();
            int kh = khachhang.Count;
            return kh;
        }


    }

}
