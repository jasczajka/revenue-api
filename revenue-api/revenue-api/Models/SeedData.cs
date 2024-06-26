using Microsoft.EntityFrameworkCore;
using revenue_api.Context;
using revenue_api.Helpers;
using revenue_api.Models.Auth;

namespace revenue_api.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new RevenueDbContext(
                   serviceProvider.GetRequiredService<DbContextOptions<RevenueDbContext>>()
               ))
        {
            InitializeClients(context);
            InitializeSoftware(context);
            InitializeDiscounts(context);
            InitializeContracts(context);
            InitializePayments(context);
            InitializeSubscriptionOffers(context);
            InitializeSubscriptions(context);
            InitializeAdmin(context);
        }
    }

    private static void InitializeClients(RevenueDbContext context)
    {
        if (context.Clients.Any())
        {
            return; // Clients already Seeded
        }

        context.Clients.AddRange(
            new CorporateClient("12345678901234")
            {
                CompanyName = "Spółka A",
                EmailAddress = "contact@spolka-a.com",
                PhoneNumber = "123-456-789",
                Address = "Street A, City A, 00-001"
            },
            new CorporateClient("23456789012345")
            {
                CompanyName = "Spółka B",
                EmailAddress = "contact@spolka-b.com",
                PhoneNumber = "234-567-890",
                Address = "Street B, City B, 00-002"
            },
            new CorporateClient("34567890123456")
            {
                CompanyName = "Spółka C",
                EmailAddress = "contact@spolka-c.com",
                PhoneNumber = "345-678-901",
                Address = "Street C, City C, 00-003"
            },
            new IndividualClient("12345678901")
            {
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "john.doe@example.com",
                PhoneNumber = "456-789-012",
                Address = "Street D, City D, 00-004",
            },
            new IndividualClient("23456789012")
            {
                FirstName = "Jane",
                LastName = "Smith",
                EmailAddress = "jane.smith@example.com",
                PhoneNumber = "567-890-123",
                Address = "Street E, City E, 00-005",
            }

        );
        context.SaveChanges();
    }

    private static void InitializeSoftware(RevenueDbContext context)
    {
        if (context.Softwares.Any())
        {
            return; // Software already seeded
        }

        context.Softwares.AddRange(
            new Software
            {
                Name = "FinancePro",
                Description = "Finance management software",
                Category = "Finance",
                CurrentVersion = 1.0f,
                YearlyPrice = 2000m
            },
            new Software
            {
                Name = "EduMaster",
                Description = "Educational software for schools",
                Category = "Education",
                CurrentVersion = 2.1f,
                YearlyPrice = 1500m
            }
        );

        context.SaveChanges();
    }

    private static void InitializeDiscounts(RevenueDbContext context)
    {
        if (context.Discounts.Any())
        {
            return; // Discounts already seeded
        }

        context.Discounts.AddRange(
            new Discount
            {
                Name = "Black Friday Discount",
                DiscountType = "SUB",
                Value = 10,
                From = new DateOnly(2024, 1, 1),
                To = new DateOnly(2024, 3, 3),
                Softwares = context.Softwares.ToList(),
            },
            new Discount
            {
                Name = "Holiday Discount",
                DiscountType = "PUR",
                Value = 15,
                From = new DateOnly(2024, 12, 1),
                To = new DateOnly(2024, 12, 31)
            }
        );
        context.SaveChanges();
        context.Discounts.First(d => d.Name == "Holiday Discount")
            .Softwares.Add(context.Softwares.First());
        context.SaveChanges();
    }

    private static void InitializeContracts(RevenueDbContext context)
    {
        if (context.Contracts.Any())
        {
            return; // Contracts already seeded
        }

        var client = context.Clients.First();
        var software = context.Softwares.First(s => s.Name == "FinancePro");

        context.Contracts.AddRange(
            new Contract(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(10)), 1,
                1.0f, client, software),
            new Contract(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(15)), 2,
                2.1f, client, software)
        );

        context.SaveChanges();
    }

    private static void InitializePayments(RevenueDbContext context)
    {
        if (context.Payments.Any())
        {
            return; // Payments already seeded
        }

        var contract = context.Contracts.First();
        var client = context.Clients.First();

        context.Payments.AddRange(
            new Payment
            {
                AmountPaid = 2000m,
                Contract = contract,
                Client = client
            },
            new Payment
            {
                AmountPaid = 1000,
                Contract = contract,
                Client = client
            }
        );

        context.SaveChanges();
    }

    private static void InitializeAdmin(RevenueDbContext context)
    {
        if (context.Users.Where(u => u.Role == Role.Admin).Any())
        {
            return; // Admin already seeded
        }

        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt("admin");

        context.Users.Add(new AppUser()
        {
            Email = "admin@admin",
            Login = "admin",
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelpers.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.MaxValue,
            Role = Role.Admin
        });


        context.SaveChanges();
    }

    private static void InitializeSubscriptionOffers(RevenueDbContext context)
    {
        if (context.SubscriptionOffers.Any())
        {
            return; // SubscriptionOffers already seeded
        }

        var software = context.Softwares.First();

        context.SubscriptionOffers.AddRange(
            new SubscriptionOffer
            {
                Name = "Monthly FinancePro",
                Price = 200m,
                RenewalPeriod = 30,
                SoftwareVersion = 1.0f,
                Software = software
            },
            new SubscriptionOffer
            {
                Name = "Yearly FinancePro",
                Price = 2000m,
                RenewalPeriod = 365,
                SoftwareVersion = 1.0f,
                Software = software
            }
        );
        context.SaveChanges();

    }
    private static void InitializeSubscriptions(RevenueDbContext context)
    {
        if (context.Subscriptions.Any())
        {
            return; // Subscriptions already seeded
        }

        var client = context.Clients.First();
        var subscriptionOffer = context.SubscriptionOffers.First();

        context.Subscriptions.AddRange(
            new Subscription
            {
                ActiveUntil = DateOnly.FromDateTime(DateTime.Today.AddDays(30)),
                IsCurrentPeriodPaid = true,
                SubscriptionOffer = subscriptionOffer,
                Client = client
            },
            new Subscription
            {
                ActiveUntil = DateOnly.FromDateTime(DateTime.Today.AddDays(365)),
                IsCurrentPeriodPaid = true,
                SubscriptionOffer = subscriptionOffer,
                Client = client
            }
        );

        context.SaveChanges();
    }
}