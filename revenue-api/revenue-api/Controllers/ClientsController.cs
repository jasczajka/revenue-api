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

    [HttpPost("add_corporate_client")]
    public async Task<IActionResult> AddCorporateClientAsync(NewCorporateClientDto newCorporateClientDto,
        CancellationToken cancellationToken)
    {
        var newClient = await _revenueService.AddNewCorporateClientAsync(newCorporateClientDto, cancellationToken);
        return Ok(newClient);
    }
    [HttpPost("add_individual_client")]
    public async Task<IActionResult> AddIndividualClientAsync(NewIndividualClientDto newIndividualClientDto,
        CancellationToken cancellationToken)
    {
        var newClient = await _revenueService.AddNewIndividualClientAsync(newIndividualClientDto, cancellationToken);
        return Ok(newClient);
    }

    [HttpDelete("{clientId:int}")]
    public async Task<IActionResult> DeleteClientAsync(int clientId, CancellationToken cancellationToken)
    {
        await _revenueService.DeleteClientByIdAsync(clientId, cancellationToken);
        return Ok();
    }

    [HttpPut("update_corporate_client/{clientId:int}")]
    public async Task<IActionResult> UpdateCorporateClientAsync(int clientId, UpdateCorporateClientDto newClientInfo,
        CancellationToken cancellationToken)
    {
        var updatedClient = await _revenueService.UpdateCorporateClientInfo(clientId, newClientInfo, cancellationToken);
        return Ok(updatedClient);
    }
    [HttpPut("update_individual_client/{clientId:int}")]
    public async Task<IActionResult> UpdateIndividualClientAsync(int clientId, UpdateIndividualClientDto newClientInfo,
        CancellationToken cancellationToken)
    {
        var updatedClient = await _revenueService.UpdateIndividualClientInfo(clientId, newClientInfo, cancellationToken);
        return Ok(updatedClient);
    }
}