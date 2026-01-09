using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuizMaker.Application.Exceptions;
using FluentValidation;

namespace QuizMaker.Api.Handlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails();

        switch (exception)
        {
            case ValidationException ve:
                _logger.LogWarning(exception, "Validation error: {Message}", ve.Message);
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Validation Failed";
                problemDetails.Detail = string.Join(" ", ve.Errors.Select(e => e.ErrorMessage));
                break;

            case NotFoundException:
                _logger.LogWarning(exception, "Not found: {Message}", exception.Message);
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Title = "Resource Not Found";
                problemDetails.Detail = "The requested item could not be found.";
                break;

            case BadHttpRequestException bhe:
                _logger.LogWarning(exception, "Bad request: {Message}", bhe.Message);
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Invalid Request";
                problemDetails.Detail = "The request could not be understood or was missing required parameters.";
                break;

            case BadRequestException bre:
                _logger.LogWarning(exception, "Bad request: {Message}", bre.Message);
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Bad Request";
                problemDetails.Detail = bre.Message;
                break;

            default:
                _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Server Error";
                problemDetails.Detail = "An unexpected error occurred. Please try again later.";
                break;
        }

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
