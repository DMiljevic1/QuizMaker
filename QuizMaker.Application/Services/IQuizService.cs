using QuizMaker.Application.Dto.Requests;

namespace QuizMaker.Application.Services;

public interface IQuizService
{
    Task CreateQuiz(CreateQuizRequest request, CancellationToken cancellationToken);
}
