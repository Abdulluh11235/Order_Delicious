using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Category
{
    [MinLength(1)]
    public int Id { get; set; }
   
    [StringLength(40)]
    public string Name { get; set; } = null!;
    
    [MinLength(1)]
    public int ImageId { get; set; }
    public Image Image { get; set; } = null!;
}