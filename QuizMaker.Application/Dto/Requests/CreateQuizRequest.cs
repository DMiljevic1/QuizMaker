namespace QuizMaker.Application.Dto.Requests;

public class CreateQuizRequest
{
    public string QuizName { get; set; } = default!;
    public ICollection<CreateQuestionRequest> NewQuestions { get; set; } = new List<CreateQuestionRequest>();
    public ICollection<int> ExistingQuestionsIds { get; set; } = new List<int>();
}
