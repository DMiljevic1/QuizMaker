using Microsoft.AspNetCore.Mvc;
using QuizMaker.Application.Dto.Responses;
using QuizMaker.Application.Paginations;
using QuizMaker.Application.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace QuizMaker.Api.Controllers;

[Route("api/v1/questions")]
[ApiController]
public class QuestionController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Retrieves a list of questions",
        Description = "Returns a paginated list of all questions. " +
                      "Supports query parameters for pagination such as page number and page size. " +
                      "You can optionally filter by the question text using the 'text' query parameter. " +
                      "Returns 200 OK with the list of questions. " +
                      "Unexpected errors return 500 Internal Server Error."
    )]
    [ProducesResponseType(typeof(PagedResult<QuestionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetQuestions([FromQuery] PaginationParameters parameters, [FromQuery] string? text, CancellationToken cancellationToken)
    {
        var result = await _questionService.GetQuestions(parameters, text, cancellationToken);
        return Ok(result);
    }

}
