using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizMaker.Infrastructure.Contexts;

namespace QuizMaker.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureDbConnection(services, configuration);
    }

    private static void ConfigureDbConnection(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<QuizContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
    }
}
