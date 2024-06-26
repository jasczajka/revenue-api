using revenue_api.Helpers;
using revenue_api.Models.Auth;
using revenue_api.Repositories;

namespace RevenueApiTests.Fakes;

public class FakeUserRepository : IUserRepository
{
    private readonly List<AppUser> _users;

    public FakeUserRepository()
    {
        var hashedPasswordSaltUser = SecurityHelpers.GetHashedPasswordAndSalt("userpassword");
        var hashedPasswordSaltAdmin = SecurityHelpers.GetHashedPasswordAndSalt("adminpassword");
        
        // Initialize with some fake data
        _users = new List<AppUser>
        {
            
            new AppUser
            {
                Email = "user@example.com",
                Login = "user",
                Password = hashedPasswordSaltUser.Item1,
                Salt = hashedPasswordSaltUser.Item2,
                RefreshToken = SecurityHelpers.GenerateRefreshToken(),
                RefreshTokenExp = DateTime.Now.AddDays(1),
                Role = Role.User
            },
            new AppUser
            {
                Email = "admin@example.com",
                Login = "admin",
                Password = hashedPasswordSaltAdmin.Item1,
                Salt = hashedPasswordSaltAdmin.Item2,
                RefreshToken = SecurityHelpers.GenerateRefreshToken(),
                RefreshTokenExp = DateTime.Now.AddDays(1),
                Role = Role.Admin
            }
        };
    }

    public Task<AppUser?> GetUserByLoginAsync(string login, CancellationToken cancellationToken)
    {
        var user = _users.FirstOrDefault(u => u.Login == login);
        return Task.FromResult(user);
    }

    public Task<AppUser?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var user = _users.FirstOrDefault(u => u.RefreshToken == refreshToken);
        return Task.FromResult(user);
    }

    public Task<AppUser> RegisterUserToDbAsync(RegisterRequest registerRequest,
        Tuple<string, string> hashedPasswordAndSalt, CancellationToken cancellationToken)
    {
        var user = new AppUser
        {
            Email = registerRequest.Email,
            Login = registerRequest.Login,
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelpers.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.Now.AddDays(1),
            Role = Role.User
        };

        _users.Add(user);
        return Task.FromResult(user);
    }
}