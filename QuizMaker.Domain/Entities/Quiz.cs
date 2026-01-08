namespace QuizMaker.Domain.Entities;

public class Quiz : EntityBase<int>
{
    public string Name { get; set; } = default!;

    public ICollection<Question> Questions { get; set; } = new List<Question>();
}
