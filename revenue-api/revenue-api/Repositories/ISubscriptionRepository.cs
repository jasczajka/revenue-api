using revenue_api.Models;

namespace revenue_api.Repositories;

public interface ISubscriptionRepository
{
    public Task<List<Subscription>> GetSubscriptionsWithPaymentsForClientByIdAsync(int clientId, CancellationToken cancellationToken);
    public Task<List<Subscription>> GetSubscriptionsWithPaymentsForSoftwareByIdAsync(int softwareId, CancellationToken cancellationToken);
    public Task<Subscription?> GetSubscriptionByIdAsync(int subscriptionId, CancellationToken cancellationToken);
    public Task<SubscriptionOffer?> GetSubscriptionOfferByIdAsync(int subscriptionOfferId, CancellationToken cancellationToken);

    public Task<Payment> IssuePaymentForSubscriptionAsync(Subscription subscription, decimal amount, DateOnly dateOfPayment,
        CancellationToken cancellationToken);
    public Task<Subscription> CreateNewSubscriptionAsync(DateOnly from, SubscriptionOffer subscriptionOffer, float softwareVersion,
        Client client, CancellationToken cancellationToken);

    public Task<List<Subscription>> GetYesterdayExpiredPaidForSubscriptionsAsync(CancellationToken cancellationToken);
    public Task MarkSubscriptionsAsUnpaid(List<Subscription> subscriptions, CancellationToken cancellationToken);
}