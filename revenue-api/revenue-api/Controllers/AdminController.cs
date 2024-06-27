using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using revenue_api.Services;

namespace revenue_api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IRevenueService _revenueService;

    public AdminController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }
    [Authorize (Roles = "Admin")]
    [HttpPatch("refresh_subscriptions")]
    //ideally in practice would set up an external service to call this at 00:00 each day
    public async Task<IActionResult> RefreshSubscriptions(CancellationToken cancellationToken)
    {
        await _revenueService.MarkYesterdayExpiredSubscriptionsAsNotPaid(cancellationToken);
        return Ok();
    }
}