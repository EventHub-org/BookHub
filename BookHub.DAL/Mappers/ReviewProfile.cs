using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;

namespace BookHub.DAL.Mappers
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile() 
        {
            CreateMap<ReviewDto, ReviewEntity>()
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
