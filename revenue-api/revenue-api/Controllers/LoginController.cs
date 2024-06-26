using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using revenue_api.Models.Auth;
using revenue_api.Services;

namespace revenue_api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    
    private readonly IRevenueService _revenueService;

    public LoginController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterRequest registerRequest, CancellationToken cancellationToken)
    {
        await _revenueService.RegisterUserAsync(registerRequest, cancellationToken);
        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        
        var (accessToken, refreshToken) = await _revenueService.ValidateLoginAsync(loginRequest, cancellationToken);
        
        return Ok(new
        {
            accessToken = accessToken,
            refreshToken = refreshToken
        });
        
        
    }

    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
    {
        
        var (accessToken, refreshToken) = await _revenueService.RefreshLoginAsync(refreshTokenRequest, cancellationToken);
        
        return Ok(new
        {
            accessToken = accessToken,
            refreshToken = refreshToken
        });
        
        
    }
}