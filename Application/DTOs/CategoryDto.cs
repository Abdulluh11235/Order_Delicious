using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public record CategoryDto(
    [MinLength(1)] int Id,
    [StringLength(40)] string Name,
    ImageDto Image);
