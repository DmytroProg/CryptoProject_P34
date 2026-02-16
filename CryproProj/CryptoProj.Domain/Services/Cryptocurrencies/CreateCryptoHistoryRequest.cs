namespace CryptoProj.Domain.Services.Cryptocurrencies;

public class CreateCryptoHistoryRequest
{
    public decimal Buy { get; set; }
    public decimal Sell { get; set; }
    public decimal Quantity { get; set; }
}