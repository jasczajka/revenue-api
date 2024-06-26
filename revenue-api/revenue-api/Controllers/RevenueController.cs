using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using revenue_api.Services;

namespace revenue_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RevenueController : ControllerBase
{
    private readonly IRevenueService _revenueService;

    public RevenueController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }
    
    [Authorize]
    [HttpGet("product/{productId:int}")]
    public async Task<IActionResult> GetRevenueForProductAsync(int productId,
        CancellationToken cancellationToken, [FromQuery] bool calculateProjected = false, [FromQuery] string currencySymbol = "PLN" )
    {
        currencySymbol = currencySymbol.ToUpper();
        var returnDto =
            await _revenueService.GetRevenueForProductAsync(productId, calculateProjected, cancellationToken,
                currencySymbol);
        return Ok(returnDto);
    }
    
    [Authorize]
    [HttpGet("client/{clientId:int}")]
    public async Task<IActionResult> GetRevenueForClientAsync(int clientId,
        CancellationToken cancellationToken, [FromQuery] bool calculateProjected = false, [FromQuery] string currencySymbol = "PLN" )
    {
        currencySymbol = currencySymbol.ToUpper();
        var returnDto =
            await _revenueService.GetRevenueForClientAsync(clientId, calculateProjected, cancellationToken,
                currencySymbol);
        return Ok(returnDto);
    }
}