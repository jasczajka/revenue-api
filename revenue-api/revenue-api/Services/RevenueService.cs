using revenue_api.Exceptions;
using revenue_api.Models;
using revenue_api.Models.Dtos.RequestDtos;
using revenue_api.Models.Dtos.ReturnDtos;
using revenue_api.Repositories;

namespace revenue_api.Services;

public class RevenueService : IRevenueService
{
    private readonly IClientRepository _clientRepository;
    private readonly IContractRepository _contractRepository;
    private readonly ISoftwareRepository _softwareRepository;

    public RevenueService(IClientRepository clientRepository, IContractRepository contractRepository, ISoftwareRepository softwareRepository)
    {
        _clientRepository = clientRepository;
        _contractRepository = contractRepository;
        _softwareRepository = softwareRepository;
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
            throw new  NoSuchResourceException($"no client with id {idClient}");
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
            throw new NoSuchResourceException($"no client with id {idClient}");
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
            throw new NoSuchResourceException($"no client with id {idClient}");
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

    public async Task IssuePaymentForContractAsync(IssuePaymentRequestDto paymentInfo, CancellationToken cancellationToken)
    {
        
        var contract = await _contractRepository.GetContractByIdAsync(paymentInfo.ContractId, cancellationToken);
        if (contract == null)
        {
            throw new NoSuchResourceException($"no contract with id {paymentInfo.ContractId}");
        }

        var client = await _clientRepository.GetClientByIdAsync(contract.Client.ClientId, cancellationToken);
        if (client == null)
        {
            throw new NoSuchResourceException($"no client with id {contract.Client.ClientId}");
        }
        var software = await _softwareRepository.GetSoftwareByIdAsync(contract.Software.SoftwareId, cancellationToken);
        if (software == null)
        {
            throw new NoSuchResourceException($"no software with id {contract.Software.SoftwareId}");
        }
        decimal paidSoFar = GetAmountPaidForContract(contract);
        decimal priceToPay = GetPriceForContractWithDiscountIncluded(contract, client, software);
        if (paidSoFar >= priceToPay )
        {
            throw new DomainException($"contract {contract.ContractId} has already been paid for");
        }

        if (paidSoFar + paymentInfo.Amount > priceToPay)
        {
            throw new DomainException($"contract {contract.ContractId} should be paid in amount of {priceToPay}, this payment would exceed this amount"); 
        }

        if (paymentInfo.DateOfPayment > contract.To)
        {
            var cancelledPaymentsCount = await _contractRepository.CancelPaymentsForContractAsync(contract, cancellationToken);
            throw new ContractOverdueException($"contract id {contract.ContractId} payment window closed on {contract.To}, cancelled {cancelledPaymentsCount} previous payments");
        }
        bool isContractPaid = paidSoFar + paymentInfo.Amount == priceToPay;
        await _contractRepository.IssuePaymentForContractAsync(contract, paymentInfo.Amount, isContractPaid,  cancellationToken);
        

    }
    
    public async Task<NewContractReturnDto> CreateNewContractAsync(NewContractRequestDto newContractRequestDto,
        CancellationToken cancellationToken)
    {
        //  Tworząc umowę, upewnij się, że klient nie ma już aktywnej subskrypcji ani aktywnej umowy na ten produkt.
        var client =
            await _clientRepository.GetClientByIdAsync(newContractRequestDto.ClientId, cancellationToken);
        if (client == null)
        {
            throw new NoSuchResourceException($"no client with id {newContractRequestDto.ClientId}");
        }
        var software = await _softwareRepository.GetSoftwareByIdAsync(newContractRequestDto.SoftwareId, cancellationToken);
        if (software == null)
        {
            throw new NoSuchResourceException($"no software with id {newContractRequestDto.SoftwareId}");
        }
        if (CheckIfClientAlreadyHasActiveContractForThisSoftware(client, newContractRequestDto.SoftwareId))
        {
            throw new DomainException(
                $"client with id  ${client.ClientId} already has an active contract for software with id {newContractRequestDto.SoftwareId}");
        }

        if (!new List<int> { 0, 1, 2, 3 }.Contains(newContractRequestDto.YearsOfUpdateSupport))
        {
            throw new DomainException($"years of support must be 0, 1, 2, or 3");
        }
        var newContract = await _contractRepository.CreateNewContractAsync(
            newContractRequestDto.From,
            newContractRequestDto.To,
            newContractRequestDto.YearsOfUpdateSupport,
            newContractRequestDto.SoftwareVersion,
            client,
            software,
            cancellationToken);
        return new NewContractReturnDto()
        {
            ClientId = client.ClientId,
            ContractId = newContract.ContractId,
            From = newContract.From,
            To = newContract.From,
            SoftwareId = newContract.Software.SoftwareId,
            SoftwareVersion = newContract.SoftwareVersion,
            YearsOfUpdateSupport = newContract.YearsOfUpdateSupport
        };
    }

    public bool CheckIfClientAlreadyHasActiveContractForThisSoftware(Client client, int idSoftware)
    {
        //if bought a subscripotion within last year
        List<Contract> contractsForThisSoftware = client.Contracts
            .Where(c => c.Software.SoftwareId == idSoftware)
            .Where(c => c.IsSigned)
            .Where(c => DateOnly.FromDateTime(DateTime.Now) <= c.From.AddYears(1))
            .ToList();
        if (contractsForThisSoftware.Count >= 1)
        {
            return true;
        }

        return false;
    }
    

    public decimal GetAmountPaidForContract(Contract contract)
    {
        return contract.Payments
            .Sum(p => p.AmountPaid);
    }

    public int GetHighestDiscountForSoftwareAndContract(DateOnly contractStart, DateOnly contractEnd, Software software)
    {
        //under the assumption that if a discount is active in any period of the contract 'opening' for payment, the contract is eligible for a discount
        var applicableDiscounts = software.Discounts
            .Where(d => (contractEnd >= d.From && contractEnd <= d.To)
                        || (contractStart >= d.From && contractStart <= d.To)
            ).ToList();
        return applicableDiscounts.Any() ? applicableDiscounts.Max(d => d.Value) : 0;
    }

    public int GetTotalDiscountForContract(Contract contract, Client client, Software software)
    {
        bool isReturningClient = client.Contracts
            .Where(c => c.IsSigned)
            .ToList()
            .Count >= 1;
        int discount = GetHighestDiscountForSoftwareAndContract(contract.From, contract.To, software);
        if (isReturningClient){
            discount += 5;
        }
        return discount;
    }

    public decimal GetPriceForContractWithDiscountIncluded(Contract contract, Client client, Software software)
    {
        decimal priceWithoutDiscount = software.YearlyPrice + contract.YearsOfUpdateSupport * 1000m;
        int totalDiscount = GetTotalDiscountForContract(contract, client, software);
        decimal discountAmount = (priceWithoutDiscount * totalDiscount) / 100m;
        return priceWithoutDiscount - discountAmount;
    }
    
}