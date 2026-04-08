using System.ComponentModel.DataAnnotations;

namespace Application.Commands;

public record UpdateImage([Url]
  [StringLength(100),Required]
    string Url,
    [StringLength(40),Required]
    string Title,
    [StringLength(40),Required]
    string AltText );