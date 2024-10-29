using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;

namespace BookHub.DAL.Mappers
{
    public class BookCreateProfile : Profile
    {
        public BookCreateProfile()
        {
            CreateMap<BookCreateDto, BookEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
