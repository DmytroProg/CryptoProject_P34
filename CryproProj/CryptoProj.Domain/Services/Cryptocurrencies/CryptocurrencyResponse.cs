namespace CryptoProj.Domain.Services.Cryptocurrencies;

public class CryptocurrencyResponse
{
    public int Id { get; set; }
    public required string Symbol { get; set; }
    public required string Name { get; set; }
    public required decimal Price { get; set; }
}