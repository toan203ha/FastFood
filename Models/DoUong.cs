using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Microsoft.EntityFrameworkCore;

namespace CNPM_NC_DoAnNhanh.Models
{
    public class DoUong
    {
 

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = ObjectId.GenerateNewId().ToString();// ObjectId cho trường _id trong MongoDB
        [BsonElement("TenDoUong")]
        public string TenDoUong { get; set; } = "";
        [BsonElement("Img")]
        public string Img { get; set; } = "";
        [BsonElement("GiaTien")]
        public int GiaTien { get; set; } = 0;
        [BsonElement("SoLuong")]
        public int SoLuong { get; set; } = 0;
        [BsonElement("Size")]
        public string Size { get; set; } = "";
        [BsonElement("LoaiSanPham")] // Thêm trường để lưu loại sản phẩm
        public string LoaiSanPham { get; set; } = "";
 
    }
}
