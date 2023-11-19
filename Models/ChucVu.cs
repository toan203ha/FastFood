using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace CNPM_NC_DoAnNhanh.Models
{
    public class ChucVu
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = ObjectId.GenerateNewId().ToString();// ObjectId cho trường _id trong MongoDB
        [BsonElement("TenCV")]
        [Required(ErrorMessage = "Tên chức vụ là bắt buộc.")]
        [StringLength(20, ErrorMessage = "Tên nhân viên không được vượt quá 20 ký tự.")]
        public string TenCV { get; set; } = "";
    }
}
