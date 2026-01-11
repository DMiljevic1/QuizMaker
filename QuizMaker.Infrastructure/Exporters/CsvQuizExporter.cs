using QuizMaker.Application.Exporters;
using QuizMaker.Domain.Entities;
using System.Composition;
using System.Text;

namespace QuizMaker.Infrastructure.Exporters;

[Export(typeof(IQuizExporter))]
public class CsvQuizExporter : IQuizExporter
{
    public string ExportFormat => "CSV";

    public byte[] Export(Quiz quiz)
    {
        var sb = new StringBuilder();
        sb.AppendLine(quiz.Name);
        foreach (var q in quiz.QuizQuestions)
            sb.AppendLine($"\"{q.Question.Text}\"");

        return Encoding.UTF8.GetBytes(sb.ToString());
    }
}
