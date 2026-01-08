namespace QuizMaker.Domain.Entities;

public class Question : EntityBase<int>
{
    public string Text { get; set; } = default!;

    public Answer? Answer { get; set; }
}
