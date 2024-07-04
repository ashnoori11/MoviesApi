using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Infrastructure;
using Application.Services.CacheService.CacheDecorators;
using Application.Services.CacheService;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, bool hasDisterbutedCache)
    {
        services.AddInfrastructureServices();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMemoryCache();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
            options.InstanceName = "MyApp";
        });
        services.AddScoped<ICacheManager, CompositeCacheManagerDecorator>();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(CacheBehavior<,>));
        });
        return services;
    }
}
