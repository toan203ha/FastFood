using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CNPM_NC_DoAnNhanh.Models
{
    public class CTDH
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = ObjectId.GenerateNewId().ToString();// ObjectId cho trường _id trong MongoDB
        [BsonElement("MaDH")]
        public string MaDH { get; set; }
        [BsonElement("MaSP")]
        public string MaSP { get; set; }
        [BsonElement("GiaSP")]
        public double GiaSP { get; set; }
        [BsonElement("SoLuong")]
        public int SoLuong { get; set; }
        [BsonElement("ThanhTien")]
        public decimal ThanhTien { get; set; }
        public List<DoUong> DoUong { get; set; }
        public List<DonHang> DonHang { get; set; }


    }
}
