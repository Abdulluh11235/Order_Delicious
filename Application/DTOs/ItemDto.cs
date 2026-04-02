using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.DTOs;

public record ItemDto (
    [MinLength(1)] 
    int Id,
    [StringLength(50)]
    string Name ,
    [StringLength(200)]
    string?  Description,
    [Range(0, double.MaxValue)]
    decimal Price,
    [Range(0, double.MaxValue)]
    decimal DiscountRate,
    ICollection<ImageDto> Images ,
    ICollection<CategoryDto> Categories ,
    bool IsAvailable );