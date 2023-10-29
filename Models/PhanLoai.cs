using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CNPM_NC_DoAnNhanh.Models
{
    public class PhanLoai
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = ObjectId.GenerateNewId().ToString();// ObjectId cho trường _id trong MongoDB
        [BsonElement("TenLoai")]
        public string TenLoai { get; set; } = "";
    }
}
