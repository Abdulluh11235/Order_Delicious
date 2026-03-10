using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace Application.Commands;

public record CreateCategory(
[StringLength(40)]
string Name,
CreateImage Image );
