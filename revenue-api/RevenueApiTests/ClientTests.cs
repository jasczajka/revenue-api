using revenue_api.Exceptions;
using revenue_api.Models;
using revenue_api.Models.Dtos.RequestDtos;
using revenue_api.Models.Dtos.ReturnDtos;
using revenue_api.Repositories;
using revenue_api.Services;
using RevenueApiTests.Fakes;

namespace RevenueApiTests;

public class ClientTests
{
    private readonly IClientRepository _clientRepository;
    private readonly IRevenueService _revenueService;

    public ClientTests()
    {
        _clientRepository = new FakeClientRepository();
        _revenueService = new RevenueService(_clientRepository);
    }
    [Fact]
    public async Task AddNewCorporateClientAsync_ShouldThrowException_WhenKrsIsNotUnique()
    {
        // Arrange
        var newCorporateClientDto = new NewCorporateClientDto
        {
            KRS = "12345678901234",
            CompanyName = "New Company",
            EmailAddress = "newcompany@example.com",
            PhoneNumber = "999-888-7777",
            Address = "101 New Street, New City, NC 10101"
        };
        
        // Act and Assert
        await Assert.ThrowsAsync<DomainException>(async () =>
        {
            await _revenueService.AddNewCorporateClientAsync(newCorporateClientDto, CancellationToken.None);
        });
    }
    [Fact]
    public async Task AddNewCorporateClientAsync_ShouldAddClient_WhenKrsIsUnique()
    {
        // Arrange
        var newCorporateClientDto = new NewCorporateClientDto
        {
            KRS = "12312312312312",
            CompanyName = "New Company",
            EmailAddress = "newcompany@example.com",
            PhoneNumber = "999-888-7777",
            Address = "101 New Street, New City, NC 10101"
        };

        // Act
        var newClient = await _revenueService.AddNewCorporateClientAsync(newCorporateClientDto, CancellationToken.None);

        // Assert
        Assert.NotNull(newClient);
        Assert.IsType<ReturnCorporateClientDto>(newClient);
        Assert.Equal(newCorporateClientDto.KRS, newClient.KRS);
    }
    
