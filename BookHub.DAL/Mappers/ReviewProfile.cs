using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;

namespace BookHub.DAL.Mappers
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            // Map ReviewDto to ReviewEntity and ignore relationships that should not be mapped.
            CreateMap<ReviewDto, ReviewEntity>()
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            // Explicitly map ReviewEntity to ReviewDto
            CreateMap<ReviewEntity, ReviewDto>();
        }
    }
}
