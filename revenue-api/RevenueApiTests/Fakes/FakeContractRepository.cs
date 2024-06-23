using revenue_api.Models;
using revenue_api.Repositories;

namespace RevenueApiTests.Fakes
{
    public class FakeContractRepository : IContractRepository
    {
        private readonly List<Contract> _contracts;
        private readonly List<Payment> _payments;
        private readonly List<Software> _softwares;
        private readonly List<Client> _clients = FakeClientRepository.GetTestClients();
        
        private int _nextContractId;
        private int _nextPaymentId;

        public FakeContractRepository()
        {
            _nextContractId = 1;
            _nextPaymentId = 1;
            _softwares = FakeSoftwareRepository.GetTestSoftwares();
            _contracts = new List<Contract>
            {
                new Contract(DateOnly.FromDateTime(DateTime.Now.AddDays(-10)), DateOnly.FromDateTime(DateTime.Now.AddDays(-5)), 2, 1.0f,
                    _clients.Find(c => c.ClientId == 1),
                    _softwares.First(s => s.SoftwareId == 1))
                {
                    ContractId = 1,
                    Payments = new List<Payment>
                    {
                        new Payment { PaymentId = _nextPaymentId ++, AmountPaid = 200 },
                        new Payment { PaymentId = _nextPaymentId ++, AmountPaid = 300 }
                    }
                },
                new Contract(DateOnly.FromDateTime(DateTime.Now.AddDays(-10)), DateOnly.FromDateTime(DateTime.Now.AddDays(-5)), 2, 1.0f,
                    new CorporateClient("12345678901234")
                    {
                        ClientId = 2,
                        CompanyName = "Company B"
                    },
                    _softwares.First(s => s.SoftwareId == 2)
                    )
                {
                    ContractId = 2,
                    Payments = new List<Payment>()
                },
                new Contract(DateOnly.FromDateTime(DateTime.Now.AddDays(-10)), DateOnly.FromDateTime(DateTime.Now.AddDays(-5)), 2, 1.0f,
                    _clients.First(c => c.ClientId == 1),
                    _softwares.First(s => s.SoftwareId == 3))
                {
                    ContractId = 3,
                    Payments = new List<Payment>()
                    {
                        new Payment { PaymentId = 1, AmountPaid = 3500 },
                    }
                },
                new Contract(DateOnly.FromDateTime(DateTime.Now.AddDays(-375)), DateOnly.FromDateTime(DateTime.Now.AddDays(-365)), 2, 1.0f,
                    _clients.First(c => c.ClientId == 1),
                    _softwares.First(s => s.SoftwareId == 3))
                {
                    ContractId = 4,
                },
                new Contract(DateOnly.FromDateTime(DateTime.Now.AddDays(-10)), DateOnly.FromDateTime(DateTime.Now.AddDays(3)), 2, 1.0f,
                    _clients.First(c => c.ClientId == 1),
                    _softwares.First(s => s.SoftwareId == 3))
                {
                    ContractId = 5
                    
                },
                
            };
            _nextContractId = _contracts.Max(c => c.ContractId) + 1;
            _clients.Find(c => c.ClientId == 1).Contracts.Add(_contracts.Find(c => c.ContractId == 1));
            _payments = _contracts.SelectMany(c => c.Payments).ToList();
        }

        public Task<Contract?> GetContractByIdAsync(int contractId, CancellationToken cancellationToken)
        {
            var contract = _contracts.FirstOrDefault(c => c.ContractId == contractId);
            return Task.FromResult(contract);
        }

        public Task<Payment> IssuePaymentForContractAsync(Contract contract, decimal amount, bool isContractPaid, CancellationToken cancellationToken)
        {
            var newPayment = new Payment
            {
                PaymentId = _nextPaymentId++,
                AmountPaid = amount,
                Contract = contract,
                Client = contract.Client
            };

            contract.Payments.Add(newPayment);
            contract.Client.Payments.Add(newPayment);
            _payments.Add(newPayment);

            if (isContractPaid)
            {
                contract.IsSigned = true;
            }

            return Task.FromResult(newPayment);
        }

        public Task<Contract> CreateNewContractAsync(DateOnly from, DateOnly to, int yearsOfUpdateSupport, float softwareVersion, Client client, Software software, CancellationToken cancellationToken)
        {
            var newContract = new Contract(from, to, yearsOfUpdateSupport, softwareVersion, client, software)
            {
                ContractId = _nextContractId++,
                Payments = new List<Payment>()
            };

            _contracts.Add(newContract);
            return Task.FromResult(newContract);
        }

        public Task<int> CancelPaymentsForContractAsync(Contract contract, CancellationToken cancellationToken)
        {
            var paymentsRemovedCount = contract.Payments.Count;
            foreach (var payment in contract.Payments)
            {
                _payments.Remove(payment);
            }

            contract.Payments.Clear();
            return Task.FromResult(paymentsRemovedCount);
        }
    }
}
