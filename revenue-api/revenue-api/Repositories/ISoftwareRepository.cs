using revenue_api.Models;

namespace revenue_api.Repositories;

public interface ISoftwareRepository
{
    public Task<Software?> GetSoftwareByIdAsync(int softwareId, CancellationToken cancellationToken);
    
}