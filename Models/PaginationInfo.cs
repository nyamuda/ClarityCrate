namespace Clarity_Crate.Models
{
    public class PaginationInfo<T>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public int TotalItems { get; set; }

        public List<T> Items { get; set; }
    }
}
