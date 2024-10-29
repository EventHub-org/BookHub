using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;



namespace BookHub.DAL.Mappers
{
    
    public class UserRegisterProfile : Profile
    {
        public UserRegisterProfile()
        {
            CreateMap<UserRegisterDto, UserEntity>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());
            CreateMap<UserEntity, UserDto>();
        }
    }
}
