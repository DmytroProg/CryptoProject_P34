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

        user = await _userRepository.Register(user);

        return MapToResponse(user);
    }

    public async Task<UserResponse> Login(LoginUserRequest request)
    {
        var user = await _userRepository.GetUserByEmail(request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

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