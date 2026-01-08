namespace QuizMaker.Application.Dto.Responses;

public class QuizResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<QuestionAnswerResponse> QuestionsAnswers { get; set; } = new List<QuestionAnswerResponse>();
}
