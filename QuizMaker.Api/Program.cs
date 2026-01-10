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
        Title = "Aymo API",
        Version = "v1"
    });

    

    c.SupportNonNullableReferenceTypes();
});

builder.Services.AddSerilogLogging(builder);
builder.Services.AddGlobalException();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();
