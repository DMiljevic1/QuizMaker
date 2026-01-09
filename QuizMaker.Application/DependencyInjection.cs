using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using QuizMaker.Application.Dto.Requests;
using QuizMaker.Application.Validators;

namespace QuizMaker.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        ConfigureValidators(services);
    }

    private static void ConfigureValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateQuizRequest>, CreateQuizValidator>();
        services.AddScoped<IValidator<CreateQuestionRequest>, CreateQuestionValidator>();
        services.AddScoped<IValidator<UpdateQuizRequest>, UpdateQuizValidator>();
    }
}
