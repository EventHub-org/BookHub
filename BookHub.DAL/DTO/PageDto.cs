namespace BookHub.DAL.DTO
{
    public class PageDto<T>
    {
        public List<T> Items { get; set; }
        public long TotalElements { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public PageDto()
        {
            Items = new List<T>();
        }

        public PageDto(List<T> items, long totalElements, int currentPage, int totalPages)
        {
            Items = items;
            TotalElements = totalElements;
            CurrentPage = currentPage;
            TotalPages = totalPages;
        }
    }
}
