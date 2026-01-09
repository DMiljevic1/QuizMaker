namespace QuizMaker.Domain.Entities;

public class Answer : AuditBase<int>
{
    public string Text { get; set; } = default!;
    public int QuestionId { get; set; }
    public Question Question { get; set; } = default!;
}
