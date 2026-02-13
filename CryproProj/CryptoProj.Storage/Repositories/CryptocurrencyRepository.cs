using CryptoProj.Domain.Abstractions;
using CryptoProj.Domain.Models;

namespace CryptoProj.Storage.Repositories;

public class CryptocurrencyRepository : BaseRepository<Cryptocurrency>, ICryptocurrencyRepository
{
    public CryptocurrencyRepository(CryptoContext context) : base(context)
    {
    }

    public async Task<Cryptocurrency> Update(Cryptocurrency cryptocurrency)
    {
        Context.Cryptocurrencies.Update(cryptocurrency);
        await Context.SaveChangesAsync();
        return cryptocurrency;
    }

    public async Task Delete(int cryptocurrencyId)
    {
        var crypto = await Context.Cryptocurrencies.FindAsync(cryptocurrencyId);
        
        if(crypto == null)
            return;
        
        crypto.DeletedAt = DateTime.UtcNow;
        await Context.SaveChangesAsync();
    }
}