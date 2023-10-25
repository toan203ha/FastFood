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
        [BsonElement("MaNuoc")]
        public string MaMonAn { get; set; }
        [BsonElement("MaNuoc")]
        public string MaNuoc { get; set; }
        [BsonElement("MaCombo")]
        public string MaCombo { get; set; }
        [BsonElement("SoLuong")]
        public int SoLuong { get; set; }
        [BsonElement("DonGia")]
        public decimal DonGia { get; set; }
        [BsonElement("ThanhTien")]
        public decimal ThanhTien { get; set; }
        

    }
}
