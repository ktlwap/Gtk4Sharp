namespace Gtk4Sharp.Bindings.Generator;

class Program
{
    static void Main(string[] args)
    {
        Configuration gtkConfiguration = new Configuration()
        {
            Library = Library.Gtk,
            DefaultNamespace = "Gtk4Sharp.Gtk",
            WorkDirPath = "./build/submodules/gtk",
            OutputDirectory = "./src/Gtk",
            InputFiles = [ "gtk/gtk.h" ]
        };

        Generator.Run(gtkConfiguration);
    }
}