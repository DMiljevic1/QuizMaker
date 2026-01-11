using Microsoft.AspNetCore.Mvc;
using QuizMaker.Api.Extensions;
using QuizMaker.Application;
using QuizMaker.Infrastructure;
using QuizMaker.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(ProblemDetails), StatusCodes.Status500InternalServerError));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "swagger doc",
        Version = "v1",
        Description = "API for managing quizzes and questions"
    });

    c.EnableAnnotations();

    c.SupportNonNullableReferenceTypes();
});

builder.Services.AddSerilogLogging(builder);
builder.Services.AddGlobalException();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quiz API v1");
    });
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
