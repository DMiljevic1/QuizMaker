using System.ComponentModel.DataAnnotations;

namespace QuizMaker.Domain.Entities;

public class Quiz : AuditBase<int>
{
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    public ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
}
