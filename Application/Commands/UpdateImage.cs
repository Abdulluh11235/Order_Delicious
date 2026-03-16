using System.ComponentModel.DataAnnotations;

namespace Application.Commands;

public record UpdateImage([Url]
    [StringLength(100)]
    string Url,
    [StringLength(40)]
    string Title,
    [StringLength(40)]
    string AltText );