using CryptoProj.Domain.Abstractions;
using CryptoProj.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoProj.Storage.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(CryptoContext context) : base(context)
    {
    }

    public async Task<User> Register(User user)
    {
        Context.Users.Add(user);
        await Context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> Login(string email, string passwordHash)
    {
        var user = await Context.Users.FirstOrDefaultAsync(u => u.Email == email);

        return user != null && passwordHash == user.PasswordHash
            ? user 
            : null;
    }

    public Task<bool> IsEmailTaken(string email)
    {
        return Context.Users.AnyAsync(u => u.Email == email);
    }
}