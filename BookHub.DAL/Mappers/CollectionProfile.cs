using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;

namespace BookHub.DAL.Mappers
{
    public class CollectionProfile : Profile
    {
        public CollectionProfile()
        {
            CreateMap<CollectionEntity, CollectionDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.UserId)) // Витягуємо UserId з User
                .ForMember(dest => dest.BookCount, opt => opt.MapFrom(src => src.Books != null ? src.Books.Count : 0)) // Додаємо маппінг для BookCount
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore()); // Ігноруємо User під час мапування назад
        }
    }
}
