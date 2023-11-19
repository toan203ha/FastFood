using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CNPM_NC_DoAnNhanh.Models
{
    public class DonHang
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = ObjectId.GenerateNewId().ToString();// ObjectId cho trường _id trong MongoDB
        [BsonElement("MaKH")]
        public string MaKH { get; set; }
        [BsonElement("MaVoucher")]
        public string MaVoucher { get; set; }
        [BsonElement("NgayDatHang")]
        public DateTime NgayDatHang { get; set; }
        [BsonElement("TongTien")]
        [BsonRepresentation(BsonType.Decimal128)] // Đặt kiểu ObjectId cho trường MaCV
        public decimal TongTien { get; set; }
        [BsonElement("TinhTrangDonHang")]
        public Nullable<bool> TinhTrangDonHang { get; set; }
        [BsonElement("NgayGiaoHang")]
        public DateTime? NgayGiaoHang { get; set; }
        [BsonElement("TenNguoiNhan")]
        public string TenNguoiNhan { get; set; }
        [BsonElement("DiaChiNhan")]
        public string DiaChiNhan { get; set; }
        [BsonElement("HinhThucThanhToan")]
        public Nullable<bool> HinhThucThanhToan { get; set; }
        [BsonElement("HinhThucGiaoHang")]
        public Nullable<bool> HinhThucGiaoHang { get; set; }
        [BsonElement("DaGiao")]
        public Nullable<bool> DaGiao { get; set; }

    }

}
