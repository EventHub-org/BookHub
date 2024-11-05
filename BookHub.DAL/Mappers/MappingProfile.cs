using AutoMapper;
using BookHub.DAL.Entities;
using BookHub.DAL.DTO;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<BookEntity, BookDto>();

        CreateMap<CollectionEntity, CollectionDto>(); 

        CreateMap<UserEntity, UserDto>();

    }
}
