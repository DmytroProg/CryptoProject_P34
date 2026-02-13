namespace CryptoProj.Domain.Models;

public class CryptoHistoryItem
{
    public Guid Id { get; set; }
    public int CryptocurrencyId { get; set; }
    public DateTime DateTime { get; set; }

    public decimal Buy { get; set; }
    public decimal Sell { get; set; }
    public decimal Quantity { get; set; }
}