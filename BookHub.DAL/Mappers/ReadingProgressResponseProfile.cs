using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;

namespace BookHub.DAL.Mappers
{
    public class ReadingProgressResponseProfile : Profile
    {
        public ReadingProgressResponseProfile()
        {
            CreateMap<ReadingProgressEntity, ReadingProgressResponseDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
                .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage)) 
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status ?? "InProgress")) 
                .ForMember(dest => dest.DateFinished, opt => opt.MapFrom(src => src.DateFinished)) 
                .ReverseMap()
                .ForMember(dest => dest.Book, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}
