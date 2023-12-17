using FileExplorer.FileSearch;

namespace FileExplorer.MainApp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the File Search Application!");

        // Initialize and run the application.
        var app = new App();
        app.Run();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
        // Step 2: Create an instance of PluginLoader.
        var pluginLoader = new PluginLoader();

        // Step 3: Load the plugin DLL (provide the full path to the DLL).
        string pluginPath = @"plugins\txtfilehandler.dll"; // Adjust the path as needed.
        IFileTypeHandler txtFileHandler = pluginLoader.LoadPlugin(pluginPath);

        if (txtFileHandler != null)
        {
            Console.WriteLine("Plugin loaded successfully.");

            // Example: Process a TXT file using the loaded handler.
            string filePath = @"path\to\your\textfile.txt"; // Adjust the path as needed.

            if (filePath.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                // Step 4: Use the loaded handler to process the file.
                //txtFileHandler.Search(filePath);
            }
            else
            {
                Console.WriteLine("File is not a TXT file.");
            }
        }
        else
        {
            Console.WriteLine("Plugin not found or compatible handler not available.");
        }
    }
}
