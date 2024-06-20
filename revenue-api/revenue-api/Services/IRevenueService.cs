using revenue_api.Models;
using revenue_api.Models.Dtos.RequestDtos;
using revenue_api.Models.Dtos.ReturnDtos;

namespace revenue_api.Services;

public interface IRevenueService
{
    public Task<ReturnCorporateClientDto> AddNewCorporateClientAsync(NewCorporateClientDto newCorporateClientInfo, CancellationToken cancellationToken);
    public Task<ReturnIndividualClientDto> AddNewIndividualClientAsync(NewIndividualClientDto newIndividualClientInfo, CancellationToken cancellationToken);
    public Task DeleteClientByIdAsync(int idClient, CancellationToken cancellationToken);
    public Task<ReturnCorporateClientDto> UpdateCorporateClientInfo(int idClient, UpdateCorporateClientDto newClientInfo, CancellationToken cancellationToken);
    public Task<ReturnIndividualClientDto> UpdateIndividualClientInfo(int idClient, UpdateIndividualClientDto newClientInfo, CancellationToken cancellationToken);
}