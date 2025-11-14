namespace LegacyOrderService.Models
{
    public class Order
    {
        public long Id;
        public string CustomerName;
        public string ProductName;
        public int Quantity;
        public decimal Price;
        public long? ProductId { get; set; }
        public Product Product { get; set; }
    }
}
