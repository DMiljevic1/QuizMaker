using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizMaker.Application.Exporters;
using QuizMaker.Application.Services;
using QuizMaker.Infrastructure.Contexts;
using QuizMaker.Infrastructure.Exporters;
using QuizMaker.Infrastructure.Services;

namespace QuizMaker.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureDbConnection(services, configuration);
        ConfigureApplicationServices(services);
        ConfigureExporters(services);
    }

    private static void ConfigureDbConnection(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<QuizContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
    }

    private static void ConfigureApplicationServices(IServiceCollection services)
    {
        services.AddScoped<IQuizService, QuizService>();
        services.AddScoped<IQuestionService, QuestionService>();
    }

    private static void ConfigureExporters(IServiceCollection services)
    {
        services.AddSingleton<IExporterProvider, ExporterProvider>();
    }
}
