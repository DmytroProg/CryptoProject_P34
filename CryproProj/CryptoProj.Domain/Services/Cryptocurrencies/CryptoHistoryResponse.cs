namespace CryptoProj.Domain.Services.Cryptocurrencies;

public class CryptoHistoryResponse
{
    public DateTime DateTime { get; set; }

    public decimal Buy { get; set; }
    public decimal Sell { get; set; }
    public decimal Quantity { get; set; }
}