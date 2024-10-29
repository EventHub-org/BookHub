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
                .ForMember(dest => dest.User1, opt => opt.MapFrom(src => new UserDto
                {
                    Id = src.User1.UserId,
                    Name = src.User1.Name,
                    ProfilePictureUrl = src.User1.ProfilePicture
                }))
                .ForMember(dest => dest.User2, opt => opt.MapFrom(src => new UserDto
                {
                    Id = src.User2.UserId,
                    Name = src.User2.Name,
                    ProfilePictureUrl = src.User2.ProfilePicture
                }))
                .ReverseMap()
                .ForPath(dest => dest.User1.UserId, opt => opt.MapFrom(src => src.User1.Id))
                .ForPath(dest => dest.User1.Name, opt => opt.MapFrom(src => src.User1.Name))
                .ForPath(dest => dest.User1.ProfilePicture, opt => opt.MapFrom(src => src.User1.ProfilePictureUrl))
                .ForPath(dest => dest.User2.UserId, opt => opt.MapFrom(src => src.User2.Id))
                .ForPath(dest => dest.User2.Name, opt => opt.MapFrom(src => src.User2.Name))
                .ForPath(dest => dest.User2.ProfilePicture, opt => opt.MapFrom(src => src.User2.ProfilePictureUrl));
        }
    }

}