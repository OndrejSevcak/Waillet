using Microsoft.EntityFrameworkCore;
using WailletAPI.Data;
using WailletAPI.Entities;

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

    public async Task<IReadOnlyList<Ledger>> GetAccountTransactionsAsync(long accKey)
    {
        return await _context.Ledgers
            .Where(l => l.AccKey == accKey)
            .OrderByDescending(l => l.CreatedAt)
            .ThenByDescending(l => l.Id)
            .ToListAsync();
    }
}