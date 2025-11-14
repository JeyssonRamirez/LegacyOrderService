using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegacyOrderService.Models
{
    /// <summary>
    /// Base Table
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// Table ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        /// <summary>
        /// Creation Date
        /// </summary>
        public DateTime RegistrationDate { get; set; }
        /// <summary>
        /// Record Status
        /// </summary>
        public StatusType Status { get; set; }


    }

}
