using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookHub.DAL.Mappers
{
    public class CollectionProfile : Profile
    {
        public CollectionProfile()
        {
            CreateMap<CollectionEntity, CollectionDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.UserId)) // Витягуємо UserId з User
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore()); // Ігноруємо User під час мапування назад
        }
    }

}