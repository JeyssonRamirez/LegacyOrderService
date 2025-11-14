using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegacyOrderService.Models
{
    public class Order :Entity

    {
        
        public string CustomerName;
        public string ProductName;
        public int Quantity;
        public decimal Price;
        public decimal NewPrice;
        public long? ProductId { get; set; }
        public Product Product { get; set; }
    }
}
