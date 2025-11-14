using Crosscutting.Util.CustomValidators;
using LegacyOrderService.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Core.DataTransferObject.Order
{
    public class ModifyOrderDTO
    {
        [ConditionalValidation("ActionType", ActionType.Update, ActionType.Delete)]
        public long? Id { get; set; }

        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal NewPrice { get; set; }
        public long ProductId { get; set; }

        /// <summary>
        /// Ignore this property set automatically 
        /// </summary>
        [IgnoredForSwaggerAttribute]
        [SwaggerSchema(Description = "Action type please Ignore", ReadOnly = true)]
        public ActionType ActionType { get; set; }
        public decimal Total { get; set; }
    }
}
