using CryptoProj.Domain.Abstractions;
using CryptoProj.Domain.Models;
using CryptoProj.Domain.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace CryptoProj.Storage.Repositories;

public class CryptocurrencyRepository : BaseRepository<Cryptocurrency>, ICryptocurrencyRepository
{
    public CryptocurrencyRepository(CryptoContext context) : base(context)
    {
    }

    public Task<Cryptocurrency[]> GetAll(CryptocurrencyRequest request)
    {
        var query = Context.Set<Cryptocurrency>()
            .AsNoTracking();

        if (!string.IsNullOrEmpty(request.Symbol))
        {
            query = query.Where(c => c.Symbol.Contains(request.Symbol));
        }
        
        return query
            .Skip(request.Offset)
            .Take(request.Limit)
            .ToArrayAsync();
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