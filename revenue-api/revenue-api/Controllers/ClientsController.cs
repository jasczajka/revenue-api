using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using revenue_api.Models.Dtos.RequestDtos;
using revenue_api.Services;

namespace revenue_api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IRevenueService _revenueService;

    public ClientsController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }

    [Authorize]
    [HttpPost("corporate_clients")]
    public async Task<IActionResult> AddCorporateClientAsync(NewCorporateClientDto newCorporateClientDto,
        CancellationToken cancellationToken)
    {
        var newClient = await _revenueService.AddNewCorporateClientAsync(newCorporateClientDto, cancellationToken);
        return Ok(newClient);
    }
    [Authorize]
    [HttpPost("individual_clients")]
    public async Task<IActionResult> AddIndividualClientAsync(NewIndividualClientDto newIndividualClientDto,
        CancellationToken cancellationToken)
    {
        var newClient = await _revenueService.AddNewIndividualClientAsync(newIndividualClientDto, cancellationToken);
        return Ok(newClient);
    }
    [Authorize (Roles = "Admin")]
    [HttpDelete("{clientId:int}")]
    public async Task<IActionResult> DeleteClientAsync(int clientId, CancellationToken cancellationToken)
    {
        await _revenueService.DeleteClientByIdAsync(clientId, cancellationToken);
        return Ok();
    }
    [Authorize (Roles = "Admin")]
    [HttpPut("corporate_clients/{clientId:int}")]
    public async Task<IActionResult> UpdateCorporateClientAsync(int clientId, UpdateCorporateClientDto newClientInfo,
        CancellationToken cancellationToken)
    {
        if (clientId != newClientInfo.ClientId)
        {
            return BadRequest("URL does not match the client"); 
        }
        var updatedClient = await _revenueService.UpdateCorporateClientInfo(clientId, newClientInfo, cancellationToken);
        return Ok(updatedClient);
    }
    [Authorize (Roles = "Admin")]
    [HttpPut("individual_clients/{clientId:int}")]
    public async Task<IActionResult> UpdateIndividualClientAsync(int clientId, UpdateIndividualClientDto newClientInfo,
        CancellationToken cancellationToken)
    {
        if (clientId != newClientInfo.ClientId)
        {
            return BadRequest("URL does not match the client"); 
        }
        var updatedClient = await _revenueService.UpdateIndividualClientInfo(clientId, newClientInfo, cancellationToken);
        return Ok(updatedClient);
    }

    
}