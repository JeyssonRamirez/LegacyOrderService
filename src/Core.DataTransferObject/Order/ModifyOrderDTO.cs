using Crosscutting.Util.CustomValidators;
using LegacyOrderService.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Core.DataTransferObject.Order
{
    public class ModifyOrderDTO
    {
        [ConditionalValidation("ActionType", ActionType.Update, ActionType.Delete)]
        public long? Id { get; set; }

        public string CustomerName;
        public string ProductName;
        public int Quantity;
        public double Price;
        public decimal NewPrice;
        public decimal Total;

        /// <summary>
        /// Ignore this property set automatically 
        /// </summary>
        [IgnoredForSwaggerAttribute]
        [SwaggerSchema(Description = "Action type please Ignore", ReadOnly = true)]
        public ActionType ActionType { get; set; }
        public long ProductId { get; set; }
    }
}
