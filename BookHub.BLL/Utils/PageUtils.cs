using BookHub.DAL.DTO;

namespace BookHub.BLL.Utils
{
    public class PageUtils
    {
        private PageUtils() { }
        public static ServiceResultType<PageDto<T>> ValidatePage<T>(Pageable pageable)
        {
            if (pageable.Size <= 0)
            {
                return ServiceResultType<PageDto<T>>.ErrorResult("Page size must be greater than zero.");
            }

            if (pageable.Page < 0)
            {
                return ServiceResultType<PageDto<T>>.ErrorResult("Page number must be greater than or equal to zero.");
            }

            return ServiceResultType<PageDto<T>>.SuccessResult(null);
        }
    }
}
