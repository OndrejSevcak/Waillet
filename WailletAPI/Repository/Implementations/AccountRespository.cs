using Microsoft.EntityFrameworkCore;
using WailletAPI.Data;
using WailletAPI.Entities;

namespace WailletAPI.Repository.Implementations;

public class AccountRepository : IAccountRepository
{
    private readonly WailletDbContext _context;

    public AccountRepository(WailletDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Account>> GetAccountsByUserAsync(long userKey)
    {
        return await _context.Accounts.Where(a => a.UserKey == userKey).ToListAsync();
    }

    public async Task<Account?> GetByAccKeyAsync(long accKey)
    {
        return await _context.Accounts.FirstOrDefaultAsync(a => a.AccKey == accKey);
    }
    
    public async Task<Account> CreateAccountAsync(Account account)
    {
        var created = _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
        return created.Entity;
    }
}
