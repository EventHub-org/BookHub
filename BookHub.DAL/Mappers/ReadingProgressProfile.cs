﻿using AutoMapper;
using BookHub.DAL.DTO;
using BookHub.DAL.Entities;

public class ReadingProgressProfile : Profile
{
    public ReadingProgressProfile()
    {
        CreateMap<ReadingProgressDTO, ReadingProgressEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "InProgress"))
            .ForMember(dest => dest.DateFinished, opt => opt.Ignore())
            .ForMember(dest => dest.Book, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());
    }
}