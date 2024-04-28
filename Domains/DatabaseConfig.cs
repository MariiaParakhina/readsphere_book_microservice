using Domains.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Domains;
public class DatabaseConfig : IDatabaseConfig
{
    private readonly IConfiguration _configuration;

    public DatabaseConfig(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string? GetConnectionString()
    {
        // Retrieve connection string from configuration
        return _configuration["DATABASE_CONNECTION_STRING"];
    }
}