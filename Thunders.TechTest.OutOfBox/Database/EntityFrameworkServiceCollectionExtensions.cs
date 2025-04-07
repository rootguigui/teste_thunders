using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Thunders.TechTest.OutOfBox.Database
{
    public static class EntityFrameworkServiceCollectionExtensions
    {
        public static IServiceCollection AddNpgsqlDbContext<TContext>(this IServiceCollection services, IConfiguration configuration)
            where TContext : DbContext
        {
            services.AddDbContext<TContext>((options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("ThundersTechTestDb"));
            });

            // Criar o banco de dados automaticamente
            using var scope = services.BuildServiceProvider().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            context.Database.EnsureCreated();

            return services;
        }
    }
}
