using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Address
{
    public int Id { get; set; }
    public int  CountryId { get; set; }
    public Country Country { get; set; }
    public int  StateId { get; set; }
    public State State { get; set; }
    [MinLength(10),MaxLength(200)]
    public string Details { get; set; } = null!;
}