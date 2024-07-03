using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using revenue_api.Models.Dtos.RequestDtos;
using revenue_api.Services;

namespace revenue_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionsController : ControllerBase
{
    private readonly IRevenueService _revenueService;

    public SubscriptionsController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }
    [Authorize]
    [HttpPost()]
    public async Task<IActionResult> CreateNewSubscriptionAsync(NewSubscriptionRequestDto newSubscriptionRequestDto,
        CancellationToken cancellationToken)
    {
        var newSubscriptionInfo = await _revenueService.CreateNewSubscriptionAsync(newSubscriptionRequestDto, cancellationToken);
        return Ok(newSubscriptionInfo);
    }
    [Authorize]
    [HttpPost("subscriptions/{subscriptionId:int}/payments")]
    public async Task<IActionResult> IssuePaymentForSubscriptionAsync(int subscriptionId, IssueSubscriptionPaymentRequestDto subscriptionPaymentInfo,
        CancellationToken cancellationToken)
    {
        if (subscriptionId != subscriptionPaymentInfo.SubscriptionId)
        {
            return BadRequest("URL does not match the subscription"); 
        }
        await _revenueService.IssuePaymentForSubscriptionAsync(subscriptionPaymentInfo, false, cancellationToken);
        return Ok();
    }
}