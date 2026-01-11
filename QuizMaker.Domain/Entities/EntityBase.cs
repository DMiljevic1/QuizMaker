namespace QuizMaker.Domain.Entities;

public abstract class EntityBase<TKey>
{
    public TKey Id { get; set; } = default!;
}
