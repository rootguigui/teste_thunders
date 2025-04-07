using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Thunders.TechTest.OutOfBox.Cache;

public static class CacheExtensions
{
    public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });

        return services;
    }
}
