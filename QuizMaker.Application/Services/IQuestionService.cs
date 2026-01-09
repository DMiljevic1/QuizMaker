using QuizMaker.Application.Dto.Responses;
using QuizMaker.Application.Paginations;

namespace QuizMaker.Application.Services;

public interface IQuestionService
{
    Task<PagedResult<QuestionResponse>> GetQuestions(PaginationParameters parameters, string? textFilter, CancellationToken cancellationToken);
}
