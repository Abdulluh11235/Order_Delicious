using System.ComponentModel.DataAnnotations;

namespace Application.Commands;

public record UpdateItem(
    [StringLength(50)]
    string Name ,
    [StringLength(200)]
    string?  Description ,
    [Range(0, double.MaxValue)]
    decimal Price ,
    [MinLength(1)]
    IEnumerable<CreateImage> Images,
    [MinLength(1)]
    IEnumerable<int> CategoryIds,
    [Range(0,1)]
    decimal DiscountRate,
    bool IsAvailable );
