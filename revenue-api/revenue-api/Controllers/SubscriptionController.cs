using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using revenue_api.Models.Dtos.RequestDtos;
using revenue_api.Services;

namespace revenue_api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionController : ControllerBase
{
    private readonly IRevenueService _revenueService;

    public SubscriptionController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }
    [Authorize]
    [HttpPost("new_subscription")]
    public async Task<IActionResult> CreateNewSubscriptionAsync(NewSubscriptionRequestDto newSubscriptionRequestDto,
        CancellationToken cancellationToken)
    {
        var newSubscriptionInfo = await _revenueService.CreateNewSubscriptionAsync(newSubscriptionRequestDto, cancellationToken);
        return Ok(newSubscriptionInfo);
    }
    [Authorize]
    [HttpPost("issue_payment_for_subscription/{subscriptionId:int}")]
    public async Task<IActionResult> IssuePaymentForSubscriptionAsync(int subscriptionId, IssueSubscriptionPaymentRequestDto subscriptionPaymentInfo,
        CancellationToken cancellationToken)
    {
        if (subscriptionId != subscriptionPaymentInfo.SubscriptionId)
        {
            return BadRequest("URL does not match the contract"); 
        }
        await _revenueService.IssuePaymentForSubscriptionAsync(subscriptionPaymentInfo, false, cancellationToken);
        return Ok();
    }
}