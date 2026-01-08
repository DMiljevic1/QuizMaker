namespace QuizMaker.Domain.Entities;

public class Answer : EntityBase<int>
{
    public string Text { get; set; } = default!;
    public int QuestionId { get; set; }
    public Question? Question { get; set; }
}
