
using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;

namespace BookHub.DAL.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, UserDto>()
                .ConstructUsing(src => new UserDto(src.UserId, src.Name, src.ProfilePicture))
                .ReverseMap();
        }
    }
}
