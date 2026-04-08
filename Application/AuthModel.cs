namespace Application;

public class AuthModel
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public ICollection<string> Roles { get; set; } = new List<string>();
    public string Token { get; set; } = null!;
    public DateTime ExpiresOn { get; set; }
}