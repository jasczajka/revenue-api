using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using revenue_api.Models.Dtos.RequestDtos;
using revenue_api.Services;

namespace revenue_api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ContractsController : ControllerBase
{
    private readonly IRevenueService _revenueService;

    public ContractsController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }
    
    [Authorize]
    [HttpPost("new_contract")]
    public async Task<IActionResult> CreateNewContractAsync(NewContractRequestDto newContractRequestDto,
        CancellationToken cancellationToken)
    {
        var newContractInfo = await _revenueService.CreateNewContractAsync(newContractRequestDto, cancellationToken);
        return Ok(newContractInfo);
    }
    
    [Authorize]
    [HttpPost("issue_payment_for_contract/{contractId:int}")]
    public async Task<IActionResult> IssuePaymentForContractAsync(int contractId, IssueContractPaymentRequestDto contractPaymentInfo,
        CancellationToken cancellationToken)
    {
        if (contractId != contractPaymentInfo.ContractId)
        {
            return BadRequest("URL does not match the contract"); 
        }
        await _revenueService.IssuePaymentForContractAsync(contractPaymentInfo, cancellationToken);
        return Ok();
    }
    
}