using CryptoProj.Domain.Abstractions;
using CryptoProj.Domain.Models;
using CryptoProj.Domain.Models.Requests;

namespace CryptoProj.Domain.Services.Cryptocurrencies;

public class CryptocurrenciesService
{
    private readonly ICryptocurrencyRepository _cryptocurrencyRepository;
    private readonly ICryptoHistoryRepository _cryptoHistoryRepository;

    public CryptocurrenciesService(ICryptocurrencyRepository cryptocurrencyRepository, ICryptoHistoryRepository cryptoHistoryRepository)
    {
        _cryptocurrencyRepository = cryptocurrencyRepository;
        _cryptoHistoryRepository = cryptoHistoryRepository;
    }

    public async Task<CryptocurrencyResponse?> GetById(int id)
    {
        var crypto = await _cryptocurrencyRepository.Get(id);
        
        return crypto == null 
            ? null 
            : MapToResponse(crypto);
    }

    public async Task<CryptocurrencyResponse> Add(CreateCryptocurrencyRequest request)
    {
        var crypto = new Cryptocurrency
        {
            Name = request.Name,
            Symbol = request.Symbol,
            Price = request.Price
        };
        
        crypto = await _cryptocurrencyRepository.Add(crypto);
        
        return MapToResponse(crypto);
    }
    
    public async Task<CryptocurrencyResponse> Update(int id, CreateCryptocurrencyRequest request)
    {
        var crypto = await _cryptocurrencyRepository.Get(id);
        
        if(crypto == null)
            throw new ArgumentNullException("Cryptocurrency not found.");
        
        crypto.Name = request.Name;
        crypto.Symbol = request.Symbol;
        crypto.Price = request.Price;
        
        crypto = await _cryptocurrencyRepository.Update(crypto);
        
        return MapToResponse(crypto);
    }
    
    public Task Delete(int id)
    {
        return _cryptocurrencyRepository.Delete(id);
    }

    public async Task AddHistoryItem(int cryptocurrencyId, CreateCryptoHistoryRequest request)
    {
        var cryptoHistory = new CryptoHistoryItem
        {
            Id = Guid.NewGuid(),
            CryptocurrencyId = cryptocurrencyId,
            DateTime = DateTime.UtcNow,
            Buy = request.Buy,
            Sell = request.Sell,
            Quantity = request.Quantity
        };
        
        await _cryptoHistoryRepository.Add(cryptoHistory);
    }

    public Task<CryptoHistoryResponse[]> GetHistories(HistoryRequest request)
    {
        return _cryptoHistoryRepository.GetAll(request);
    }

    private CryptocurrencyResponse MapToResponse(Cryptocurrency cryptocurrency)
        => new()
        {
            Id = cryptocurrency.Id,
            Symbol = cryptocurrency.Symbol,
            Name = cryptocurrency.Name,
            Price = cryptocurrency.Price
        };
}