    [Fact]
    public async Task AddNewIndividualClientAsync_ShouldAddClient_WhenPeselIsUnique()
    {
        // Arrange
        var newIndividualClientDto = new NewIndividualClientDto
        {
            Pesel = "55555555555",
            FirstName = "NewFirstName",
            LastName = "NewLastName",
            EmailAddress = "newindividual@example.com",
            PhoneNumber = "777-888-9999",
            Address = "101 New Street, New City, NC 10101"
        };

        // Act
        var newClient = await _revenueService.AddNewIndividualClientAsync(newIndividualClientDto, CancellationToken.None);

        // Assert
        Assert.NotNull(newClient);
        Assert.IsType<ReturnIndividualClientDto>(newClient);
        Assert.Equal(newIndividualClientDto.Pesel, newClient.Pesel);
    }
    [Fact]
    public async Task AddNewIndividualClientAsync_ShouldThrowException_WhenPeselIsNotUnique()
    {
        // Arrange
        var newIndividualClientDto = new NewIndividualClientDto
        {
            Pesel = "12345678901", // existing PESEL
            FirstName = "ExistingFirstName",
            LastName = "ExistingLastName",
            EmailAddress = "existingindividual@example.com",
            PhoneNumber = "111-222-3333",
            Address = "202 Existing Street, Existing City, EC 20202"
        };

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(async () =>
            await _revenueService.AddNewIndividualClientAsync(newIndividualClientDto, CancellationToken.None));
    }
    [Fact]
    public async Task DeleteClientByIdAsync_ShouldSoftDeleteIndividualClient_WhenClientExists()
    {
        // Arrange
        int clientId = 3; // existing individual client

        // Act
        await _revenueService.DeleteClientByIdAsync(clientId, CancellationToken.None);

        // Assert
        var individualClient = await _clientRepository.GetIndividualClientByIdAsync(clientId, CancellationToken.None);
        Assert.Equal(true, individualClient.IsDeleted);
        Assert.NotNull(individualClient.DeletedOnUtc);
    }
    [Fact]
    public async Task DeleteClientByIdAsync_ShouldThrowException_WhenClientIsCorporate()
    {
        // Arrange
        int clientId = 1; // existing corporate client
        
        // Act and Assert
        await Assert.ThrowsAsync<DomainException>(async () =>
        {
            await _revenueService.DeleteClientByIdAsync(clientId, CancellationToken.None);
        });
    }
    [Fact]
    public async Task DeleteClientByIdAsync_ShouldThrowException_WhenClientDoesNotExist()
    {
        // Arrange
        int nonExistentClientId = 999;

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(async () =>
            await _revenueService.DeleteClientByIdAsync(nonExistentClientId, CancellationToken.None));
    }
    [Fact]
    public async Task UpdateCorporateClientInfo_ShouldUpdateClient_WhenClientExists()
    {
        // Arrange
        int clientId = 1; // existing corporate client
        var updateCorporateClientDto = new UpdateCorporateClientDto
        {
            CompanyName = "Updated Company",
            EmailAddress = "updatedcompany@example.com",
            PhoneNumber = "123-456-7890",
            Address = "Updated Address"
        };

        // Act
        var updatedClient = await _revenueService.UpdateCorporateClientInfo(clientId, updateCorporateClientDto, CancellationToken.None);

        // Assert
        Assert.NotNull(updatedClient);
        Assert.IsType<ReturnCorporateClientDto>(updatedClient);
        Assert.Equal(updateCorporateClientDto.CompanyName, updatedClient.CompanyName);
    }
    [Fact]
    public async Task UpdateIndividualClientInfo_ShouldUpdateClient_WhenClientExists()
    {
        // Arrange
        int clientId = 3; // existing individual client
        var updateIndividualClientDto = new UpdateIndividualClientDto
        {
            FirstName = "UpdatedFirstName",
            LastName = "UpdatedLastName",
            EmailAddress = "updatedindividual@example.com",
            PhoneNumber = "111-222-3333",
            Address = "Updated Address"
        };

        // Act
        var updatedClient = await _revenueService.UpdateIndividualClientInfo(clientId, updateIndividualClientDto, CancellationToken.None);

        // Assert
        Assert.NotNull(updatedClient);
        Assert.IsType<ReturnIndividualClientDto>(updatedClient);
        Assert.Equal(updateIndividualClientDto.FirstName, updatedClient.FirstName);
    }
    [Fact]
    public async Task UpdateCorporateClientInfo_ShouldThrowException_WhenClientNotExists()
    {
        // Arrange
        int clientId = 999; // existing corporate client
        var updateCorporateClientDto = new UpdateCorporateClientDto
        {
            CompanyName = "Updated Company",
            EmailAddress = "updatedcompany@example.com",
            PhoneNumber = "123-456-7890",
            Address = "Updated Address"
        };

       
        // Act and Assert
        await Assert.ThrowsAsync<DomainException>(async () =>
        {
            await _revenueService.UpdateCorporateClientInfo(clientId, updateCorporateClientDto, CancellationToken.None);
        });
    }

    [Fact]
    public async Task UpdateIndividualClientInfo_ShouldThrowException_WhenClientNotExists()
    {
        // Arrange
        int clientId = 9999; // existing individual client
        var updateIndividualClientDto = new UpdateIndividualClientDto
        {
            FirstName = "UpdatedFirstName",
            LastName = "UpdatedLastName",
            EmailAddress = "updatedindividual@example.com",
            PhoneNumber = "111-222-3333",
            Address = "Updated Address"
        };

        // Act and Assert
        await Assert.ThrowsAsync<DomainException>(async () =>
        {
            await _revenueService.UpdateIndividualClientInfo(clientId, updateIndividualClientDto,
                CancellationToken.None);
        });
    }
}