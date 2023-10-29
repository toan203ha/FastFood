using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CNPM_NC_DoAnNhanh.Models
{
    public class Vourcher
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = ObjectId.GenerateNewId().ToString();// ObjectId cho trường _id trong MongoDB

        [BsonElement("TenVoucher")]
        public string TenVoucher { get; set; }

        [BsonElement("GiaTriVoucher")]
        public double GiaTriVoucher { get; set; }

        [BsonElement("NgayBatDau")]
        public DateTime NgayBatDau { get; set; }

        [BsonElement("NgayKetThuc")]
        public DateTime NgayKetThuc { get; set; }

        [BsonElement("Img")]
        public string Img { get; set; }

        [BsonElement("MoTa")]
        public string MoTa { get; set; }
        // Thuộc tính lưu trạng thái mã giảm giá
        public bool IsUsed { get; set; } = false;

    }
}
