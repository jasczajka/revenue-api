using revenue_api.Context;

namespace revenue_api.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly RevenueDbContext _context;

    public UnitOfWork(RevenueDbContext context)
    {
        _context = context;
    }

    public RevenueDbContext GetDbContext()
    {
        return _context;
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}