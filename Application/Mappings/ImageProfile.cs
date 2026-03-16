using Application.Commands;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class ImageProfile:Profile
{
    public ImageProfile()
    {
        CreateMap<CreateImage, Image>();
        CreateMap<UpdateImage, Image>();
         CreateMap<Image, ImageDto>();
      //   CreateMap<ImageDto, Image>();
    }
}