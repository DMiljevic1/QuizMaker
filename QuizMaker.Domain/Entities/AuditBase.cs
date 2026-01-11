using QuizMaker.Domain.Interfaces;

namespace QuizMaker.Domain.Entities;

public abstract class AuditBase<TKey> : EntityBase<TKey>, IAuditable
{
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DateDeleted { get; set; }
}