using Microsoft.AspNetCore.Mvc;
using QuizMaker.Application.Dto.Requests;
using QuizMaker.Application.Dto.Responses;
using QuizMaker.Application.Paginations;
using QuizMaker.Application.Services;

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
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateQuiz(CreateQuizRequest request, CancellationToken cancellationToken)
    {
        await _quizService.CreateQuiz(request, cancellationToken);
        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<QuizResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetQuizzes([FromQuery] PaginationParameters parameters, CancellationToken cancellationToken)
    {
        var quizzes = await _quizService.GetQuizzes(parameters, cancellationToken);
        return Ok(quizzes);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateQuiz([FromBody] UpdateQuizRequest request, CancellationToken cancellationToken)
    {
        await _quizService.UpdateQuiz(request, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{quizId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteQuiz([FromRoute] int quizId, CancellationToken cancellationToken)
    {
        await _quizService.DeleteQuiz(quizId, cancellationToken);
        return NoContent();
    }

    [HttpGet("exporters/{quizId}")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetAvailableExporters()
    {
        var formats = _quizService.GetAvailableExportFormats();
        return Ok(formats);
    }

    [HttpGet("export/{quizId}")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ExportQuiz([FromRoute] int quizId, [FromQuery] string format)
    {
        var fileBytes = await _quizService.ExportQuiz(quizId, format);
        return File(fileBytes, "application/octet-stream", $"quiz_{quizId}.{format.ToLower()}");
    }
}
