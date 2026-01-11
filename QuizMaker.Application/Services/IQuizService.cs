using QuizMaker.Application.Dto.Requests;
using QuizMaker.Application.Dto.Responses;
using QuizMaker.Application.Paginations;

namespace QuizMaker.Application.Services;

public interface IQuizService
{
    Task<QuizResponse> CreateQuiz(CreateQuizRequest request, CancellationToken cancellationToken);
    Task<PagedResult<QuizResponse>> GetQuizzes(PaginationParameters parameters, CancellationToken cancellationToken);
    Task UpdateQuiz(int quizId, UpdateQuizRequest request, CancellationToken cancellationToken);
    Task DeleteQuiz(int quizId, CancellationToken cancellationToken);
    IEnumerable<string> GetAvailableExportFormats();
    Task<byte[]> ExportQuiz(int quizId, string format);
}
