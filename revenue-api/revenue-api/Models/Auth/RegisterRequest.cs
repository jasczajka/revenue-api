namespace revenue_api.Models.Auth;
public class RegisterRequest
{
    public string Email { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}