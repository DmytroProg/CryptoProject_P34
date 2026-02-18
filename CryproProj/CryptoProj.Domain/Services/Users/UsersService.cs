using CryptoProj.Domain.Abstractions;
using CryptoProj.Domain.Exceptions;
using CryptoProj.Domain.Models;
using CryptoProj.Domain.Services.Auth;
using Microsoft.Extensions.Logging;

namespace CryptoProj.Domain.Services.Users;

public class UsersService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<UsersService> _logger;

    public UsersService(IUserRepository userRepository, ILogger<UsersService> logger, JwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _logger = logger;
        _jwtTokenGenerator = jwtTokenGenerator;
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
        
        var token = _jwtTokenGenerator.GenerateToken(user);

        return MapToResponse(user, token);
    }

    public async Task<UserResponse> Login(LoginUserRequest request)
    {
        var user = await _userRepository.GetUserByEmail(request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }
        
        _logger.LogInformation($"User {user.Username} logged in successfully.");
        
        var token = _jwtTokenGenerator.GenerateToken(user);

        return MapToResponse(user, token);
    }

    public async Task<UserResponse?> GetById(int userId)
    {
        var user = await _userRepository.Get(userId);
        
        return user == null
            ? null
            : MapToResponse(user);
    }

    private UserResponse MapToResponse(User user, string? token = null) =>
        new()
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            Balance = user.Balance,
            Token = token
        };
}