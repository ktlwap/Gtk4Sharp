using System.Runtime.InteropServices;
using ClangSharp;
using ClangSharp.Interop;

namespace Gtk4Sharp.Bindings.Generator;

public static class Generator
{
    public static void Run(Configuration configuration)
    {
        PInvokeGeneratorConfigurationOptions pInvokeConfigOpts = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? PInvokeGeneratorConfigurationOptions.None
            : PInvokeGeneratorConfigurationOptions.GenerateUnixTypes;

        PInvokeGeneratorConfiguration pInvokeConfig = new PInvokeGeneratorConfiguration("c", String.Empty,
            configuration.DefaultNamespace, configuration.OutputDirectory, String.Empty,
            PInvokeGeneratorOutputMode.CSharp, pInvokeConfigOpts)
        {
            IncludedNames =
            [
                configuration.WorkDirPath + "/gtk"
            ]
        };

        using var pInvokeGenerator = new PInvokeGenerator(pInvokeConfig);

        CXTranslationUnit_Flags translationUnitFlags = CXTranslationUnit_Flags.CXTranslationUnit_None; 
        translationUnitFlags |= CXTranslationUnit_Flags.CXTranslationUnit_IncludeAttributedTypes;
        translationUnitFlags |= CXTranslationUnit_Flags.CXTranslationUnit_VisitImplicitAttributes;

        string[] clangCommandLineArgs = new string[]
        {
            "--language=c",
            "-Wno-pragma-once-outside-header"
        };
        
        foreach (string file in configuration.InputFiles)
        {
            string filePath = Path.Combine(configuration.WorkDirPath, file);
            
            CXErrorCode translationUnitError = CXTranslationUnit.TryParse(pInvokeGenerator.IndexHandle, filePath,
                ReadOnlySpan<string>.Empty, Array.Empty<CXUnsavedFile>(), translationUnitFlags, out CXTranslationUnit handle);

            if (translationUnitError != CXErrorCode.CXError_Success)
                throw new Exception($"Failed to parse file: \"{file}\" with error: \"{translationUnitError}\"");
            
            if (handle.NumDiagnostics != 0)
            {
                Console.WriteLine($"Diagnostics for '{filePath}':");

                for (uint i = 0; i < handle.NumDiagnostics; ++i)
                {
                    using var diagnostic = handle.GetDiagnostic(i);

                    Console.Write("    ");
                    Console.WriteLine(diagnostic.Format(CXDiagnostic.DefaultDisplayOptions).ToString());
                }
            }
                
            using TranslationUnit translationUnit = TranslationUnit.GetOrCreate(handle);

            Console.WriteLine($"Processing {filePath}.");
            
            pInvokeGenerator.GenerateBindings(translationUnit, filePath, clangCommandLineArgs, translationUnitFlags);
        }
    }
}