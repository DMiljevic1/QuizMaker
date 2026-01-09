namespace QuizMaker.Application.Dto.Requests;

public class UpdateQuizRequest
{
    public int QuizId { get; set; }
    public string QuizName { get; set; } = default!;
    public ICollection<CreateQuestionAnswerRequest> NewQuestionsAnswers { get; set; } = new List<CreateQuestionAnswerRequest>();
    public ICollection<int> ExistingQuestionIds { get; set; } = new List<int>();
}
