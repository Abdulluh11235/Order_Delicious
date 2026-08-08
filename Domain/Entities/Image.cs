using System.ComponentModel.DataAnnotations;
using Domain.Interfaces;

namespace Domain.Entities;

public class Image:IIdentifiable
{
    [MinLength(1)]
    public int Id { get; set; }
    [Url]
    [StringLength(400)]
    public string Url { get; set; } = null!;
    [StringLength(40)]
    public string Title { get; set; } = null!;
    [StringLength(40)]
    public string AltText { get; set; } = null!;
}