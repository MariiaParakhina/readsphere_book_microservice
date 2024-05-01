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
        services.AddScoped<IOpenLibraryRepository, OpenLibraryRepository>();

        //register use cases
        services.AddScoped<GetAllBooksUseCase>();
        services.AddScoped<AddBookUseCase>();
        services.AddScoped<DeleteBookUseCase>();

        // Register facade
        services.AddScoped<IBookFacade, BookFacade>();
        services.AddSingleton<IDatabaseConfig, DatabaseConfig>();
    }
}