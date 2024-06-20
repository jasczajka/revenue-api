using Microsoft.EntityFrameworkCore;
using revenue_api.Context;

namespace revenue_api.Models;

public static class SeedData
{
    public static void InitializeClients(IServiceProvider serviceProvider)
    {
        using (var context = new RevenueDbContext(
                   serviceProvider.GetRequiredService<DbContextOptions<RevenueDbContext>>()
               ))
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
                    CompanyName =  "Spółka B",
                    EmailAddress = "contact@spolka-b.com",
                    PhoneNumber = "234-567-890",
                    Address = "Street B, City B, 00-002"
                },
                new CorporateClient("34567890123456" )
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
      
    }
}