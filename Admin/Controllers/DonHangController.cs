using CNPM_NC_DoAnNhanh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Driver;
using System.Net;

namespace CNPM_NC_DoAnNhanh.Admin.Controllers
{
    public class DonHangController : Controller
    {
  
        private readonly IMongoDatabase _database;

        public DonHangController(IMongoDatabase database)
        {
            _database = database;
        }
        public ActionResult Xacnhan(string? id)
        {
            var collection = _database.GetCollection<DonHang>("DonHang");
            var result = collection.Find(x => x._id == id).FirstOrDefault();
            if (id == null)
            {
                return NotFound();
            }

            if (result.TinhTrangDonHang == true)
            {
                var filter = Builders<DonHang>.Filter.Eq("_id", id);
                var update = Builders<DonHang>.Update
                    .Set("TinhTrangDonHang", false);
                var result_ = collection.UpdateOne(filter, update);
            }
            else if (result.TinhTrangDonHang == false)
            {
                 
                var filter = Builders<DonHang>.Filter.Eq("_id", id);
                var update = Builders<DonHang>.Update
                    .Set("TinhTrangDonHang", true);
                var result_ = collection.UpdateOne(filter, update);
            }
            if (result == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Xacnhan_kh(string? id)
        {
            var collection = _database.GetCollection<DonHang>("DonHang");
            var result = collection.Find(x => x._id == id).FirstOrDefault();
            if (id == null)
            {
                return NotFound();
            }

            //if (result.TinhTrangDonHang == true)
            //{
            //    var filter = Builders<DonHang>.Filter.Eq("_id", id);
            //    var update = Builders<DonHang>.Update
            //        .Set("DaGiao", false);
            //    var result_ = collection.UpdateOne(filter, update);
            //}
            //else if (result.TinhTrangDonHang == false)
            {

                var filter = Builders<DonHang>.Filter.Eq("_id", id);
                var update = Builders<DonHang>.Update
                    .Set("DaGiao", true);
                var result_ = collection.UpdateOne(filter, update);
            }
            if (result == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Index()
        {
            var Donhang = _database.GetCollection<DonHang>("DonHang");
            var danhsachDH = Donhang.Find(_ => true).ToList();
            return View(danhsachDH);
        }
        [HttpGet]
        public IActionResult Delete(string id)
        {
            var collection = _database.GetCollection<DonHang>("DonHang");
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
            var collection = _database.GetCollection<DonHang>("DonHang");
            var result = collection.Find(x => x._id == id).FirstOrDefault();

            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

    }
}
