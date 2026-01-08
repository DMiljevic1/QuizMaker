namespace QuizMaker.Application.Dto.Responses;

public class QuestionAnswerResponse
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = default!;
    public int AnswerId { get; set; }
    public string AnswerText { get; set; } = default!;
}
