namespace QuizMaker.Application.Dto.Requests;

public class CreateQuestionRequest
{
    public string QuestionText { get; set; } = default!;
    public string AnswerText { get; set; } = default!;
}
