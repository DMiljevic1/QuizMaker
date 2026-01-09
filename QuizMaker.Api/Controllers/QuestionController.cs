using Microsoft.AspNetCore.Mvc;
using QuizMaker.Application.Dto.Responses;
using QuizMaker.Application.Paginations;
using QuizMaker.Application.Services;

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
    [ProducesResponseType(typeof(PagedResult<QuestionResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetQuestions([FromQuery] PaginationParameters parameters, [FromQuery] string? text, CancellationToken cancellationToken)
    {
        var result = await _questionService.GetQuestions(parameters, text, cancellationToken);
        return Ok(result);
    }

}
