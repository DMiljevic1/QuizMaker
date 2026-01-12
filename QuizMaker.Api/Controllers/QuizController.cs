using Microsoft.AspNetCore.Mvc;
using QuizMaker.Application.Dto.Requests;
using QuizMaker.Application.Dto.Responses;
using QuizMaker.Application.Paginations;
using QuizMaker.Application.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace QuizMaker.Api.Controllers;

[Route("api/v1/quizzes")]
[ApiController]
public class QuizController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuizController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Creates a new quiz",
        Description = "Creates a quiz with the provided details. Returns 204 No Content on success. " +
                      "If the request is invalid, returns 400 Bad Request. " +
                      "Unexpected errors return 500 Internal Server Error."
    )]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateQuiz(CreateQuizRequest request, CancellationToken cancellationToken)
    {
        var quiz = await _quizService.CreateQuiz(request, cancellationToken);
        return Created($"/api/v1/quizzes/{quiz.Id}", quiz);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Retrieves a list of quizzes",
        Description = "Returns a paginated list of all quizzes. " +
                      "Supports query parameters for pagination such as page number and page size. " +
                      "Returns 200 OK with the list of quizzes. " +
                      "Unexpected errors return 500 Internal Server Error."
    )]
    [ProducesResponseType(typeof(PagedResult<QuizResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetQuizzes([FromQuery] PaginationParameters parameters, CancellationToken cancellationToken)
    {
        var quizzes = await _quizService.GetQuizzes(parameters, cancellationToken);
        return Ok(quizzes);
    }

    [HttpGet("{quizId}")]
    [SwaggerOperation(
        Summary = "Retrieves an existing quiz",
        Description = "Retrieves the details of an existing quiz. " +
                      "Returns 200 Ok on success or 404 Not Found if the quiz does not exist. " +
                      "Unexpected errors return 500 Internal Server Error."
    )]
    [ProducesResponseType(typeof(QuizResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQuiz([FromRoute] int quizId, CancellationToken cancellationToken)
    {
        var quiz = await _quizService.GetQuiz(quizId, cancellationToken);
        return Ok(quiz);
    }

    [HttpPut("{quizId}")]
    [SwaggerOperation(
        Summary = "Updates an existing quiz",
        Description = "Updates the details of an existing quiz. " +
                      "Returns 204 No Content on success, 400 Bad Request if the input is invalid, " +
                      "or 404 Not Found if the quiz does not exist. " +
                      "Unexpected errors return 500 Internal Server Error."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateQuiz([FromRoute] int quizId, [FromBody] UpdateQuizRequest request, CancellationToken cancellationToken)
    {
        await _quizService.UpdateQuiz(quizId, request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{quizId}")]
    [SwaggerOperation(
        Summary = "Deletes a quiz",
        Description = "Soft deletes a quiz by its ID. " +
                      "Returns 204 No Content if deletion was successful, or 404 Not Found if the quiz does not exist. " +
                      "Unexpected errors return 500 Internal Server Error."
    )]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteQuiz([FromRoute] int quizId, CancellationToken cancellationToken)
    {
        await _quizService.DeleteQuiz(quizId, cancellationToken);
        return NoContent();
    }

    [HttpGet("export-formats")]
    [SwaggerOperation(
        Summary = "Gets all supported export formats",
        Description = "Returns a list of all available export formats (e.g., CSV). " +
                      "Returns 200 OK with the list. " +
                      "Unexpected errors return 500 Internal Server Error."
    )]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public IActionResult GetAvailableExporters()
    {
        var formats = _quizService.GetAvailableExportFormats();
        return Ok(formats);
    }

    [HttpGet("export/{quizId}")]
    [SwaggerOperation(
        Summary = "Exports a quiz in a specific format",
        Description = "Exports the specified quiz in the requested format (e.g., CSV). " +
                      "Returns 200 OK with the file content, 400 Bad Request if the format is invalid, " +
                      "or 404 Not Found if the quiz does not exist. " +
                      "Unexpected errors return 500 Internal Server Error."
    )]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExportQuiz([FromRoute] int quizId, [FromQuery] string format)
    {
        var fileBytes = await _quizService.ExportQuiz(quizId, format);
        return File(fileBytes, "application/octet-stream", $"quiz_{quizId}.{format.ToLower()}");
    }
}
