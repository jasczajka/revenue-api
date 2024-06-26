using revenue_api.Models;

namespace revenue_api.Repositories;

public interface IContractRepository
{
    public Task<Contract?> GetContractByIdAsync(int contractId, CancellationToken cancellationToken);
    public Task<List<Contract>> GetContractsWithPaymentsForClientByIdAsync(int clientId, CancellationToken cancellationToken);
    public Task<List<Contract>> GetContractsWithPaymentsForSoftwareByIdAsync(int softwareId, CancellationToken cancellationToken);
    public Task<Payment> IssuePaymentForContractAsync(Contract contract, decimal amount, bool isContractPaid, CancellationToken cancellationToken);

    public Task<Contract> CreateNewContractAsync(DateOnly from, DateOnly to, int yearsOfUpdateSupport, float softwareVersion,
        Client client, Software software, CancellationToken cancellationToken);

    public Task<int> CancelPaymentsForContractAsync(Contract contract, CancellationToken cancellationToken);
    
}