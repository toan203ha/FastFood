using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CNPM_NC_DoAnNhanh.Models
{
    public class KhachHang
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = ObjectId.GenerateNewId().ToString();// ObjectId cho trường _id trong MongoDB

        [BsonElement("HoTenKH")]
        public string HoTenKH { get; set; }
        [BsonElement("SDT")]
        public string SDT { get; set; }
        [BsonElement("DiaChi")]
        public string DiaChi { get; set; }
        [BsonElement("Vi")]
        public decimal Vi { get; set; }
        [BsonElement("TK")]
        public string TK { get; set; }
        [BsonElement("PASS")]
        public string PASS { get; set; }


    }

}
