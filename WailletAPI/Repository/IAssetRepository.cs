using System.Collections.Generic;
using System.Threading.Tasks;
using WailletAPI.Entities;

namespace WailletAPI.Repository;

public interface IAssetRepository
{
    /// <summary>
    /// Returns all active assets.
    /// </summary>
    Task<IEnumerable<Asset>> GetAllAsync();
}
