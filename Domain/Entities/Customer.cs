using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public Guid ApplicationUserId { get; set; }
    [Phone]
    public string PhoneNumber { get; set; } = null!;
    public int SpecialPoints { get; set; }
    public bool IsSpecial { get; set; }
    public int AddressId { get; set; }
    public Address Address { get; set; } = null!;
}