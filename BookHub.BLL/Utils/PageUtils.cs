using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookHub.BLL.Utils
{
    public class PageUtils
    {
        private PageUtils() { }
        public static void ValidatePage(int size, int page)
        {
            if (size <= 0)
            {
                throw new ArgumentException("Page size must be greater than zero.", nameof(size));
            }

            if (page <= 0)
            {
                throw new ArgumentException("Page number must be greater than zero.", nameof(page));
            }
        }
    }
}
