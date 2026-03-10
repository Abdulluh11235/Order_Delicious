using System.ComponentModel.DataAnnotations;
using Domain.Entities.Abstract;
using Domain.Interfaces;

namespace Domain.Entities;

public class Item:TimeTrackable,IDiscountable, IAvailable
{
    [MinLength(1)]
    public int Id { get; set; }
    [StringLength(50)]
    public string Name { get; set; } = null!;
    [StringLength(200)]
    public string?  Description { get; set; }
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
    [Range(0, double.MaxValue)]
    public decimal DiscountRate { get; set; }
    public ICollection<Image> Images { get; set; } = new List<Image>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public bool IsAvailable { get; set; }
}