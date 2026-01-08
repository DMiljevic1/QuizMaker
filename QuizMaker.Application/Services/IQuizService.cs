using QuizMaker.Application.Dto.Requests;
using QuizMaker.Application.Dto.Responses;
using QuizMaker.Application.Paginations;

namespace QuizMaker.Application.Services;

public interface IQuizService
{
    Task CreateQuiz(CreateQuizRequest request, CancellationToken cancellationToken);
    Task<PagedResult<QuizResponse>> GetQuizzes(PaginationParameters parameters, CancellationToken cancellationToken);
}
