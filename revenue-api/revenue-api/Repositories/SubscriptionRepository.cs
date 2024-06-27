using Microsoft.EntityFrameworkCore;
using revenue_api.Models;

namespace revenue_api.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public SubscriptionRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Subscription?> GetSubscriptionByIdAsync(int subscriptionId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.GetDbContext().Subscriptions
            .Include(s => s.SubscriptionOffer)
            .Include(s => s.Payments)
            .Include(s => s.Client)
            .FirstOrDefaultAsync(s => s.SubscriptionId == subscriptionId, cancellationToken);

    }

    public async Task<SubscriptionOffer?> GetSubscriptionOfferByIdAsync(int subscriptionOfferId,
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.GetDbContext().SubscriptionOffers
            .Include(s => s.Software)
            .FirstOrDefaultAsync(s => s.SubscriptionOfferId == subscriptionOfferId, cancellationToken);
    }
    
    public async Task<List<Subscription>> GetSubscriptionsWithPaymentsForClientByIdAsync(int clientId,
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.GetDbContext().Subscriptions
            .Include(s => s.Payments)
            .Include(s => s.SubscriptionOffer)
            .Where(s => s.Client.ClientId == clientId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Subscription>> GetSubscriptionsWithPaymentsForSoftwareByIdAsync(int softwareId,
        CancellationToken cancellationToken)
    {
        return await _unitOfWork.GetDbContext().Subscriptions
            .Include(s => s.Payments)
            .Include(s => s.SubscriptionOffer)
            .Where(s => s.SubscriptionOffer.Software.SoftwareId == softwareId)
            .ToListAsync(cancellationToken);
    }
    public async Task<Payment> IssuePaymentForSubscriptionAsync(Subscription subscription, decimal amount, DateOnly dateOfPayment,
        CancellationToken cancellationToken)
    {
        var newPayment = new Payment()
        {
            AmountPaid = amount,
            Subscription = subscription,
            Client = subscription.Client
        };
        subscription.IsCurrentPeriodPaid = true;
        subscription.Payments.Add(newPayment);
        await _unitOfWork.CommitAsync(cancellationToken);
        return newPayment;
    }

    public async Task<Subscription> CreateNewSubscriptionAsync(DateOnly from, SubscriptionOffer subscriptionOffer, float softwareVersion,
        Client client, CancellationToken cancellationToken)
    {
        Subscription newSubscription = new Subscription()
        {
            Client = client,
            IsCurrentPeriodPaid = false,
            ActiveUntil = from.AddDays(subscriptionOffer.RenewalPeriod),
            SubscriptionOffer = subscriptionOffer,

        };
        await _unitOfWork.GetDbContext().Subscriptions.AddAsync(newSubscription, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return newSubscription;
    }

    public async Task<List<Subscription>> GetYesterdayExpiredPaidForSubscriptionsAsync(CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);
        var paidSubs = await _unitOfWork.GetDbContext().Subscriptions
            .Where(s => s.IsCurrentPeriodPaid)
            .ToListAsync(cancellationToken);
        return paidSubs.Where(s => today == s.ActiveUntil.AddDays(1)).ToList();
    }
    public async Task MarkSubscriptionsAsUnpaid(List<Subscription> subscriptions, CancellationToken cancellationToken)
    {
        subscriptions.ForEach(s =>
        {
            s.IsCurrentPeriodPaid = false;
            s.ActiveUntil = s.ActiveUntil.AddDays(s.SubscriptionOffer.RenewalPeriod);
        });
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    
}