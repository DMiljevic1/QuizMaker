using QuizMaker.Api.Handlers;

namespace QuizMaker.Api.Extensions
{
    public static class GlobalExceptionExtension
    {
        public static void AddGlobalException(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
        }
    }
}
