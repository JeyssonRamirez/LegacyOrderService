using System.ComponentModel;

namespace LegacyOrderService.Models
{
    public class Order
    {
        public string CustomerName;
        public string ProductName;
        public int Quantity;
        public double Price;
    }

    public enum StatusType
    {
        [Description("Active")]
        Active = 0,
        [Description("Pending")]
        Pending = 1,
        [Description("Inactive")]
        Inactive = 2,
        [Description("Locked")]
        Locked = 3,
        [Description("Deleted")]
        Deleted = 4,
        [Description("Other")]
        Other = 5
    }


}
