using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application;

public class TokenRequestModel
{
    
    [Required,EmailAddress] public string Email { get; set; } = null!;
    [Required,PasswordPropertyText] public string Password { get; set; } = null!;
}