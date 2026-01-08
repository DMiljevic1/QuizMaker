using QuizMaker.Application.Dto.Requests;
using QuizMaker.Application.Dto.Responses;
using QuizMaker.Application.Paginations;
using QuizMaker.Application.Services;
using QuizMaker.Infrastructure.Contexts;

namespace QuizMaker.Infrastructure.Services;

public class QuizService : IQuizService
{
    private readonly QuizContext _context;

    public QuizService(QuizContext context)
    {

        _context = context;
    }

    public async Task CreateQuiz(CreateQuizRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResult<QuizResponse>> GetQuizzes(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
