using Microsoft.EntityFrameworkCore;
using WailletAPI.Data;
using WailletAPI.Entities;

namespace WailletAPI.Repository.Implementations;

public class UserRepository : IUserRepository
{
    private readonly WailletDbContext _context;

    public UserRepository(WailletDbContext context)
    {
        _context = context;
    }

    public async Task<User> AddUserAsync(User user) {
        var created = _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return created.Entity;
    }

    public Task<User?> GetByUserNameAsync(string email)
    {
        return _context.Users.SingleOrDefaultAsync(u => u.Email == email);
    }

    public Task<User?> GetByIdAsync(long userKey)
    {
        return _context.Users.FindAsync(userKey).AsTask();
    }
}
