namespace Domain.Entities;

public class Menu
{
    public int Id { get; set; }
    public ICollection<Item> Items = new List<Item>();
    public decimal DiscountAmount { get; set; }
    public bool IsAvailable { get; set; }
}