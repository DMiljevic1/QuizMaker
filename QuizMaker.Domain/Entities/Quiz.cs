using QuizMaker.Domain.Interfaces;

namespace QuizMaker.Domain.Entities;

public class Quiz : AuditBase<int>, ISoftDeleteAggregate
{
    public string Name { get; set; } = default!;

    public ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();

    public void SoftDelete()
    {
        if(IsDeleted) 
            return;

        IsDeleted = true;
        DateDeleted = DateTime.UtcNow;

        foreach(var qq in QuizQuestions)
        {
            qq.IsDeleted = true;
            qq.DateDeleted = DateTime.UtcNow;
        }
    }
}
