using revenue_api.Models;
using revenue_api.Models.Auth;
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
    public Task IssuePaymentForContractAsync(IssueContractPaymentRequestDto contractPaymentInfo, CancellationToken cancellationToken);

    public Task IssuePaymentForSubscriptionAsync(IssueSubscriptionPaymentRequestDto subPaymentInfo, bool isNewSubcription,
        CancellationToken cancellationToken);
    public Task<NewContractReturnDto> CreateNewContractAsync (NewContractRequestDto newContractRequestDto, CancellationToken cancellationToken);
    public Task<NewSubscriptionReturnDto> CreateNewSubscriptionAsync (NewSubscriptionRequestDto newSubscriptionRequestDto, CancellationToken cancellationToken);
    public Task<ProductRevenueReturnDto> GetRevenueForProductAsync(int idProduct, bool calculateProjected,
         CancellationToken cancellationToken, string currencySymbol = "PLN");
    public Task<ClientRevenueReturnDto> GetRevenueForClientAsync(int idClient, bool calculateProjected,
        CancellationToken cancellationToken, string currencySymbol = "PLN");

    public Task MarkYesterdayExpiredSubscriptionsAsNotPaid(CancellationToken cancellationToken);
    public bool IsClientReturning(Client client);
    
    public decimal GetPriceForContractWithDiscountIncluded(Contract contract, Client client, Software software);
    public int GetTotalDiscountForContract(Contract contract, Client client, Software software);
    public decimal GetAmountPaidForContract(Contract contract);

    public int GetHighestDiscountForSoftwareAndContract(DateOnly contractStart, DateOnly contractEnd,
        Software software);

    public bool CheckIfClientAlreadyHasActiveContractOrSubscriptionForThisSoftware(Client client, int idSoftware);

    public bool CheckIfPaymentForSubscriptionOverdue(Subscription subscription, DateOnly paymentDate);
    
    
    Task<AppUser> RegisterUserAsync(RegisterRequest registerRequest, CancellationToken cancellationToken);
    Task<Tuple<string, string>> ValidateLoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken);
    Task<Tuple<string, string>> RefreshLoginAsync(RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken);
    
}