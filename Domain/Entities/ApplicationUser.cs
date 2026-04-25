using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;
public class ApplicationUser:IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}