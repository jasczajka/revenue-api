using System.ComponentModel.DataAnnotations;

namespace revenue_api.Models.Auth;

public class AppUser
{
    [Key]
    public int IdUser { get; set; }
    [Required]
    public string Login { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string RefreshToken { get; set; }
    public Role Role { get; set; }
    public DateTime? RefreshTokenExp { get; set; }
    
}