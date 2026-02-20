namespace CryptoProj.Domain.Models.Requests;

public class CryptocurrencyRequest
{
    public int Limit { get; set; }
    public int Offset { get; set; }
    public string? Symbol { get; set; }
}