using Core;
using Core.UseCases;
using Domains;
using Domains.Interfaces;
using Infrastructure.DataProviders;

namespace BookService;

public static class DependencyInjection
{
    public static void ConfigureServices(IServiceCollection services)
    { 
        RegisterBookDependencies(services);
    }

    private static void RegisterBookDependencies(IServiceCollection services)
    {
        //register repositories
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddSingleton<IDatabaseConfig, DatabaseConfig>();
        
        //register use cases
        services.AddScoped<GetAllBooksUseCase>();

        // Register facade
        services.AddScoped<IBookFacade, BookFacade>();
    }
}