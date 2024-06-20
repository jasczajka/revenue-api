using revenue_api.Exceptions;
using revenue_api.Models;
using revenue_api.Models.Dtos.RequestDtos;
using revenue_api.Models.Dtos.ReturnDtos;
using revenue_api.Repositories;

namespace revenue_api.Services;

public class RevenueService : IRevenueService
{
    private readonly IClientRepository _clientRepository;

    public RevenueService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    
    
    public async Task<ReturnCorporateClientDto> AddNewCorporateClientAsync(NewCorporateClientDto newCorporateClientInfo, CancellationToken cancellationToken)
    {
        var clientCheck = await _clientRepository.GetCorporateClientByKrsAsync(newCorporateClientInfo.KRS, cancellationToken);
        if (clientCheck != null)
        {
            throw new DomainException($"corporate client with krs no. {newCorporateClientInfo.KRS} already exists");
        }

        var newClient =  await _clientRepository.AddNewCorporateClientAsync(newCorporateClientInfo, cancellationToken);
        return new ReturnCorporateClientDto()
        {
            ClientId = newClient.ClientId,
            EmailAddress = newClient.EmailAddress,
            PhoneNumber = newClient.PhoneNumber,
            Address = newClient.Address,
            KRS = newClient.KRS,
            CompanyName = newClient.CompanyName
        };
    }

    public async Task<ReturnIndividualClientDto> AddNewIndividualClientAsync(NewIndividualClientDto newIndividualClientInfo,
        CancellationToken cancellationToken)
    {
        var clientCheck = await _clientRepository.GetIndividualClientByPeselAsync(newIndividualClientInfo.Pesel, cancellationToken);
        if (clientCheck != null)
        {
            throw new DomainException($"individual client with pesel no. {newIndividualClientInfo.Pesel} already exists");
        }

        var newClient =  await _clientRepository.AddNewIndividualClientAsync(newIndividualClientInfo, cancellationToken);
        return new ReturnIndividualClientDto()
        {
            ClientId = newClient.ClientId,
            EmailAddress = newClient.EmailAddress,
            PhoneNumber = newClient.PhoneNumber,
            Address = newClient.Address,
            Pesel = newClient.Pesel,
            FirstName = newClient.FirstName,
            LastName = newClient.LastName
        };
    }

    public async Task DeleteClientByIdAsync(int idClient, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetClientByIdAsync(idClient, cancellationToken);
        if (client == null)
        {
            throw new DomainException($"no client with id {idClient}");
        }

        if (client is CorporateClient)
        {
            throw new DomainException($"client with id {idClient} is a corporate client, such data cannot be deleted");
            
        }
        await _clientRepository.DeleteClientByIdAsync(idClient, cancellationToken);
    }

    public async Task<ReturnCorporateClientDto> UpdateCorporateClientInfo(int idClient, UpdateCorporateClientDto newClientInfo,
        CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetClientByIdAsync(idClient, cancellationToken);
        if (client == null)
        {
            throw new DomainException($"no client with id {idClient}");
        }

        if (!(client is CorporateClient corporateClient))
        {
            throw new DomainException($"client with id {idClient} is not a corporate client");
        }

        var changedClient = await _clientRepository.UpdateCorporateClientInfo(corporateClient, newClientInfo, cancellationToken);
        return new ReturnCorporateClientDto()
        {
            ClientId = changedClient.ClientId,
            EmailAddress = changedClient.EmailAddress,
            PhoneNumber = changedClient.PhoneNumber,
            Address = changedClient.Address,
            KRS = changedClient.KRS,
            CompanyName = changedClient.CompanyName
        };
    }

    public async Task<ReturnIndividualClientDto> UpdateIndividualClientInfo(int idClient, UpdateIndividualClientDto newClientInfo,
        CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetClientByIdAsync(idClient, cancellationToken);
        if (client == null)
        {
            throw new DomainException($"no client with id {idClient}");
        }

        if (!(client is IndividualClient individualClient))
        {
            throw new DomainException($"client with id {idClient} is not an individual client");
        }

        var changedClient = await _clientRepository.UpdateIndividualClientInfo(individualClient, newClientInfo, cancellationToken);
        return new ReturnIndividualClientDto()
        {
            ClientId = changedClient.ClientId,
            EmailAddress = changedClient.EmailAddress,
            PhoneNumber = changedClient.PhoneNumber,
            Address = changedClient.Address,
            Pesel = changedClient.Pesel,
            FirstName = changedClient.FirstName,
            LastName = changedClient.LastName
        };
    }
}