namespace CNPM_NC_DoAnNhanh.Models
{
    public class CartItem
    {
        public string MaSP { get; set; }
        public string Ten { get; set; }
        public string AnhBia { get; set; }
        public Double Dongia { get; set; }
        public int Soluong { get; set; }

        public CartItem() { }

        public CartItem(string productId, string productName, double price, int quantity)
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

        public void ApplyDiscount(double discountAmount)
        {
            // Áp dụng giảm giá cho sản phẩm
            if (Dongia > discountAmount)
            {
                Dongia -= discountAmount;
            }
            else
            {
                Dongia = 0;
            }
        }
    }
}
