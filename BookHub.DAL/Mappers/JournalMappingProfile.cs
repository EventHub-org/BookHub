using AutoMapper;
using BookHub.DAL.DTO;

namespace BookHub.DAL.Mappers
{
    public class JournalMappingProfile : Profile
    {
        public JournalMappingProfile()
        {
            CreateMap<BookDto, JournalEntryDto>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Progress, opt => opt.MapFrom(src => $"{src.NumberOfPages}/{src.NumberOfPages}")) 
                .ForMember(dest => dest.LastOpened, opt => opt.MapFrom(src => src.PublishedDate.ToString("dd.MM.yyyy"))); 
        }
    }
}
