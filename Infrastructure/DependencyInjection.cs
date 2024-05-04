using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Infrastructure.Data;
using Infrastructure.UnitOfWorks;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<MoviesContext>();
        services.AddScoped<IUnitOfWork, UnitOfwork>();

        return services;
    }
}
