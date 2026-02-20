using System.Globalization;
using CryptoProj.Domain.Abstractions;
using CryptoProj.Domain.Models;
using CryptoProj.Domain.Models.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoProj.WebDataProvider.DataProviders;

public class CryptocurrencyDataProvider : ICryptocurrencyRepository
{
    private readonly HttpClient _httpClient;

    public CryptocurrencyDataProvider()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://api.coinmarketcap.com/data-api/v3/cryptocurrency/");
        _httpClient.DefaultRequestHeaders.Add("x-request-id", "4208475f72d74eb98b95552f5828a27f");
        _httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36");
        _httpClient.DefaultRequestHeaders.Add("origin", "https://coinmarketcap.com");
        _httpClient.DefaultRequestHeaders.Add("accept", "application/json");
    }

    public async Task<Cryptocurrency[]> GetAll(CryptocurrencyRequest request)
    {
        var response = await _httpClient.GetAsync($"listing?start={request.Offset}&limit={request.Limit}&sortBy=rank&sortType=desc&convert=USD");

        await Task.Delay(3000);
        
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(json);
            var cryptos = data["data"]["cryptoCurrencyList"];

            return cryptos.Select(x => new Cryptocurrency
            {
                Id = int.Parse(x["id"].ToString()),
                Symbol = x["symbol"].ToString(),
                Name = x["name"].ToString(),
                Price = ParsePrice(x["quotes"][0]["price"].ToString())
            })
            .ToArray();
        }

        return [];
    }

    private decimal ParsePrice(string price)
    {
        return decimal.Parse(price, NumberStyles.Float, CultureInfo.InvariantCulture);
    }

    public ValueTask<Cryptocurrency?> Get(int cryptocurrencyId)
    {
        throw new NotImplementedException();
    }

    public Task<Cryptocurrency> Add(Cryptocurrency cryptocurrency)
    {
        throw new NotImplementedException();
    }

    public Task<Cryptocurrency> Update(Cryptocurrency cryptocurrency)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int cryptocurrencyId)
    {
        throw new NotImplementedException();
    }
}