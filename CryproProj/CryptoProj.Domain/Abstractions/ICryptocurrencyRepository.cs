using CryptoProj.Domain.Models;

namespace CryptoProj.Domain.Abstractions;

public interface ICryptocurrencyRepository
{
    ValueTask<Cryptocurrency?> Get(int cryptocurrencyId);
    Task<Cryptocurrency> Add(Cryptocurrency cryptocurrency);
    Task<Cryptocurrency> Update(Cryptocurrency cryptocurrency);
    Task Delete(int cryptocurrencyId);
}