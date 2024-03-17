using System.Runtime.InteropServices;
using ClangSharp;

namespace Gtk4Sharp.Bindings.Generator;

public class Generator
{
    public void Run(Configuration configuration)
    {
        PInvokeGeneratorConfigurationOptions pInvokeConfigOpts = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? PInvokeGeneratorConfigurationOptions.None
            : PInvokeGeneratorConfigurationOptions.GenerateUnixTypes;

        PInvokeGeneratorConfiguration pInvokeConfig = new PInvokeGeneratorConfiguration("c", String.Empty,
            configuration.DefaultNamespace, configuration.OutputDirectory, String.Empty,
            PInvokeGeneratorOutputMode.CSharp, pInvokeConfigOpts);

        using (var pInvokeGenerator = new PInvokeGenerator(pInvokeConfig))
        {
            
        }
    }
}