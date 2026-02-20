using CryptoProj.Domain.Abstractions;
using CryptoProj.Domain.Models;
using CryptoProj.Domain.Models.Requests;
using Microsoft.Extensions.Logging;

namespace CryptoProj.Domain.Services.Cryptocurrencies;

public class CryptocurrenciesService
{
    private readonly ICryptocurrencyRepository _cryptocurrencyRepository;
    private readonly ICryptoHistoryRepository _cryptoHistoryRepository;
    private readonly ILogger<CryptocurrenciesService> _logger;

    public CryptocurrenciesService(ICryptocurrencyRepository cryptocurrencyRepository, ICryptoHistoryRepository cryptoHistoryRepository, ILogger<CryptocurrenciesService> logger)
    {
        _cryptocurrencyRepository = cryptocurrencyRepository;
        _cryptoHistoryRepository = cryptoHistoryRepository;
        _logger = logger;
    }
    
    public Task<Cryptocurrency[]> GetCryptocurrencies(CryptocurrencyRequest request)
    {
        return _cryptocurrencyRepository.GetAll(request);
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
        
        _logger.LogInformation($"Cryptocurrency {crypto.Symbol} added successfully.");
        
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
        
        _logger.LogInformation($"Cryptocurrency {crypto.Symbol} updated successfully.");
        
        return MapToResponse(crypto);
    }
    
    public Task Delete(int id)
    {
        _logger.LogInformation($"Cryptocurrency with id {id} deleted successfully.");
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
        
        _logger.LogInformation($"History item for cryptocurrency {cryptocurrencyId} added successfully.");
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