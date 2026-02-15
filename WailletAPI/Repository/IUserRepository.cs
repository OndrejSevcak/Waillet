using WailletAPI.Entities;

namespace WailletAPI.Repository;

public interface IUserRepository
{
    Task<User> AddUserAsync(User user);
    Task<User?> GetByUserNameAsync(string userName);
    Task<User?> GetByIdAsync(long userKey);
}