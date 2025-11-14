namespace LegacyOrderService.Models
{
    public class Product:Entity
    {
        
        public string Name { get; set; }
        public decimal Price { get; set; }

        public List<Order> Orders { get; set; }
    }


}
