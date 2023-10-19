using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CNPM_NC_DoAnNhanh.Models
{
    public class MoAn
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = ObjectId.GenerateNewId().ToString();// ObjectId cho trường _id trong MongoDB
        [BsonElement("TenMonAn")]
        public string TenMonAn { get; set; } = "";
        [BsonElement("Img")]
        public string Img { get; set; } = "";
        [BsonElement("GiaTien")]
        public int GiaTien { get; set; } = 0;
        [BsonElement("SoLuong")]
        public int SoLuong { get; set; } = 0;
        [BsonElement("Size")]
        public string Size { get; set; } = "";

        //[BsonElement("AutoIncrementId")]
        //public int AutoIncrementId { get; set; }
    }
}
