namespace Gtk4Sharp.Bindings.Generator;

public class Configuration
{
    public required Library Library { get; set; }
    public required string WorkDirPath { get; set; }
    public required string OutputDirectory { get; set; }
    public required string DefaultNamespace { get; set; }
    public required List<string> InputFiles { get; set; }
}