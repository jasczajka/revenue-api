using Microsoft.EntityFrameworkCore;
using revenue_api.Models;

namespace revenue_api.Repositories;

public class SoftwareRepository : ISoftwareRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public SoftwareRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Software?> GetSoftwareByIdAsync(int softwareId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.GetDbContext().Softwares
            .Include(s => s.Discounts)
            .FirstOrDefaultAsync(s => s.SoftwareId == softwareId, cancellationToken);
    }
    
}
