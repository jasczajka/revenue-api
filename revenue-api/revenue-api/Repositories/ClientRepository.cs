using Microsoft.EntityFrameworkCore;
using revenue_api.Models;
using revenue_api.Models.Dtos.RequestDtos;

namespace revenue_api.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public ClientRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Client?> GetClientByIdAsync(int idClient, CancellationToken cancellationToken)
    {
        var client = await _unitOfWork.GetDbContext().Clients.FirstOrDefaultAsync(c => c.ClientId == idClient, cancellationToken);
        return client;
    }

    public async Task<IndividualClient?> GetIndividualClientByIdAsync(int idClient, CancellationToken cancellationToken)
    {
        var individualClient = await _unitOfWork.GetDbContext().IndividualClients
            .FirstOrDefaultAsync(ic => ic.ClientId == idClient);
        return individualClient;
    }

    public async Task<CorporateClient?> GetCorporateClientByKrsAsync(string krs, CancellationToken cancellationToken)
    {
        var client = await _unitOfWork.GetDbContext().CorporateClients.FirstOrDefaultAsync(c => c.KRS == krs, cancellationToken);
        return client;
    }

    public async Task<IndividualClient?> GetIndividualClientByPeselAsync(string pesel, CancellationToken cancellationToken)
    {
        var client = await _unitOfWork.GetDbContext().IndividualClients.FirstOrDefaultAsync(c => c.Pesel == pesel, cancellationToken);
        return client;
    }

    public async Task<CorporateClient> AddNewCorporateClientAsync(NewCorporateClientDto newCorporateClientInfo, CancellationToken cancellationToken)
    {
        var newClient = new CorporateClient(newCorporateClientInfo.KRS)
        {
            Address = newCorporateClientInfo.Address,
            CompanyName = newCorporateClientInfo.CompanyName,
            EmailAddress = newCorporateClientInfo.EmailAddress,
            PhoneNumber = newCorporateClientInfo.PhoneNumber
        };
        await _unitOfWork.GetDbContext().Clients.AddAsync(newClient, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return newClient;
    }

    public async Task<IndividualClient> AddNewIndividualClientAsync(NewIndividualClientDto newIndividualClientInfo,
        CancellationToken cancellationToken)
    {
        var newClient = new IndividualClient(newIndividualClientInfo.Pesel)
        {
            FirstName = newIndividualClientInfo.FirstName,
            LastName = newIndividualClientInfo.LastName,
            Address = newIndividualClientInfo.Address,
            EmailAddress = newIndividualClientInfo.EmailAddress,
            PhoneNumber = newIndividualClientInfo.PhoneNumber
        };
        await _unitOfWork.GetDbContext().Clients.AddAsync(newClient, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return newClient;
    }

    public async Task DeleteClientByIdAsync(int idClient, CancellationToken cancellationToken)
    {
        //here we have checked in service that it is not a corporate client
        IndividualClient client = await _unitOfWork.GetDbContext().IndividualClients
            .FirstOrDefaultAsync(ic => ic.ClientId == idClient);
        client.IsDeleted = true;
        client.DeletedOnUtc = DateTime.Now;
        await _unitOfWork.CommitAsync(cancellationToken);
        
        
    }

    public async Task<CorporateClient> UpdateCorporateClientInfo(CorporateClient clientToUpdate,
        UpdateCorporateClientDto newClientInfo, CancellationToken cancellationToken)
    {
        clientToUpdate.CompanyName = newClientInfo.CompanyName;
        clientToUpdate.EmailAddress = newClientInfo.EmailAddress;
        clientToUpdate.PhoneNumber = newClientInfo.PhoneNumber;
        clientToUpdate.Address = newClientInfo.Address;
        await _unitOfWork.CommitAsync(cancellationToken);
        return clientToUpdate;
    }

    public async Task<IndividualClient> UpdateIndividualClientInfo(IndividualClient clientToUpdate,
        UpdateIndividualClientDto newClientInfo, CancellationToken cancellationToken)
    {
        clientToUpdate.FirstName = newClientInfo.FirstName;
        clientToUpdate.LastName = newClientInfo.LastName;
        clientToUpdate.EmailAddress = newClientInfo.EmailAddress;
        clientToUpdate.PhoneNumber = newClientInfo.PhoneNumber;
        clientToUpdate.Address = newClientInfo.Address;
        await _unitOfWork.CommitAsync(cancellationToken);
        return clientToUpdate;
    }
}