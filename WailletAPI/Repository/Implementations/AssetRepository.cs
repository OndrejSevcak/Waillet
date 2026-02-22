using Microsoft.EntityFrameworkCore;
using WailletAPI.Data;
using WailletAPI.Entities;

namespace WailletAPI.Repository.Implementations;

public class AssetRepository : IAssetRepository
{
    private readonly WailletDbContext _context;

    public AssetRepository(WailletDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Returns all active assets (IsActive == true).
    /// </summary>
    public async Task<IEnumerable<Asset>> GetAllAsync()
    {
        return await _context.Assets
            .Where(a => a.IsActive)
            .ToListAsync();
    }
}
