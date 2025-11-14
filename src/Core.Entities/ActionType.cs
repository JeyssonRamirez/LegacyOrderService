using Crosscutting.Util.Extension;
using System.ComponentModel;

namespace LegacyOrderService.Models
{
    public enum ActionType
    {
        [Description("Undefined")]
        [StringValue("0")]
        Undefined = 0,
        [Description("Add")]
        [StringValue("1")]
        Add = 1,
        [Description("Update")]
        [StringValue("2")]
        Update = 2,
        [Description("Delete")]
        [StringValue("3")]
        Delete = 3,
    }

}
