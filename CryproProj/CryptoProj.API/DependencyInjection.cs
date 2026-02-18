using CryptoProj.Domain.Abstractions;
using CryptoProj.Domain.Services.Auth;
using CryptoProj.Domain.Services.Cryptocurrencies;
using CryptoProj.Domain.Services.Users;
using CryptoProj.Storage;
using CryptoProj.Storage.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CryptoProj.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddDbContext<CryptoContext>(opt => opt.UseInMemoryDatabase("local"));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICryptocurrencyRepository, CryptocurrencyRepository>();
        services.AddScoped<ICryptoHistoryRepository, CryptoHistoryRepository>();

        services.AddScoped<UsersService>();
        services.AddScoped<CryptocurrenciesService>();
        services.AddTransient<JwtTokenGenerator>();
        
        return services;
    }
}