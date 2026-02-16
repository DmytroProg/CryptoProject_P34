using CryptoProj.Domain.Abstractions;
using CryptoProj.Domain.Models;
using CryptoProj.Domain.Models.Requests;
using CryptoProj.Domain.Services.Cryptocurrencies;
using Microsoft.EntityFrameworkCore;

namespace CryptoProj.Storage.Repositories;

public class CryptoHistoryRepository : BaseRepository<CryptoHistoryItem>, ICryptoHistoryRepository
{
    public CryptoHistoryRepository(CryptoContext context) : base(context)
    {
    }

    public Task<CryptoHistoryResponse[]> GetAll(HistoryRequest request)
    {
        return Context.CryptoHistoryItems
            .AsNoTracking()
            .Where(h => h.CryptocurrencyId == request.CryptocurrencyId)
            .OrderByDescending(h => h.DateTime)
            .Skip(request.Offset)
            .Take(request.Limit)
            .Select(h => new CryptoHistoryResponse
            {
                Buy = h.Buy,
                Sell = h.Sell,
                Quantity = h.Quantity,
                DateTime = h.DateTime
            })
            .ToArrayAsync();
    }
}