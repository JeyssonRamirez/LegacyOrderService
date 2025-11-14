using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegacyOrderService.Models
{
    public class Order :Entity

    {
        
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal NewPrice { get; set; }
        public decimal Total { get; set; }
        public long ProductId { get; set; }        
        public Product Product { get; set; }
    }
}
