using Application.Commands;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class ItemProfile:Profile
{
    public ItemProfile()
    {
        CreateMap<CreateItem, Item>();
        CreateMap<Item, ItemDto>();
    }
}