namespace QuizMaker.Domain.Interfaces;

public interface IAuditable
{
    DateTime DateCreated { get; set; }
    DateTime? DateUpdated { get; set; }
    bool IsDeleted { get; set; }
    DateTime? DateDeleted { get; set; }
}
