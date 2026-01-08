namespace QuizMaker.Domain.Entities;

public class QuizQuestion : EntityBase<int>
{
    public int QuizId { get; set; }
    public Quiz? Quiz { get; set; }
    public int QuestionId { get; set; }
    public Question? Question { get; set; }
}
