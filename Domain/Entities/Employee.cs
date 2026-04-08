using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public Guid ApplicationUserId { get; set; }
    [Phone]
    public string PhoneNumber { get; set; } = null!;
    public int RestaurantBranchId { get; set; }
    public decimal Salary { get; set; }
}