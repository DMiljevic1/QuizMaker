using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizMaker.Application.Dto.Requests;
using QuizMaker.Application.Services;

namespace QuizMaker.Api.Controllers
{
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
        [Route("create")]
        public async Task<IActionResult> CreateQuiz(CreateQuizRequest request, CancellationToken cancellationToken)
        {
            await _quizService.CreateQuiz(request, cancellationToken);
            return NoContent();
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetQuizzes(CancellationToken cancellationToken)
        {
            var quizzes = await _quizService.GetQuizzes(cancellationToken);
            return Ok(quizzes);
        }
    }
}
