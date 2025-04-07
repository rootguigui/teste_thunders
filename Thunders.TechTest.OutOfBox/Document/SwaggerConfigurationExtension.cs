using Microsoft.Extensions.DependencyInjection;

namespace Thunders.TechTest.OutOfBox.Document;

public static class SwaggerConfigurationExtension
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Thunders Tech Test API",
                Version = "v1",
                Description = "API para gerenciamento de pedágios"
            });
        });

        return services;
    }
}
