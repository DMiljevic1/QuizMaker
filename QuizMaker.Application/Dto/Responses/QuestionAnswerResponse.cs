using QuizMaker.Domain.Entities;

namespace QuizMaker.Application.Dto.Responses;

public class QuestionAnswerResponse
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = default!;
    public int AnswerId { get; set; }
    public string AnswerText { get; set; } = default!;
}

public static class QuestionAnswerResponseExtension
{
    public static QuestionAnswerResponse ToDto(this QuizQuestion quizQuestion)
    {
        return new QuestionAnswerResponse
        {
            QuestionId = quizQuestion.QuestionId,
            QuestionText = quizQuestion.Question.Text,
            AnswerId = quizQuestion.Question.Answer.Id,
            AnswerText = quizQuestion.Question.Answer.Text
        };
    }
}
