using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;
using Infrastructure.UnitOfWorks;
using NetTopologySuite.Geometries;
using NetTopologySuite;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<MoviesContext>();
        services.AddScoped<IUnitOfWork, UnitOfwork>();

        services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid:4326));
        return services;
    }
}
