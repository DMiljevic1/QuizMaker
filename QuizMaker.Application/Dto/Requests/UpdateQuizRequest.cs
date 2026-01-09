namespace QuizMaker.Application.Dto.Requests;

public class UpdateQuizRequest
{
    public int QuizId { get; set; }
    public string QuizName { get; set; } = default!;
    public ICollection<CreateQuestionRequest> NewQuestions { get; set; } = new List<CreateQuestionRequest>();
    public ICollection<int> ExistingQuestionIds { get; set; } = new List<int>();
}
