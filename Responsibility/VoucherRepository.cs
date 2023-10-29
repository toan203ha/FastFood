using System;
using System.Collections.Generic;
using CNPM_NC_DoAnNhanh.Models;
using MongoDB.Driver;

public class VoucherRepository
{
    private readonly IMongoCollection<Vourcher> _voucherCollection;

    public VoucherRepository(IMongoDatabase database)
    {
        _voucherCollection = database.GetCollection<Vourcher>("Vourcher");
    }

     public Vourcher GetDiscountCodeByCode(string code)
        {
            return _voucherCollection.Find(x => x.TenVoucher == code).FirstOrDefault();
        }
}
