using Microsoft.EntityFrameworkCore;
using WailletAPI.Data;

namespace WailletAPI.Repository.Implementations;

public class LedgerRepository : ILedgerRepository
{
    private readonly WailletDbContext _context;

    public LedgerRepository(WailletDbContext context)
    {
        _context = context;
    }

    public async Task<decimal> GetAccountBalanceAsync(long accKey)
    {
        return await _context.Ledgers
            .Where(l => l.AccKey == accKey)
            .SumAsync(l => (decimal?)l.Amount) ?? 0m;
    }
}