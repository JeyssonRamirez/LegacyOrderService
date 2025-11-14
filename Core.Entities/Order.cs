namespace LegacyOrderService.Models
{
    public class Order 
    {
        public long Id;
        public string CustomerName;
        public string ProductName;
        public int Quantity;
        public decimal Price;
        public long ProductId { get; set; }
    }

    public class Product:Entity
    {
        
        public string Name { get; set; }
        public decimal Price { get; set; }
    }


}
