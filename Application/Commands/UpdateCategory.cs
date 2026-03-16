using System.ComponentModel.DataAnnotations;

namespace Application.Commands;

public record UpdateCategory(
    [StringLength(40)]
    string Name,
    UpdateImage Image );