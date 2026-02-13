namespace CryptoProj.Domain.Models.Requests;

public class HistoryRequest
{
    public int CryptocurrencyId { get; set; }
    public int Limit { get; set; } = 10;
    public int Offset { get; set; } = 0;
}