using System.ComponentModel.DataAnnotations;

namespace Application.Commands;

public record UpdateItem(
    [StringLength(50),Required]
    string Name ,
    [StringLength(200),Required]
    string?  Description ,
    [Range(0, double.MaxValue),Required]
    decimal Price ,
    [MinLength(1),Required]
    IEnumerable<UpdateImage> Images,
    [MinLength(1),Required]
    IEnumerable<int> CategoryIds,
    [Range(0,1),Required]
    decimal DiscountRate,
    [Required]
    bool IsAvailable );
