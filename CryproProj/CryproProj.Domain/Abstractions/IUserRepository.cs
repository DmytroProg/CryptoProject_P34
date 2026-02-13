using CryproProj.Domain.Models;

namespace CryproProj.Domain.Abstractions;

public interface IUserRepository
{
    Task<User?> Register(User user);
    Task<User?> Login(string email, string password);
    ValueTask<User?> Get(int userId);
    Task<bool> IsEmailTaken(string email);
}