using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application;

public class RegisterModel
{
 [Required,StringLength(50)] public string FirstName { get; set; } = null!;

 [Required, StringLength(50)] public string LastName { get; set; } = null!;
 [Required, StringLength(50)] public string Username { get; set; } = null!;
 [Required, EmailAddress, StringLength(128)]
 public string Email { get; set; } = null!;
 [Required, PasswordPropertyText, StringLength(128)]
 public string Password { get; set; } = null!;
}