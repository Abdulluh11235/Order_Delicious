using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Image
{
    [MinLength(1)]
    public int Id { get; set; }
    [Url]
    [StringLength(100)]
    public string Url { get; set; } = null!;
    [StringLength(40)]
    public string Title { get; set; } = null!;
    [StringLength(40)]
    public string AltText { get; set; } = null!;
}