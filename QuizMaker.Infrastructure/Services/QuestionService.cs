using Microsoft.EntityFrameworkCore;
using QuizMaker.Application.Dto.Responses;
using QuizMaker.Application.Paginations;
using QuizMaker.Application.Services;
using QuizMaker.Infrastructure.Contexts;

namespace QuizMaker.Infrastructure.Services;

public class QuestionService : IQuestionService
{
    private readonly QuizContext _context;

    public QuestionService(QuizContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<QuestionResponse>> GetQuestions(PaginationParameters parameters, string? textFilter, CancellationToken cancellationToken)
    {
        var query = _context.Questions
            .AsNoTracking()
            .Select(x => new QuestionResponse
            {
                Id = x.Id,
                Text = x.Text,
            })
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(textFilter))
        {
            var normalized = textFilter.Trim().ToLower();
            query = query.Where(q => q.Text.ToLower().Contains(normalized));
        }

        return await query.ToPagedResultAsync(parameters.PageNumber, parameters.PageSize, cancellationToken);
    }
}
