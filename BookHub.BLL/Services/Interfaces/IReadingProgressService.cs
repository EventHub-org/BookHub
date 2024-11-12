using BookHub.BLL.Utils;
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
        public Task<ServiceResultType<ReadingProgressResponseDTO>> CreateReadingProgressAsync(ReadingProgressDTO readingProgressDTO);
        public Task<ServiceResultType<ReadingProgressResponseDTO>> GetReadingProgressByIdAsync(int id);
        public Task<ServiceResultType<bool>> DeleteReadingProgressAsync(int id);
        public Task<ServiceResultType<ReadingProgressResponseDTO>> UpdateReadingProgressAsync(int id, ReadingProgressDTO readingProgressDTO);
        public Task<ServiceResultType<List<ReadingProgressResponseDTO>>> GetReadingProgressByUserIdAsync(int userId);

    }
}
