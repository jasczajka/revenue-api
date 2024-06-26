using Microsoft.EntityFrameworkCore;
using revenue_api.Helpers;
using revenue_api.Models.Auth;

namespace revenue_api.Repositories;

public class UserRepository : IUserRepository
{
    
    private readonly IUnitOfWork _unitOfWork;

    public UserRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    
    
    public async Task<AppUser> RegisterUserToDbAsync(RegisterRequest registerRequest, Tuple<string, string> hashedPasswordAndSalt, CancellationToken cancellationToken)
    {
        var user = new AppUser()
        {
            Email = registerRequest.Email,
            Login = registerRequest.Login,
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelpers.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.Now.AddDays(1),
            Role = Role.User
        };

        _unitOfWork.GetDbContext().Users.Add(user);
        await _unitOfWork.CommitAsync(cancellationToken);
        return user;
    }

    public async Task<AppUser?> GetUserByLoginAsync(string login, CancellationToken cancellationToken)
    {
        AppUser? user = await _unitOfWork.GetDbContext().Users.Where(u => u.Login == login)
            .FirstOrDefaultAsync(cancellationToken);
        return user;
    }

    public async Task<AppUser?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        AppUser? user = await _unitOfWork.GetDbContext().Users.Where(u => u.RefreshToken == refreshToken)
            .FirstOrDefaultAsync(cancellationToken);
        return user;
    }
}