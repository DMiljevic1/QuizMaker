using QuizMaker.Application.Exceptions;
using QuizMaker.Application.Exporters;
using System.Composition.Hosting;
using System.Reflection;

namespace QuizMaker.Infrastructure.Exporters;

public class ExporterProvider : IExporterProvider
{
    private readonly Dictionary<string, IQuizExporter> _exporters = new(StringComparer.OrdinalIgnoreCase);

    public ExporterProvider()
    {
        LoadAllExporters();
    }

    private void LoadAllExporters()
    {
        var assemblies = new List<Assembly> { Assembly.GetExecutingAssembly() };

        var exportersFolder = Path.Combine(AppContext.BaseDirectory, "Exporters");

        if (!Directory.Exists(exportersFolder))
        {
            Directory.CreateDirectory(exportersFolder);
        }

        var dlls = Directory.GetFiles(exportersFolder, "*.dll", SearchOption.TopDirectoryOnly);

        foreach (var dll in dlls)
        {
            try
            {
                var assembly = Assembly.LoadFrom(dll);

                if (!assemblies.Contains(assembly))
                {
                    assemblies.Add(assembly);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri učitavanju DLL-a {dll}: {ex.Message}");
            }
        }

        try
        {
            var configuration = new ContainerConfiguration().WithAssemblies(assemblies);
            using var container = configuration.CreateContainer();

            var exporters = container.GetExports<IQuizExporter>();

            foreach (var exporter in exporters)
            {
                var formatKey = exporter.ExportFormat.Trim().ToUpperInvariant();

                if (!_exporters.ContainsKey(formatKey))
                {
                    _exporters.Add(formatKey, exporter);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MEF Error: {ex.Message}");
        }
    }

    public IEnumerable<string> GetAvailableFormats()
    {
        return _exporters.Keys;
    }

    public IQuizExporter GetExporter(string format)
    {
        if (string.IsNullOrWhiteSpace(format))
            throw new BadRequestException("Format not provided.");

        if (!_exporters.TryGetValue(format.Trim(), out var exporter))
            throw new NotFoundException($"Exporter not found. Format={format}");

        return exporter;
    }
}
