namespace CryptoProj.Domain.Exceptions;

public class EmailAlreadyTakenException : Exception
{
    public EmailAlreadyTakenException(string email) : base($"Email '{email}' already taken")
    {
    }
}