using QuizMaker.Application.Exceptions;
using QuizMaker.Application.Exporters;
using System.Composition.Hosting;
using System.Reflection;

namespace QuizMaker.Infrastructure.Exporters;

public class ExporterProvider : IExporterProvider
{
    private readonly Dictionary<string, IQuizExporter> _exporters;

    public ExporterProvider()
    {
        var assemblies = new[] { Assembly.GetExecutingAssembly() };
        var configuration = new ContainerConfiguration().WithAssemblies(assemblies);
        using var container = configuration.CreateContainer();

        var exporters = container.GetExports<IQuizExporter>();
        _exporters = exporters.ToDictionary(e => e.ExportFormat, e => e);
    }

    public IEnumerable<string> GetAvailableFormats()
    {
        return _exporters.Keys;
    }

    public IQuizExporter GetExporter(string format)
    {
        if (string.IsNullOrWhiteSpace(format))
            throw new ArgumentException("Format not provided", nameof(format));

        var normalizedFormat = format.Trim().ToUpperInvariant();

        if (!_exporters.TryGetValue(normalizedFormat, out var exporter))
            throw new NotFoundException($"Exporter not found. Format={format}");

        return exporter;
    }
}
