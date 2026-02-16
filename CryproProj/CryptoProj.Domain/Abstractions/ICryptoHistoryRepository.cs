using CryptoProj.Domain.Models;
using CryptoProj.Domain.Models.Requests;
using CryptoProj.Domain.Services.Cryptocurrencies;

namespace CryptoProj.Domain.Abstractions;

public interface ICryptoHistoryRepository
{
    Task<CryptoHistoryResponse[]> GetAll(HistoryRequest request);
    Task<CryptoHistoryItem> Add(CryptoHistoryItem cryptoHistoryItem);
}