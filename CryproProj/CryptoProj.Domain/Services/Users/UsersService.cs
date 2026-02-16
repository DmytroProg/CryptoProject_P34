using CryptoProj.Domain.Abstractions;
using CryptoProj.Domain.Exceptions;
using CryptoProj.Domain.Models;

namespace CryptoProj.Domain.Services.Users;

public class UsersService
{
    private readonly IUserRepository _userRepository;

    public UsersService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse> Register(RegisterUserRequest request)
    {
        if (await _userRepository.IsEmailTaken(request.Email))
        {
            throw new EmailAlreadyTakenException(request.Email);
        }
        
        var user = new User
        {
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Username = request.Username,
            CreatedAt = DateTime.UtcNow
        };
        
        await _userRepository.Register(user);
        
        return MapToResponse(user);
    }

    private UserResponse MapToResponse(User user) => new()
    {
        Id = user.Id,
        Email = user.Email,
        Username = user.Username,
        Balance = user.Balance
    };
}