using FluentValidation;
using Microsoft.EntityFrameworkCore;
using QuizMaker.Application.Dto.Requests;
using QuizMaker.Application.Dto.Responses;
using QuizMaker.Application.Exceptions;
using QuizMaker.Application.Exporters;
using QuizMaker.Application.Paginations;
using QuizMaker.Application.Services;
using QuizMaker.Domain.Entities;
using QuizMaker.Infrastructure.Contexts;
using QuizMaker.Infrastructure.Exporters;

namespace QuizMaker.Infrastructure.Services;

public class QuizService : IQuizService
{
    private readonly QuizContext _context;
    private readonly IExporterProvider _exporterProvider;
    private readonly IValidator<CreateQuizRequest> _createValidator;
    private readonly IValidator<UpdateQuizRequest> _updateValidator;

    public QuizService(QuizContext context, IExporterProvider exporterProvider, IValidator<CreateQuizRequest> createValidator, IValidator<UpdateQuizRequest> updateValidator)
    {
        _context = context;
        _exporterProvider = exporterProvider;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task CreateQuiz(CreateQuizRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var quiz = new Quiz
        {
            Name = request.QuizName
        };

        if (request.ExistingQuestionsIds.Any())
        {
            var existingQuestions = await _context.Questions
                .Where(q => request.ExistingQuestionsIds.Contains(q.Id))
                .ToListAsync(cancellationToken);

            foreach (var question in existingQuestions)
            {
                quiz.QuizQuestions.Add(new QuizQuestion
                {
                    Question = question
                });
            }
        }

        foreach (var newQuestion in request.NewQuestions)
        {
            var question = new Question
            {
                Text = newQuestion.QuestionText,
                Answer = new Answer
                {
                    Text = newQuestion.AnswerText
                }
            };

            quiz.QuizQuestions.Add(new QuizQuestion
            {
                Question = question
            });
        }

        await _context.Quizzes.AddAsync(quiz, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<PagedResult<QuizResponse>> GetQuizzes(PaginationParameters parameters, CancellationToken cancellationToken)
    {
        var query = _context.Quizzes
            .AsNoTracking()
            .Select(x => new QuizResponse
            {
                Id = x.Id,
                Name = x.Name,
                QuestionsAnswers = x.QuizQuestions
                    .Select(qc => new QuestionAnswerResponse
                    {
                        QuestionId = qc.QuestionId,
                        QuestionText = qc.Question.Text,
                        AnswerId = qc.Question.Answer!.Id,
                        AnswerText = qc.Question.Answer.Text
                    })
                    .ToList()
            })
            .AsQueryable();

        return await query.ToPagedResultAsync(parameters.PageNumber, parameters.PageSize, cancellationToken);
    }

    public async Task UpdateQuiz(UpdateQuizRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var quiz = await _context.Quizzes
            .Include(x => x.QuizQuestions).ThenInclude(x => x.Question).ThenInclude(x => x.Answer)
            .FirstOrDefaultAsync(x => x.Id == request.QuizId, cancellationToken);

        if (quiz == null)
            throw new NotFoundException($"Quiz not found. QuizId={request.QuizId}");

        quiz.Name = request.QuizName;

        var requestedQuestions = request.NewQuestions;

        var existingQuestionIds = request.ExistingQuestionIds
            .ToHashSet();

        var toRemove = quiz.QuizQuestions
            .Where(x => !existingQuestionIds.Contains(x.QuestionId))
            .ToList();

        foreach(var quizQuestion in toRemove)
        {
            quiz.QuizQuestions.Remove(quizQuestion);
        }

        foreach (var questionId in request.ExistingQuestionIds)
        {
            if (!quiz.QuizQuestions.Any(x => x.QuestionId == questionId))
                quiz.QuizQuestions.Add(new QuizQuestion { QuestionId = questionId });
        }

        foreach (var newQ in request.NewQuestions)
        {
            var question = new Question
            {
                Text = newQ.QuestionText,
                Answer = new Answer { Text = newQ.AnswerText }
            };

            quiz.QuizQuestions.Add(new QuizQuestion { Question = question });
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteQuiz(int quizId, CancellationToken cancellationToken)
    {
        var quiz = await _context.Quizzes
            .FirstOrDefaultAsync(x => x.Id == quizId, cancellationToken);

        if (quiz == null)
            throw new NotFoundException($"Quiz not found. QuizId={quizId}");

        _context.Quizzes.Remove(quiz);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public IEnumerable<string> GetAvailableExportFormats()
    {
        return _exporterProvider.GetAvailableFormats();
    }

    public async Task<byte[]> ExportQuiz(int quizId, string format)
    {
        var quiz = await _context.Quizzes
            .AsNoTracking()
            .Include(x => x.QuizQuestions).ThenInclude(x => x.Question)
            .FirstOrDefaultAsync(x => x.Id == quizId);

        if (quiz == null)
            throw new NotFoundException($"Quiz not found. QuizId={quizId}");

        var exporter = _exporterProvider.GetExporter(format);
        return exporter.Export(quiz);
    }
}
