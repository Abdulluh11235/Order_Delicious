using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Restaurant
{
   public int Id { get; set; }
   public Guid ApplicationUserId { get; set; }
   [Phone]
   public string PhoneNumber { get; set; } = null!;
   [MinLength(20)]
   public string Slogan { get; set; } = null!;
}