using CryproProj.Domain.Models;

namespace CryproProj.Domain.Abstractions;

public interface ICryptocurrencyRepository
{
    Task<Cryptocurrency?> Get(int cryptocurrencyId);
    Task<Cryptocurrency> Add(Cryptocurrency cryptocurrency);
    Task<Cryptocurrency> Update(Cryptocurrency cryptocurrency);
    Task Delete(int cryptocurrencyId);
}