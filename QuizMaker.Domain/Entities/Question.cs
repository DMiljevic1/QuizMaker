namespace QuizMaker.Domain.Entities;

public class Question : AuditBase<int>
{
    public string Text { get; set; } = default!;

    public Answer Answer { get; set; } = default!;
}
