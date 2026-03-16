using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public record ImageDto(
 [MinLength(1)]
int Id ,
[Url]
[StringLength(100)]
 string Url,
[StringLength(40)]
string Title ,
[StringLength(40)]
string AltText );