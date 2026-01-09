namespace QuizMaker.Application.Dto.Requests;

public class CreateQuestionAnswerRequest
{
    public string QuestionText { get; set; } = default!;
    public string AnswerText { get; set; } = default!;
}
