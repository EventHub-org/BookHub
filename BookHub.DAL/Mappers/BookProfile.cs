using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;

namespace BookHub.DAL.Mappers
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<BookDto, BookEntity>()
                .ForMember(dest => dest.Reviews, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
