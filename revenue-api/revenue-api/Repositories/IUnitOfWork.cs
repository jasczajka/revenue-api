using Microsoft.Data.SqlClient;
using revenue_api.Context;

namespace revenue_api.Repositories;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    
    Task CommitAsync(CancellationToken cancellationToken);
    public RevenueDbContext GetDbContext();
}