
using revenue_api.Models.Auth;

namespace revenue_api.Repositories;

public interface IUserRepository
{
    Task<AppUser> RegisterUserToDbAsync(RegisterRequest registerRequest, Tuple<string, string> hashedPasswordAndSal, CancellationToken cancellationToken);
    Task<AppUser?> GetUserByLoginAsync(string login, CancellationToken cancellationToken);
    Task<AppUser?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}