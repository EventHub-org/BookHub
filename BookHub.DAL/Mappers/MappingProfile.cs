using AutoMapper;
using BookHub.DAL.Entities;
using BookHub.DAL.DTO;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserEntity, UserDto>();
        CreateMap<BookEntity, BookDto>();
        // Add other mappings if necessary
    }
}
