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

    public async Task<(IReadOnlyList<Ledger> Transactions, int TotalCount)> GetAccountTransactionsPageAsync(long accKey, int page, int pageSize)
    {
        var query = _context.Ledgers.Where(l => l.AccKey == accKey);

        var totalCount = await query.CountAsync();

        var transactions = await query
            .OrderByDescending(l => l.CreatedAt)
            .ThenByDescending(l => l.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (transactions, totalCount);
    }
}