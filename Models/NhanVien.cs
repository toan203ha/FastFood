using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace CNPM_NC_DoAnNhanh.Models
{
    public class NhanVien
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("TenNV")]
        [StringLength(20, ErrorMessage = "Tên nhân viên không được vượt quá 20 ký tự.")]
        [Required(ErrorMessage = "Tên nhân viên là bắt buộc.")]
        public string TenNV { get; set; }

        [BsonElement("MaCV")]
        [Required(ErrorMessage = "Mã chức vụ là bắt buộc.")]
        public string MaCV { get; set; } = "";

        [BsonElement("TK")]
        [Required(ErrorMessage = "Tài khoản là bắt buộc.")]
        [MinLength(5, ErrorMessage = "Tài khoản phải có ít nhất 5 ký tự.")]
        [MaxLength(15, ErrorMessage = "Tài khoản phải bé hơn 15 ký tự.")]
        public string TK { get; set; }
        [BsonElement("PASS")]
        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [MinLength(5, ErrorMessage = "Mật khẩu phải có ít nhất 5 ký tự.")]
        [MaxLength(15, ErrorMessage = "Mật khẩu phải bé hơn 15 ký tự.")]
        public string PASS { get; set; }
    }
}
