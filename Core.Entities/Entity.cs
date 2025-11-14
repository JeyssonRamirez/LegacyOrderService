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

    public class PaginationResult<T>
    {
        public IQueryable<T> Data { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }


}
