using CryproProj.Domain.Models;

namespace CryproProj.Domain.Abstractions;

public interface ICryptoHistoryRepository
{
    Task<CryptoHistoryItem[]> Get(int cryptocurrencyId);
    Task<CryptoHistoryItem> Add(CryptoHistoryItem cryptoHistoryItem);
}