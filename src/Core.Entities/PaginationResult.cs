namespace LegacyOrderService.Models
{
    public class PaginationResult<T>
    {
        public IQueryable<T> Data { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }


}
