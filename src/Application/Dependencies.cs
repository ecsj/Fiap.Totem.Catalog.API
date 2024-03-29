using Application.Interfaces;
using Application.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Application;
[ExcludeFromCodeCoverage]

public class Dependencies
{
    public static IServiceCollection ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        services.AddScoped<IProductUseCase, ProductUseCase>();

        return services;
    }
}

