namespace QuizMaker.Domain.Entities;

public class QuizQuestion : AuditBase<int>
{
    public int QuizId { get; set; }
    public Quiz? Quiz { get; set; }
    public int QuestionId { get; set; }
    public Question? Question { get; set; }
}
