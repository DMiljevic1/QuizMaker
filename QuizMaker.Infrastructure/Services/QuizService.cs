using Microsoft.EntityFrameworkCore;
using QuizMaker.Application.Dto.Requests;
using QuizMaker.Application.Dto.Responses;
using QuizMaker.Application.Paginations;
using QuizMaker.Application.Services;
using QuizMaker.Domain.Entities;
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
        var quiz = new Quiz
        {
            Name = request.Name
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

        await _context.Quizzes.AddAsync(quiz);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<PagedResult<QuizResponse>> GetQuizzes(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
