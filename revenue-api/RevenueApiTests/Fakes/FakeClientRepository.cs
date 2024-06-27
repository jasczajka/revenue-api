using revenue_api.Models;
using revenue_api.Models.Dtos.RequestDtos;
using revenue_api.Repositories;

namespace RevenueApiTests.Fakes;

public class FakeClientRepository : IClientRepository
{
    private readonly List<Client> _clients;


    private int _nextPaymentId;
    public static List<Client> GetTestClients()
    {
        return new FakeClientRepository()._clients;
    }

    public FakeClientRepository()
    {
        
        _clients = new List<Client>
        {
            new CorporateClient("12345678901234")
            {
                ClientId = 1,
                CompanyName = "Company A",
                EmailAddress = "companyA@example.com",
                PhoneNumber = "123-456-7890",
                Address = "123 Corporate Ave, Business City, BZ 12345"
            },
            new CorporateClient("98765432109876")
            {
                ClientId = 2,
                CompanyName = "Company B",
                EmailAddress = "companyB@example.com",
                PhoneNumber = "098-765-4321",
                Address = "987 Corporate Rd, Industry Town, IN 98765"
            },
            new IndividualClient("12345678901")
            {
                ClientId = 3,
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "john.doe@example.com",
                PhoneNumber = "111-222-3333",
                Address = "456 Residential St, Hometown, HT 45678",
            },
            new IndividualClient("10987654321")
            {
                ClientId = 4,
                FirstName = "Jane",
                LastName = "Doe",
                EmailAddress = "jane.doe@example.com",
                PhoneNumber = "444-555-6666",
                Address = "789 Suburb Ln, Suburbia, SB 78901",
            },
            new IndividualClient("10987654621")
            {
                ClientId = 5,
                FirstName = "Jane",
                LastName = "Duuf",
                EmailAddress = "jane.duuff@example.com",
                PhoneNumber = "444-555-6666",
                Address = "789 Suburb Ln, Suburbia, SB 78901",
            },
            new IndividualClient("16987654621")
            {
                ClientId = 6,
                FirstName = "Jan",
                LastName = "Kowalski",
                EmailAddress = "jan.kowalski@example.com",
                PhoneNumber = "444-434-6666",
                Address = "Batuty 8 Warszawa 02-791",
            },
            new IndividualClient("16987654621")
            {
                ClientId = 7,
                FirstName = "Subscription",
                LastName = "Client",
                EmailAddress = "subclient@example.com",
                PhoneNumber = "444-434-6666",
                Address = "Batuty 8 Warszawa 02-791",
            },
            new IndividualClient("16987654621")
            {
                ClientId = 8,
                FirstName = "Subscription",
                LastName = "Client2",
                EmailAddress = "subclient2@example.com",
                PhoneNumber = "444-434-6666",
                Address = "Batuty 8 Warszawa 02-791",
            },
            new IndividualClient("16981254621")
            {
                ClientId = 9,
                FirstName = "NoSubsNoContracts",
                LastName = "C",
                EmailAddress = "no@example.com",
                PhoneNumber = "444-434-6666",
                Address = "Batuty 8 Warszawa 02-791",
            },
            new IndividualClient("16987643621")
            {
                ClientId = 10,
                FirstName = "SubExpiredToday",
                LastName = "C",
                EmailAddress = "no@example.com",
                PhoneNumber = "444-434-6666",
                Address = "Batuty 8 Warszawa 02-791",
            },
            new IndividualClient("16987143621")
            {
                ClientId = 11,
                FirstName = "SubExpiresInTwoDays",
                LastName = "C",
                EmailAddress = "no@example.com",
                PhoneNumber = "444-434-6666",
                Address = "Batuty 8 Warszawa 02-791",
            },
            new IndividualClient("16987143621")
            {
                ClientId = 12,
                FirstName = "newSubTest",
                LastName = "C",
                EmailAddress = "no@example.com",
                PhoneNumber = "444-434-6666",
                Address = "Batuty 8 Warszawa 02-791",
            },
        };
        _clients.First(c => c.ClientId == 5).Contracts.Add(
            new Contract(DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
                DateOnly.FromDateTime(DateTime.Now.AddDays(-5)), 2, 1.0f,
                _clients.First(c => c.ClientId == 5),
                new Software
                {
                    SoftwareId = 1,
                    Name = "Software A",
                    YearlyPrice = 1000
                })
            {
                
                ContractId = 1,
                IsSigned = true,
            });
        _clients.First(c => c.ClientId == 6).Contracts.Add(
            new Contract(DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
                DateOnly.FromDateTime(DateTime.Now.AddDays(-5)), 2, 1.0f,
                _clients.First(c => c.ClientId == 6),
                new Software
                {
                    SoftwareId = 3,
                    Name = "Software A",
                    YearlyPrice = 1000
                })
            {
                
                ContractId = 6,
            });
        _clients.First(c => c.ClientId == 2).Contracts.Add(
            new Contract(DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
                DateOnly.FromDateTime(DateTime.Now.AddDays(15)), 2, 1.0f,
                _clients.First(c => c.ClientId == 2),
                new Software
                {
                    SoftwareId = 5,
                    Name = "Software A",
                    YearlyPrice = 1000
                })
            {
                
                ContractId = 6,
                IsSigned = true
            });
        
 
    }

