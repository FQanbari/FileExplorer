

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
    private List<IFileTypePlugin> _plugins;
    private string _pluginPath = "";



    public App()
    {
        fileSearcher = new FileSearcher();
        consoleInterface = new ConsoleInterface();
        pluginManager = new PluginManager();
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        _plugins = new List<IFileTypePlugin>();
        string projectDirectory = Directory.GetParent(baseDirectory).Parent.Parent.FullName;
        _pluginPath = Path.Combine(baseDirectory, "plugins");
    }

    public void Run()
    {
        bool isRunning = true;

        while (isRunning)
        {
            consoleInterface.DisplayMainMenu();
            _plugins = pluginManager.LoadPlugins(_pluginPath);
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
        string fileExtension = consoleInterface.GetFileExtension(_plugins.Select(x => x.TypeName).ToList());

        string rootDirectory = consoleInterface.GetRootDirectory();

        string query = consoleInterface.GetQuery();

        var foundFiles = fileSearcher.SearchFiles(rootDirectory, _plugins, query);

        consoleInterface.DisplaySearchResults(foundFiles.ToList());
    }
}