namespace CryptoProj.Domain.Services.Users;

public class UserResponse
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public decimal Balance { get; set; }
}