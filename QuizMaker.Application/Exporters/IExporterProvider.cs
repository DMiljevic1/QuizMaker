namespace QuizMaker.Application.Exporters;

public interface IExporterProvider
{
    IEnumerable<string> GetAvailableFormats();
    IQuizExporter GetExporter(string format);
}
