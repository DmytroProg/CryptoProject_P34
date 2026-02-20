using CryptoProj.Domain.Models;
using CryptoProj.Domain.Models.Requests;

namespace CryptoProj.Domain.Abstractions;

public interface ICryptocurrencyRepository
{
    Task<Cryptocurrency[]> GetAll(CryptocurrencyRequest request);
    ValueTask<Cryptocurrency?> Get(int cryptocurrencyId);
    Task<Cryptocurrency> Add(Cryptocurrency cryptocurrency);
    Task<Cryptocurrency> Update(Cryptocurrency cryptocurrency);
    Task Delete(int cryptocurrencyId);
}