using Application.Commands;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class CategoryProfile:Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategory, Category>();
        CreateMap<UpdateCategory, Category>();
        CreateMap<Category, CategoryDto>();
    }
}