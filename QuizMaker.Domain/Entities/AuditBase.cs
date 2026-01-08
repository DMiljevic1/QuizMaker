namespace QuizMaker.Domain.Entities;

public class AuditBase<TKey> : EntityBase<TKey>
{
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DateDeleted { get; set; }
}
