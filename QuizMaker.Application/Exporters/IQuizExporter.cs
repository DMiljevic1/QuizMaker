using QuizMaker.Domain.Entities;

namespace QuizMaker.Application.Exporters;

public interface IQuizExporter
{
    string ExportFormat { get; }
    byte[] Export(Quiz quiz);
}
