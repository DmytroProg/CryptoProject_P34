using CryptoProj.Domain.Models;
using CryptoProj.Domain.Models.Requests;

namespace CryptoProj.Domain.Abstractions;

public interface ICryptoHistoryRepository
{
    Task<CryptoHistoryItem[]> GetAll(HistoryRequest request);
    Task<CryptoHistoryItem> Add(CryptoHistoryItem cryptoHistoryItem);
}