using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Interfaces;

namespace Domain.Entities;

public class Category:IIdentifiable
{
    [MinLength(1)]
    public int Id { get; set; }
   
    [StringLength(40)]
    //[unique]
    public string Name { get; set; } = null!;
    
    public int ImageId { get; set; }
    public Image Image { get; set; } = null!;
    public ICollection<Item> Items = new List<Item>();
}