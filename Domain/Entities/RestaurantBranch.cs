using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class RestaurantBranch
{
    public int Id { get; set; }
    [MinLength(10)]
    public string Name { get; set; } = null!;
    public int AddressId { get; set; }
    public Address Address { get; set; } = null!;
    public int RestaurantId { get; set; }
    public ICollection<Menu> Menus { get; set; }=new List<Menu>();
}