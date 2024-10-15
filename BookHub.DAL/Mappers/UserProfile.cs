
using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;

namespace BookHub.DAL.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, UserEntity>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePictureUrl))
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Achievments, opt => opt.Ignore())
                .ForMember(dest => dest.Collections, opt => opt.Ignore())
                .ForMember(dest => dest.Inviters, opt => opt.Ignore())
                .ForMember(dest => dest.Invitees, opt => opt.Ignore())
                .ReverseMap();

        }
    }
}
