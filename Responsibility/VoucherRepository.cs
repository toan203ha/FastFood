using System;
using System.Collections.Generic;
using CNPM_NC_DoAnNhanh.Models;
using MongoDB.Driver;

public class VoucherRepository
{
    private readonly IMongoCollection<Vourcher> _voucherCollection;

    public VoucherRepository(IMongoDatabase database)
    {
        _voucherCollection = database.GetCollection<Vourcher>("Vourchers");
    }

    public Vourcher GetVoucherByCode(string maVoucher)
    {
        var filter = Builders<Vourcher>.Filter.Eq(v => v._id, maVoucher);
        return _voucherCollection.Find(filter).SingleOrDefault();
    }
}
