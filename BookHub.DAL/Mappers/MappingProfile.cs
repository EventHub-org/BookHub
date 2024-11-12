using AutoMapper;
using BookHub.DAL.Entities;
using BookHub.DAL.DTO;

public class MappingProfile : Profile
{
    public MappingProfile()
    {


        CreateMap<BookEntity, BookDto>();

        CreateMap<CollectionEntity, CollectionDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.UserId)) // Витягуємо UserId з User
                .ForMember(dest => dest.BookCount, opt => opt.MapFrom(src => src.Books != null ? src.Books.Count : 0)) // Додаємо маппінг для BookCount
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<UserEntity, UserDto>();

    }
}
