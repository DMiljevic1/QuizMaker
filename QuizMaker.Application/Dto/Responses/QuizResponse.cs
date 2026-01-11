using QuizMaker.Domain.Entities;

namespace QuizMaker.Application.Dto.Responses;

public class QuizResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<QuestionAnswerResponse> QuestionsAnswers { get; set; } = new List<QuestionAnswerResponse>();
}

public static class QuizResponseExtension
{
    public static QuizResponse ToDto(this Quiz quiz)
    {
        return new QuizResponse
        {
            Id = quiz.Id,
            Name = quiz.Name,
            QuestionsAnswers = quiz.QuizQuestions.Select(x => x.ToDto()).ToList()
        };
    }
}
