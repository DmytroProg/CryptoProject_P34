using CryptoProj.Domain.Abstractions;
using CryptoProj.Domain.Exceptions;
using CryptoProj.Domain.Models;
using Microsoft.Extensions.Logging;

namespace CryptoProj.Domain.Services.Users;

public class UsersService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UsersService> _logger;

    public UsersService(IUserRepository userRepository, ILogger<UsersService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
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

        user = await _userRepository.Register(user);
        _logger.LogInformation($"User {user.Username} registered successfully.");

        return MapToResponse(user);
    }

    public async Task<UserResponse> Login(LoginUserRequest request)
    {
        var user = await _userRepository.GetUserByEmail(request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }
        
        _logger.LogInformation($"User {user.Username} logged in successfully.");

        return MapToResponse(user);
    }

    public async Task<UserResponse?> GetById(int userId)
    {
        var user = await _userRepository.Get(userId);
        
        return user == null
            ? null
            : MapToResponse(user);
    }

    private UserResponse MapToResponse(User user) =>
        new()
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            Balance = user.Balance
        };
}