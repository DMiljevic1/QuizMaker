namespace QuizMaker.Domain.Entities;

public class EntityBase<TKey>
{
    public TKey Id { get; set; } = default!;
}
