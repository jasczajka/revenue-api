using Microsoft.EntityFrameworkCore;
using revenue_api.Models;

namespace revenue_api.Repositories;

public class ContractRepository : IContractRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public ContractRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Contract?> GetContractByIdAsync(int contractId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.GetDbContext().Contracts
            .Include(c => c.Payments)
            .Include(c => c.Client)
            .Include(c => c.Software)
            .FirstOrDefaultAsync(c => c.ContractId == contractId, cancellationToken);
    }

    public async Task<List<Contract>> GetContractsWithPaymentsForClientByIdAsync(int clientId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.GetDbContext().Contracts
            .Include(c => c.Payments)
            .Where(c => c.Client.ClientId == clientId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Contract>> GetContractsWithPaymentsForSoftwareByIdAsync(int softwareId,
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.GetDbContext().Contracts
            .Include(c => c.Payments)
            .Where(c => c.Software.SoftwareId == softwareId)
            .ToListAsync(cancellationToken);
    }
    public async Task<Payment> IssuePaymentForContractAsync(Contract contract, decimal amount, bool isContractPaid, CancellationToken cancellationToken)
    {
        var client = contract.Client;
        var newPayment = new Payment()
        {
            AmountPaid = amount,
            Contract = contract,
            Client = client,
        };
        contract.Payments.Add(newPayment);
        client.Payments.Add(newPayment);
        if (isContractPaid)
        {
            contract.IsSigned = true;
        }
        await _unitOfWork.CommitAsync(cancellationToken);
        
        return newPayment;
    }

    public async Task<Contract> CreateNewContractAsync(DateOnly from, DateOnly to, int yearsOfUpdateSupport, float softwareVersion, Client client,
        Software software, CancellationToken cancellationToken)
    {
        Contract newContract = new Contract(from, to, yearsOfUpdateSupport, softwareVersion, client, software);
        await _unitOfWork.GetDbContext().Contracts.AddAsync(newContract, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return newContract;
    }

    public async Task<int> CancelPaymentsForContractAsync(Contract contract, CancellationToken cancellationToken)
    {
        var paymentsRemovedCount = contract.Payments.Count;
        contract.Payments = new List<Payment>();
        await _unitOfWork.CommitAsync(cancellationToken);
        return paymentsRemovedCount;
    }
    
    
    
}