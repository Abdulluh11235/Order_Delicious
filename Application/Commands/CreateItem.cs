using System.ComponentModel.DataAnnotations;

namespace Application.Commands;

public record CreateItem(
 [StringLength(50),Required]
string Name ,
[StringLength(200),Required]
string?  Description ,
[Range(0, double.MaxValue),Required]
decimal Price ,
[MinLength(1),Required]
IEnumerable<CreateImage> Images,
[MinLength(1),Required]
IEnumerable<int> CategoryIds,
[Range(0,1),Required]
decimal DiscountRate,
[Required]
bool IsAvailable );