    public Task<Client?> GetClientByIdAsync(int idClient, CancellationToken cancellationToken)
    {
        var client = _clients.FirstOrDefault(c => c.ClientId == idClient);
        return Task.FromResult(client);
    }
    public Task<IndividualClient?> GetIndividualClientByIdAsync(int idClient, CancellationToken cancellationToken)
    {
        var individualClient = _clients
            .OfType<IndividualClient>()
            .FirstOrDefault(ic => ic.ClientId == idClient);
        return Task.FromResult(individualClient);
    }
    public Task<CorporateClient?> GetCorporateClientByKrsAsync(string krs, CancellationToken cancellationToken)
    {
        var client = _clients.OfType<CorporateClient>().FirstOrDefault(c => c.KRS == krs);
        return Task.FromResult( client);
    }

    public Task<IndividualClient?> GetIndividualClientByPeselAsync(string pesel, CancellationToken cancellationToken)
    {
        var client = _clients.OfType<IndividualClient>().FirstOrDefault(c => c.Pesel == pesel);
        return Task.FromResult( client);
    }

    public Task<CorporateClient> AddNewCorporateClientAsync(NewCorporateClientDto newCorporateClientInfo, CancellationToken cancellationToken)
    {
        var newClient = new CorporateClient(newCorporateClientInfo.KRS)
        {
            ClientId = _clients.Count + 1,
            CompanyName = newCorporateClientInfo.CompanyName,
            EmailAddress = newCorporateClientInfo.EmailAddress,
            PhoneNumber = newCorporateClientInfo.PhoneNumber,
            Address = newCorporateClientInfo.Address
        };
        _clients.Add(newClient);
        return Task.FromResult(newClient);
    }

    public Task<IndividualClient> AddNewIndividualClientAsync(NewIndividualClientDto newIndividualClientInfo, CancellationToken cancellationToken)
    {
        var newClient = new IndividualClient(newIndividualClientInfo.Pesel)
        {
            ClientId = _clients.Count + 1,
            FirstName = newIndividualClientInfo.FirstName,
            LastName = newIndividualClientInfo.LastName,
            EmailAddress = newIndividualClientInfo.EmailAddress,
            PhoneNumber = newIndividualClientInfo.PhoneNumber,
            Address = newIndividualClientInfo.Address,
            IsDeleted = false
        };
        _clients.Add(newClient);
        return Task.FromResult(newClient);
    }

    public Task DeleteClientByIdAsync(int idClient, CancellationToken cancellationToken)
    {
        var client = (IndividualClient) _clients.FirstOrDefault(c => c.ClientId == idClient);
        
        if (client != null)
        {
            client.IsDeleted = true;
            client.DeletedOnUtc = DateTime.Now;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<CorporateClient> UpdateCorporateClientInfo(CorporateClient clientToUpdate, UpdateCorporateClientDto newClientInfo, CancellationToken cancellationToken)
    {
        clientToUpdate.CompanyName = newClientInfo.CompanyName;
        clientToUpdate.EmailAddress = newClientInfo.EmailAddress;
        clientToUpdate.PhoneNumber = newClientInfo.PhoneNumber;
        clientToUpdate.Address = newClientInfo.Address;
        return Task.FromResult(clientToUpdate);
    }

    public Task<IndividualClient> UpdateIndividualClientInfo(IndividualClient clientToUpdate, UpdateIndividualClientDto newClientInfo, CancellationToken cancellationToken)
    {
        clientToUpdate.FirstName = newClientInfo.FirstName;
        clientToUpdate.LastName = newClientInfo.LastName;
        clientToUpdate.EmailAddress = newClientInfo.EmailAddress;
        clientToUpdate.PhoneNumber = newClientInfo.PhoneNumber;
        clientToUpdate.Address = newClientInfo.Address;
        return Task.FromResult(clientToUpdate);
    }

    
}