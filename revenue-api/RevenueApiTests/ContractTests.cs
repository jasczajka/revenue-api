using Microsoft.Extensions.Configuration;
using revenue_api.Exceptions;
using revenue_api.Models;
using revenue_api.Models.Dtos.RequestDtos;
using revenue_api.Repositories;
using revenue_api.Services;
using RevenueApiTests.Fakes;

namespace RevenueApiTests;

public class ContractTests
{
    private readonly IClientRepository _clientRepository;
    private readonly IContractRepository _contractRepository;
    private readonly ISoftwareRepository _softwareRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrencyExchangeService _currencyExchangeService;
    
    private readonly IRevenueService _revenueService;

    public ContractTests()
    {
        _clientRepository = new FakeClientRepository();
        _contractRepository = new FakeContractRepository();
        _softwareRepository = new FakeSoftwareRepository();
        _userRepository = new FakeUserRepository();
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) 
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        
        var httpClient = new HttpClient();
        _currencyExchangeService = new CurrencyExchangeService(httpClient, configuration);
        _revenueService = new RevenueService(_clientRepository, _contractRepository, _softwareRepository, _currencyExchangeService, _userRepository, configuration  );
    }
    
    [Fact]
    public void GetAmountPaidForContract_ReturnsCorrectAmount()
    {
        // Arrange
        var contract = _contractRepository.GetContractByIdAsync(1, CancellationToken.None).Result;

        // Act
        var result = _revenueService.GetAmountPaidForContract(contract);

        // Assert
        Assert.Equal(500, result);
    }
    [Fact]
    public void GetHighestDiscountForSoftwareAndContract_ReturnsCorrectDiscountWhenThereIsDiscount()
    {
        // Arrange
        var software = _softwareRepository.GetSoftwareByIdAsync(1, CancellationToken.None).Result;
        var contractStart = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
        var contractEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

        // Act
        var result = _revenueService.GetHighestDiscountForSoftwareAndContract(contractStart, contractEnd, software);

        // Assert
        Assert.Equal(15, result);
    }
    [Fact]
    public void GetHighestDiscountForSoftwareAndContract_ReturnsCorrectDiscountWhenThereIsNoDiscount()
    {
        // Arrange
        var software = _softwareRepository.GetSoftwareByIdAsync(2, CancellationToken.None).Result;
        var contractStart = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
        var contractEnd = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

        // Act
        var result = _revenueService.GetHighestDiscountForSoftwareAndContract(contractStart, contractEnd, software);

        // Assert
        Assert.Equal(0, result);
    }
    [Fact]
    public void GetTotalDiscountForContract_ReturnsCorrectDiscount()
    {
        // Arrange
        var client = new IndividualClient("12345678901")
        {
            ClientId = 999,
            FirstName = "John",
            LastName = "Doe",
            EmailAddress = "john.doe@example.com",
            PhoneNumber = "111-222-3333",
            Address = "456 Residential St, Hometown, HT 45678",
        };
        client.Contracts.Add(
            new Contract(DateOnly.FromDateTime(DateTime.Now.AddDays(-10)), DateOnly.FromDateTime(DateTime.Now.AddDays(-5)), 2, 1.0f,
                client,
                new Software
                {
                    Name = "Software A",
                    YearlyPrice = 1000
                })
            {
                ContractId = 999,
                IsSigned = true
            }
            );
        var newSoftware = new Software
        {
            Name = "Software B",
            YearlyPrice = 1000
        };
        var newContract = new Contract(DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
            DateOnly.FromDateTime(DateTime.Now.AddDays(-5)), 2, 1.0f,
            client,
            newSoftware)
        {
            ContractId = 1000
        };
        

        // Act
        var result = _revenueService.GetTotalDiscountForContract(newContract , client, newSoftware);

        // Assert
        Assert.Equal(5, result); // 5% for returning client
    }
    [Fact]
    public async Task GetPriceForContractWithDiscountIncluded_ReturnsCorrectPrice()
    {
        // Arrange
        var client = await _clientRepository.GetClientByIdAsync(1, CancellationToken.None);
        var software = await _softwareRepository.GetSoftwareByIdAsync(1, CancellationToken.None);
        var contract = await _contractRepository.GetContractByIdAsync(1, CancellationToken.None);

        // Act
        var result = _revenueService.GetPriceForContractWithDiscountIncluded(contract, client, software);

        // Assert
        Assert.Equal(2550, result); // 3000, 1000 for soft, 2 years of additional  - 15% (10% discount + 5% returning client)
    }
    [Fact]
    public async Task CheckIfClientAlreadyHasActiveContractForThisSoftware_ReturnsTrue_WhenClientHasActiveContract()
    {
        // Arrange
        var client = await _clientRepository.GetClientByIdAsync(1, CancellationToken.None);
        client.Contracts.Add(
                new Contract(DateOnly.FromDateTime(DateTime.Now.AddDays(-10)), DateOnly.FromDateTime(DateTime.Now.AddDays(-5)), 2, 1.0f,
                    client,
                    await _softwareRepository.GetSoftwareByIdAsync(1, CancellationToken.None))
                {
                    ContractId = 999,
                    IsSigned = true
                }
            );
        

        // Act
        var result = _revenueService.CheckIfClientAlreadyHasActiveContractForThisSoftware(client, 1);

        // Assert
        Assert.True(result);
    }
    [Fact]
    public async Task CreateNewContract_ThrowsNoSuchResourceException_WhenClientNotFound()
    {
        // Arrange
        var newContractRequestDto = new NewContractRequestDto { ClientId = 999, SoftwareId = 1, From = DateOnly.FromDateTime(DateTime.Now), To = DateOnly.FromDateTime(DateTime.Now.AddYears(1)), YearsOfUpdateSupport = 1, SoftwareVersion = 1.0f };

        // Act & Assert
        await Assert.ThrowsAsync<NoSuchResourceException>(() => _revenueService.CreateNewContractAsync(newContractRequestDto, CancellationToken.None));
    }
    [Fact]
    public async Task CreateNewContractAsync_ThrowsNoSuchResourceException_WhenSoftwareNotFound()
    {
        // Arrange
        var newContractRequestDto = new NewContractRequestDto { ClientId = 1, SoftwareId = 999, From = DateOnly.FromDateTime(DateTime.Now), To = DateOnly.FromDateTime(DateTime.Now.AddDays(30)), YearsOfUpdateSupport = 1, SoftwareVersion = 1.0f };

        // Act & Assert
        await Assert.ThrowsAsync<NoSuchResourceException>(() => _revenueService.CreateNewContractAsync(newContractRequestDto, CancellationToken.None));
    }

    [Fact]
    public async Task CreateNewContractAsync_ThrowsDomainException_WhenClientHasActiveContract()
    {
        // Arrange
        var newContractRequestDto = new NewContractRequestDto { ClientId = 5, SoftwareId = 1, From = DateOnly.FromDateTime(DateTime.Now), To = DateOnly.FromDateTime(DateTime.Now.AddYears(1)), YearsOfUpdateSupport = 1, SoftwareVersion = 1.0f };

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _revenueService.CreateNewContractAsync(newContractRequestDto, CancellationToken.None));
    }
    [Fact]
    public async Task CreateNewContractAsync_ThrowsDomainException_WhenYearsOfSupportNotInSupportedRange()
    {
        // Arrange
        var newContractRequestDto = new NewContractRequestDto { ClientId = 1, SoftwareId = 1, From = DateOnly.FromDateTime(DateTime.Now), To = DateOnly.FromDateTime(DateTime.Now.AddDays(4)), YearsOfUpdateSupport = 4, SoftwareVersion = 1.0f };

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _revenueService.CreateNewContractAsync(newContractRequestDto, CancellationToken.None));
    }
    [Fact]
    public async Task CreateNewContractAsync_CreatesContractWhenAllConditionsMet()
    {
        // Arrange
        var newContractRequestDto = new NewContractRequestDto { ClientId = 1, SoftwareId = 1, From = DateOnly.FromDateTime(DateTime.Now), To = DateOnly.FromDateTime(DateTime.Now.AddDays(4)), YearsOfUpdateSupport = 3, SoftwareVersion = 1.0f };
        
        // Act 

        var newContractId =
            (await _revenueService.CreateNewContractAsync(newContractRequestDto, CancellationToken.None)).ContractId;
        
        // Assert
        var contract = await _contractRepository.GetContractByIdAsync(newContractId, CancellationToken.None);
        Assert.NotNull(contract);
    }
    [Fact]
    public async Task IssuePaymentForContract_ThrowsNoSuchResourceException_WhenContractNotFound()
    {
        // Arrange
        var paymentInfo = new IssuePaymentRequestDto
        {
            ContractId = 999,
            Amount = 100,
            DateOfPayment = DateOnly.FromDateTime(DateTime.Now)
        };
        

        // Act & Assert
        await Assert.ThrowsAsync<NoSuchResourceException>(() => _revenueService.IssuePaymentForContractAsync(paymentInfo, CancellationToken.None));
    }
    [Fact]
    public async Task IssuePaymentForContract_ThrowsDomainException_WhenContractAlreadyPaidFor()
    {
        // Arrange
        var paymentInfo = new IssuePaymentRequestDto
        {
            ContractId = 3,
            Amount = 100,
            DateOfPayment = DateOnly.FromDateTime(DateTime.Now).AddDays(-6)
        };
        

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _revenueService.IssuePaymentForContractAsync(paymentInfo, CancellationToken.None));
    }
    
    [Fact]
    public async Task IssuePaymentForContractAsync_ThrowsContractOverdueException_WhenContractIsOverdue()
    {
        // Arrange
        var paymentInfo = new IssuePaymentRequestDto
        {
            ContractId = 4,
            Amount = 100,
            DateOfPayment = DateOnly.FromDateTime(DateTime.Now)
        };
        

        // Act & Assert
        await Assert.ThrowsAsync<ContractOverdueException>(() => _revenueService.IssuePaymentForContractAsync(paymentInfo, CancellationToken.None));
    }
    [Fact]
    public async Task CancelPaymentsForContractAsync_RemovesPaymentsCorrectly()
    {
        // Arrange
        Contract contractToCancelPayments = await _contractRepository.GetContractByIdAsync(1, CancellationToken.None);
        
        // Act
        var removedCount = _contractRepository
            .CancelPaymentsForContractAsync(contractToCancelPayments, CancellationToken.None).Result;
        
        // Assert
        Assert.Equal(2, removedCount);
        Assert.Empty(contractToCancelPayments.Payments);
    }
    [Fact]
    public async Task IssuePaymentForContractAsync_ProcessessPaymentCorrectly()
    {
        // Arrange
        var paymentInfo = new IssuePaymentRequestDto
        {
            ContractId = 5,
            Amount = 100,
            DateOfPayment = DateOnly.FromDateTime(DateTime.Now)
        };
        
        // Act

        await _revenueService.IssuePaymentForContractAsync(paymentInfo, CancellationToken.None);
        // Assert
        var contract = await _contractRepository.GetContractByIdAsync(5, CancellationToken.None );
        Assert.NotEmpty(contract.Payments);

        var amountPaid = _revenueService.GetAmountPaidForContract(contract);
        Assert.Equal(100m, amountPaid);
    }

    [Fact]
    public async Task ExchangeService_GetExchangeRateReturnsValue_WhenValidCurrencies()
    {
        string currencyFrom = "PLN";
        string currencyTo = "USD";
        
        
        var exchangeRate = await _currencyExchangeService.GetExchangeRate(currencyFrom, currencyTo);
        
        
        Assert.True(exchangeRate > 0);
    }
    [Fact]
    public async Task ExchangeService_ThrowsCurrencyExchangeExceptionWhen_FromCurrencyIncorrect()
    {
        string currencyFrom = "drfgsvdsgd";
        string currencyTo = "USD";
        
        
        
        
        await Assert.ThrowsAsync<CurrencyExchangeServiceException>(async () =>
        {
            await _currencyExchangeService.GetExchangeRate(currencyFrom, currencyTo);
        });
    }
    [Fact]
    public async Task ExchangeService_ThrowsCurrencyExchangeExceptionWhen_ToCurrencyIncorrect()
    {
        string currencyFrom = "PLN";
        string currencyTo = "drfgsvdsgd";
        
        
        
        
        await Assert.ThrowsAsync<CurrencyExchangeServiceException>(async () =>
        {
             await _currencyExchangeService.GetExchangeRate(currencyFrom, currencyTo);
        });
    }

    [Fact]
    public async Task GetRevenueForClientAsync_ThrowsNoSuchResourceExceptionWhenClientNotFound()
    {
        int notExistingClientId = 999;
        
        await Assert.ThrowsAsync<NoSuchResourceException>(() => _revenueService.GetRevenueForClientAsync(notExistingClientId,false,  CancellationToken.None));
    } 
    [Fact]
    public async Task GetRevenueForProductAsync_ThrowsNoSuchResourceExceptionWhenClientNotFound()
    {
        int notExistingProductId = 999;
        
        await Assert.ThrowsAsync<NoSuchResourceException>(() => _revenueService.GetRevenueForProductAsync(notExistingProductId,false,  CancellationToken.None));
    } 
    [Fact]
    public async Task GetRevenueForProductAsyncNotProjected_DoesNotIncludePaymentsWhenNotSigned()
    {
        int productId = 3;

        decimal revenue = (await _revenueService.GetRevenueForProductAsync(productId, false, CancellationToken.None))
            .Revenue;
        
        Assert.Equal(0, revenue);
        
    } 
    [Fact]
    public async Task GetRevenueForProductAsyncProjected_DoesIncludePaymentsWhenNotSigned()
    {
        int productId = 3;

        decimal revenue = (await _revenueService.GetRevenueForProductAsync(productId, true, CancellationToken.None))
            .Revenue;
        
        Assert.True(revenue > 0);
        
    } 
    [Fact]
    public async Task GetRevenueForClientAsyncNotProjected_DoesNotIncludePaymentsWhenNotSigned()
    {
        int clientId = 6;

        decimal revenue = (await _revenueService.GetRevenueForClientAsync(clientId, false, CancellationToken.None))
            .Revenue;
        
        Assert.Equal(0, revenue);
        
    } 
    [Fact]
    public async Task GetRevenueForClientAsyncProjected_DoesIncludePaymentsWhenNotSigned()
    {
        int clientId = 6;

        decimal revenue = (await _revenueService.GetRevenueForClientAsync(clientId, true, CancellationToken.None))
            .Revenue;
        
        Assert.True(revenue > 0);
        
    } 
    [Fact]
    public async Task ValidateLoginAsync_ValidUser_ShouldReturnTokens()
    {
        // Arrange
        var loginRequest = new revenue_api.Models.Auth.LoginRequest
        {
            Login = "user",
            Password = "userpassword"
        };

        // Act
        var result = await _revenueService.ValidateLoginAsync(loginRequest, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result.Item1)); // Access token
        Assert.False(string.IsNullOrEmpty(result.Item2)); // Refresh token
    }
    
}