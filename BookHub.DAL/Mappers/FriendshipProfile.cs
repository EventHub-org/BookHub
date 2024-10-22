using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;

namespace BookHub.DAL.Mappers
{
    public class FriendshipProfile : Profile
    {
        public FriendshipProfile()
        {
            CreateMap<FriendshipEntity, FriendshipDto>()
    .ForMember(dest => dest.User1, opt => opt.MapFrom(src => new UserDto(src.User1.UserId, src.User1.Name, src.User1.ProfilePicture)))
    .ForMember(dest => dest.User2, opt => opt.MapFrom(src => new UserDto(src.User2.UserId, src.User2.Name, src.User2.ProfilePicture)))
    .ReverseMap();
        }
    }

}