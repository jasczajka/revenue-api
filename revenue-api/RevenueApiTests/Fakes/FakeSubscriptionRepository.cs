using revenue_api.Models;
using revenue_api.Repositories;

namespace RevenueApiTests.Fakes;

public class FakeSubscriptionRepository : ISubscriptionRepository
{
    private readonly List<SubscriptionOffer> _subscriptionOffers;
    private readonly List<Subscription> _subscriptions;
    private readonly List<Software> _softwares = FakeSoftwareRepository.GetTestSoftwares();
    
    
    private int _nextSubscriptionId;
    private int _nextPaymentId;

    public static List<Subscription> GetTestSubscriptions()
    {
        return new FakeSubscriptionRepository()._subscriptions;
    }
    public FakeSubscriptionRepository( )
    {
        
        _nextSubscriptionId = 1;
            _nextPaymentId = 1;
            _subscriptionOffers = new List<SubscriptionOffer>
            {
                new SubscriptionOffer
                {
                    SubscriptionOfferId = 1,
                    Name = "Basic Plan",
                    Price = 99.99m,
                    RenewalPeriod = 30,
                    SoftwareVersion = 1.0f,
                    Software = _softwares.Find(s => s.SoftwareId == 4)
                },
                new SubscriptionOffer
                {
                    SubscriptionOfferId = 2,
                    Name = "Premium Plan",
                    Price = 199.99m,
                    RenewalPeriod = 365,
                    SoftwareVersion = 1.0f,
                    Software = _softwares.Find(s => s.SoftwareId == 4)
                },
                new SubscriptionOffer
                {
                    SubscriptionOfferId = 3,
                    Name = "Yeary Software CPlan",
                    Price = 300m,
                    RenewalPeriod = 365,
                    SoftwareVersion = 1.0f,
                    Software = _softwares.Find(s => s.SoftwareId == 5)
                }
            };

            _subscriptions = new List<Subscription>
            {
                new Subscription
                {
                    SubscriptionId = 44,
                    ActiveUntil = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                    IsCurrentPeriodPaid = true,
                    SubscriptionOffer = _subscriptionOffers[2],
                    Client = new Client()
                    {
                        ClientId = 11
                    },
                    Payments = new List<Payment>
                    {
                        new Payment { PaymentId = _nextPaymentId++, AmountPaid = 300m }
                    }
                },
                new Subscription
                {
                    SubscriptionId = 55,
                    ActiveUntil = DateOnly.FromDateTime(DateTime.Now.AddDays(-365)),
                    IsCurrentPeriodPaid = false,
                    SubscriptionOffer = _subscriptionOffers[2],
                    Client = new Client()
                    {
                        ClientId = 555
                    },
                    Payments = new List<Payment>
                    {
                        new Payment { PaymentId = _nextPaymentId++, AmountPaid = 300m }
                    }
                },
                new Subscription
                {
                    SubscriptionId = 66,
                    ActiveUntil = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                    IsCurrentPeriodPaid = true,
                    SubscriptionOffer = _subscriptionOffers[2],
                    Client = new Client()
                    {
                        ClientId = 10
                    },
                    Payments = new List<Payment>
                    {
                        new Payment { PaymentId = _nextPaymentId++, AmountPaid = 300m }
                    }
                },
                new Subscription
                {
                    SubscriptionId = 77,
                    ActiveUntil = DateOnly.FromDateTime(DateTime.Now.AddDays(40)),
                    IsCurrentPeriodPaid = false,
                    SubscriptionOffer = _subscriptionOffers[2],
                    Client = new Client()
                    {
                        ClientId = 8
                    },
                    Payments = new List<Payment>
                    {
                        new Payment { PaymentId = _nextPaymentId++, AmountPaid = 300m }
                    }
                },
                
                
                
            };
            
    }
    
    public Task<Subscription?> GetSubscriptionByIdAsync(int subscriptionId, CancellationToken cancellationToken)
    {
        var subscription = _subscriptions
            .FirstOrDefault(s => s.SubscriptionId == subscriptionId);
        return Task.FromResult(subscription);
    }
    public Task<SubscriptionOffer?> GetSubscriptionOfferByIdAsync(int subscriptionOfferId, CancellationToken cancellationToken)
    {
        var subscriptionOffer = _subscriptionOffers
            .FirstOrDefault(s => s.SubscriptionOfferId == subscriptionOfferId);
        return Task.FromResult(subscriptionOffer);
    }
    public Task<List<Subscription>> GetSubscriptionsWithPaymentsForClientByIdAsync(int clientId, CancellationToken cancellationToken)
    {
        var subscriptions = _subscriptions
            .Where(s => s.Client.ClientId == clientId )
            .ToList();
        return Task.FromResult(subscriptions);
    }
    public Task<List<Subscription>> GetSubscriptionsWithPaymentsForSoftwareByIdAsync(int softwareId, CancellationToken cancellationToken)
    {
        var subscriptions = _subscriptions
            .Where(s => s.SubscriptionOffer.Software.SoftwareId == softwareId)
            .ToList();
        return Task.FromResult(subscriptions);
    }
    public Task<Payment> IssuePaymentForSubscriptionAsync(Subscription subscription, decimal amount, DateOnly dateOfPayment, CancellationToken cancellationToken)
    {
        var newPayment = new Payment
        {
            PaymentId = _nextPaymentId++,
            AmountPaid = amount,
            Subscription = subscription,
            Client = subscription.Client
        };

        subscription.IsCurrentPeriodPaid = true;
        subscription.Payments.Add(newPayment);
        subscription.Client.Payments.Add(newPayment);
        
        return Task.FromResult(newPayment);
    }
    
    public Task<Subscription> CreateNewSubscriptionAsync(DateOnly from, SubscriptionOffer subscriptionOffer, float softwareVersion, Client client, CancellationToken cancellationToken)
    {
        var newSubscription = new Subscription
        {
            SubscriptionId = _nextSubscriptionId++,
            Client = client,
            IsCurrentPeriodPaid = false,
            ActiveUntil = from.AddDays(subscriptionOffer.RenewalPeriod),
            SubscriptionOffer = subscriptionOffer
        };

        _subscriptions.Add(newSubscription);
        return Task.FromResult(newSubscription);
    }

    public Task MarkSubscriptionsAsUnpaid(List<Subscription> subscriptions, CancellationToken cancellationToken)
    {
        subscriptions.ForEach(s =>
        {
            s.IsCurrentPeriodPaid = false;
            s.ActiveUntil = s.ActiveUntil.AddDays(s.SubscriptionOffer.RenewalPeriod);
        });
        return Task.CompletedTask;
    }
    public Task<List<Subscription>> GetYesterdayExpiredPaidForSubscriptionsAsync(CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var paidSubs =  _subscriptions
            .Where(s => s.IsCurrentPeriodPaid)
            .ToList();
        return Task.FromResult(paidSubs.Where(s => today == s.ActiveUntil.AddDays(1)).ToList());
    }
    
    
}