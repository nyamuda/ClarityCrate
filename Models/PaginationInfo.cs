namespace Clarity_Crate.Models
{
    public class PaginationInfo<T>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public int TotalPages { get; set; }
        public List<T> Items { get; set; }
    }
}
