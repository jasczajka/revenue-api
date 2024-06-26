using Microsoft.EntityFrameworkCore;
using revenue_api.Helpers;
using revenue_api.Models;
using revenue_api.Models.Auth;

namespace revenue_api.Context;

public class RevenueDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<IndividualClient> IndividualClients { get; set; }
    public DbSet<CorporateClient> CorporateClients { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Software> Softwares { get; set; }
    public DbSet<AppUser> Users { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<SubscriptionOffer> SubscriptionOffers { get; set; }
    public RevenueDbContext()
    {
    }

    public RevenueDbContext(DbContextOptions options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var eTypes = modelBuilder.Model.GetEntityTypes();
        foreach(var type in eTypes)
        {
            var foreignKeys = type.GetForeignKeys();
            foreach(var foreignKey in foreignKeys)
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }

}