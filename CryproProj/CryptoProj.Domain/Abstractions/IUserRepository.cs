using CryptoProj.Domain.Models;

namespace CryptoProj.Domain.Abstractions;

public interface IUserRepository
{
    Task<User> Register(User user);
    Task<User?> GetUserByEmail(string email);
    ValueTask<User?> Get(int userId);
    Task<bool> IsEmailTaken(string email);
}