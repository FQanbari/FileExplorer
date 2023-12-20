

using FileExplorer.FileSearch;
using FileExplorer.SearchManagement;
using FileExplorer.PluginInterface;
using FileExplorer.PluginManagement;
using FileExplorer.UserInterface;
using Microsoft.VisualBasic.FileIO;
using FileSearcher = FileExplorer.SearchManagement.FileSearcher;
using PluginManager = FileExplorer.PluginManagement.PluginManager;

namespace FileExplorer.MainApp;
public class App
{
    private readonly FileSearcher fileSearcher;
    private readonly ConsoleInterface consoleInterface;
    private readonly PluginManager pluginManager;
    private string _pluginPath = "";



    public App()
    {
        fileSearcher = new FileSearcher();
        consoleInterface = new ConsoleInterface();
        pluginManager = new PluginManager();
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string projectDirectory = Directory.GetParent(baseDirectory).Parent.Parent.FullName;
        _pluginPath = Path.Combine(baseDirectory, "_plugins");
    }

    public void Run()
    {
        bool isRunning = true;

        while (isRunning)
        {
            consoleInterface.DisplayMainMenu();
            pluginManager.LoadPlugins(_pluginPath);
            pluginManager.Warning();
            int choice = consoleInterface.Option();

            switch (choice)
            {
                case 1:
                    SearchByExtension();
                    break;
                case 2:
                    // Add more menu options and corresponding actions here.
                    // Example: Search by file name, display search history, etc.
                    break;
                case 4:
                    // Exit the application.
                    isRunning = false;
                    break;
                default:
                    consoleInterface.DisplayErrorMessage("Invalid choice. Please try again.\n\n\n");
                    break;
            }
        }
    }

    private void SearchByExtension()
    {
        List<string> fileExtensions = consoleInterface.GetFileExtension(pluginManager.GetPluginNames());

        string rootDirectory = consoleInterface.GetRootDirectory();

        string query = consoleInterface.GetQuery();

        var foundFiles = fileSearcher.SearchFiles(rootDirectory, pluginManager.GetPluginsByExtensionsInput(fileExtensions), query);

        consoleInterface.DisplaySearchResults(foundFiles.ToList());
    }     
}