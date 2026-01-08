namespace QuizMaker.Domain.Entities;

public class EntityBase<TKey>
{
    public TKey Id { get; set; } = default!;
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DateDeleted { get; set; }
}
