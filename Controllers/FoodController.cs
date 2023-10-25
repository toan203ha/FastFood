using CNPM_NC_DoAnNhanh.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Diagnostics;
using CNPM_NC_DoAnNhanh.Models;

namespace CNPM_NC_DoAnNhanh.Controllers
{
    public class FoodController : Controller
    {

   
        private readonly IMongoDatabase _database;

        public FoodController(IMongoDatabase database)
        {
            _database = database;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var collection = _database.GetCollection<MoAn>("MonAn");
            var monAnList = collection.Find(_ => true).ToList();
            return View(monAnList);
        }
        //[HttpGet]
        //public IActionResult Index_()
        //{

        //    var donHangCollection = _database.GetCollection<DonHang>("DonHang");
        //    var chiTietDonHangCollection = _database.GetCollection<ChiTietDonHang>("ChiTietDonHang");
        //    var pipeline = new BsonDocument[]
        //        {
        //            new BsonDocument("$lookup", new BsonDocument
        //            {
        //                { "from", "ChiTietDonHang" },
        //                { "localField", "_id" },
        //                { "foreignField", "MaDH" },
        //                { "as", "Data" }
        //            })
        //        };
        //    var result = donHangCollection.Aggregate<BsonDocument>(pipeline).ToList();
        //return View(result);
        //}




        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(MoAn monAn)
        {
            if (ModelState.IsValid)
            {
                // Tạo giá trị _id tự tăng
                //monAn._id = GetNextAutoIncrementValue().ToString();

                var collection = _database.GetCollection<MoAn>("MonAn");
                collection.InsertOne(monAn);
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Lỗi");
            }
            return View(monAn);
        }

        // Phương thức để lấy giá trị _id tự tăng
        //private int GetNextAutoIncrementValue()
        //{
        //    var collection = _database.GetCollection<MoAn>("MonAn");

        //    // Sắp xếp theo giảm dần của trường AutoIncrementId và lấy phần tử đầu tiên
        //    var lastDocument = collection.Find(_ => true)
        //        .SortByDescending(x => x.AutoIncrementId)
        //        .FirstOrDefault();

        //    if (lastDocument != null)
        //    {
        //        return lastDocument.AutoIncrementId + 1;
        //    }
        //    return 1; // Nếu không có dữ liệu, bắt đầu từ 1
        //}


        [HttpGet]
        public IActionResult Edit(string id)
        {
            var collection = _database.GetCollection<MoAn>("MonAn");
            var monAn = collection.Find(x => x._id == id).FirstOrDefault();

            if (monAn == null)
            {
                return NotFound();
            }

            return View(monAn);
        }

        [HttpPost]
        public IActionResult Edit(MoAn monAn)
        {
            if (ModelState.IsValid)
            {
                var collection = _database.GetCollection<MoAn>("MonAn");
                var filter = Builders<MoAn>.Filter.Eq("_id", monAn._id);
                var update = Builders<MoAn>.Update
                    .Set("TenMonAn", monAn.TenMonAn)
                    .Set("Img", monAn.Img)
                    .Set("GiaTien", monAn.GiaTien)
                    .Set("SoLuong", monAn.SoLuong)
                    .Set("Size", monAn.Size);

                var result = collection.UpdateOne(filter, update);

                if (result.IsAcknowledged && result.ModifiedCount > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // Xử lý lỗi
                }
            }
            return View(monAn);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var collection = _database.GetCollection<MoAn>("MonAn");
            var result = collection.DeleteOne(x => x._id == id);

            if (result.IsAcknowledged && result.DeletedCount > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Xử lý lỗi
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(string id)
        {
            var collection = _database.GetCollection<MoAn>("MonAn");
            var moAn = collection.Find(x => x._id == id).FirstOrDefault();

            if (moAn == null)
            {
                return NotFound();
            }

            return View(moAn);
        }


    }
}