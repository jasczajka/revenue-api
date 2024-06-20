using Microsoft.EntityFrameworkCore;
using revenue_api.Models;

namespace revenue_api.Context;

public class RevenueDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<IndividualClient> IndividualClients { get; set; }
    public DbSet<CorporateClient> CorporateClients { get; set; }
    public RevenueDbContext()
    {
    }

    public RevenueDbContext(DbContextOptions options) : base(options)
    {
        
    }

}