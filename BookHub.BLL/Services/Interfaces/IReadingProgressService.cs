using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookHub.BLL.Services.Interfaces
{
    public interface IReadingProgressService
    {
        public Task<ReadingProgressEntity> createReadingProgress(ReadingProgressDTO readingProgressDTO);
    }
}
