    using MongoDB.Driver;

    namespace CNPM_NC_DoAnNhanh.Models
    {
        public class CartItem
        {


        private readonly IMongoDatabase _database;

        public CartItem(IMongoDatabase database)
        {
            _database = database;
        }
        public string MaSP{ get; set; }
            public string Ten { get; set; }
            public string AnhBia { get; set; }
            public int Dongia { get; set; }
            public int Soluong { get; set; }

            public string MaCombo { get; set; }
            public string MaNuoc { get; set; }
            public string MaMonan { get; set; }
             public int newQuantity { get; set; } 
            public CartItem() { } 

            public CartItem(string productId, string productName, int price, int quantity)
            {
                MaSP = productId;
                Ten = productName;
                Dongia = price;
                Soluong = quantity;
            }
        public double Total()
        {
            return Soluong * Dongia;
        }
 
    }
    }
