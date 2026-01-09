using Serilog;

namespace QuizMaker.Api.Extensions;

public static class SerilogExtension
{
    public static void AddSerilogLogging(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));
    }
}
