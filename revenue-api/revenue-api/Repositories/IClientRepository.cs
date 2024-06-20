using revenue_api.Models;
using revenue_api.Models.Dtos.RequestDtos;

namespace revenue_api.Repositories;

public interface IClientRepository
{
    public Task<Client?> GetClientByIdAsync(int idClient, CancellationToken cancellationToken);
    public Task<IndividualClient?> GetIndividualClientByIdAsync(int idClient, CancellationToken cancellationToken);
    public Task<CorporateClient?> GetCorporateClientByKrsAsync(string krs, CancellationToken cancellationToken);
    public Task<IndividualClient?> GetIndividualClientByPeselAsync(string pesel, CancellationToken cancellationToken);
    public Task<CorporateClient> AddNewCorporateClientAsync(NewCorporateClientDto newCorporateClientInfo, CancellationToken cancellationToken);
    public Task<IndividualClient> AddNewIndividualClientAsync(NewIndividualClientDto newIndividualClientInfo, CancellationToken cancellationToken);
    public Task DeleteClientByIdAsync(int idClient, CancellationToken cancellationToken);
    public Task<CorporateClient> UpdateCorporateClientInfo(CorporateClient clientToUpdate, UpdateCorporateClientDto newClientInfo, CancellationToken cancellationToken);
    public Task<IndividualClient> UpdateIndividualClientInfo(IndividualClient clientToUpdate, UpdateIndividualClientDto newClientInfo, CancellationToken cancellationToken);
}