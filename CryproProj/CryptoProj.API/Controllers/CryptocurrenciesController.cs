using CryptoProj.Domain.Models.Requests;
using CryptoProj.Domain.Services.Cryptocurrencies;
using Microsoft.AspNetCore.Mvc;

namespace CryptoProj.API.Controllers;

[ApiController]
[Route("api/v1/cryptos")]
public class CryptocurrenciesController : ControllerBase
{
    private readonly CryptocurrenciesService _cryptocurrenciesService;

    public CryptocurrenciesController(CryptocurrenciesService cryptocurrenciesService)
    {
        _cryptocurrenciesService = cryptocurrenciesService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCrypto([FromRoute] int id)
    {
        var crypto = await _cryptocurrenciesService.GetById(id);
        return Ok(crypto);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddCrypto([FromBody] CreateCryptocurrencyRequest request)
    {
        var crypto = await _cryptocurrenciesService.Add(request);
        return CreatedAtAction(nameof(GetCrypto), new { id = crypto.Id }, crypto);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCrypto([FromRoute] int id, [FromBody] CreateCryptocurrencyRequest request)
    {
        var crypto = await _cryptocurrenciesService.Update(id, request);
        return Ok(crypto);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCrypto([FromRoute] int id)
    {
        await _cryptocurrenciesService.Delete(id);
        return NoContent();
    }

    [HttpGet("{id}/history")]
    public async Task<IActionResult> GetHistory([FromRoute] int id, [FromQuery] int limit = 10, [FromQuery] int offset = 0)
    {
        var request = new HistoryRequest
        {
            CryptocurrencyId = id,
            Limit = limit,
            Offset = offset
        };
        
        var history = await _cryptocurrenciesService.GetHistories(request);

        return Ok(history);
    }
    
    [HttpPost("{id}/history")]
    public async Task<IActionResult> AddHistory([FromRoute] int id, [FromBody] CreateCryptoHistoryRequest request)
    {
        await _cryptocurrenciesService.AddHistoryItem(id, request);
        return Created();
    }
}