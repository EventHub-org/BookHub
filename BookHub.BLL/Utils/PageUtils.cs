using BookHub.DAL.DTO;

namespace BookHub.BLL.Utils
{
    public class PageUtils
    {
        private PageUtils() { }
        public static void ValidatePage<T>(int size, int page)
        {
            if (size <= 0)
            {
                ServiceResultType<PageDto<T>>.ErrorResult("Page size must be greater than zero.");
            }

            if (page <= 0)
            {
                ServiceResultType<PageDto<T>>.ErrorResult("Page number must be greater than zero.");
            }
        }
    }
}
