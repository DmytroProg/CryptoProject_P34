using CryptoProj.Domain.Models;

namespace CryptoProj.Domain.Abstractions;

public interface IUserRepository
{
    Task<User> Register(User user);
    Task<User?> Login(string email, string passwordHash);
    ValueTask<User?> Get(int userId);
    Task<bool> IsEmailTaken(string email);
}