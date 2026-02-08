using Microsoft.EntityFrameworkCore;
using WailletAPI.Data;
using WailletAPI.Models;

namespace WailletAPI.Repository;

public class TransactionRepository
{
    private readonly WailletDbContext _context;

    public TransactionRepository(WailletDbContext context)
    {
        _context = context;
    }

    public async Task AddTransactionAsync(Transaction tx)
    {
        _context.Set<Transaction>().Add(tx);
        await _context.SaveChangesAsync();
    }

    public async Task<Transaction?> GetByIdAsync(long txKey)
    {
        return await _context.Set<Transaction>().FindAsync(txKey);
    }

    public async Task<IEnumerable<Transaction>> GetByUserAsync(long userKey)
    {
        return await _context.Set<Transaction>()
            .Where(t => t.InitiatorUserKey == userKey)
            .ToListAsync();
    }
}
