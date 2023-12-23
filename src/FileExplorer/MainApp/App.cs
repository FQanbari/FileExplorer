using FileExplorer.SearchManagement;
using FileExplorer.PluginManagement;
using FileExplorer.UserInterface;
using FileExplorer.HistoryManagement;
using System.IO;

namespace FileExplorer.MainApp;
public class App
{
    
    private readonly FileSearcher fileSearcher;
    private readonly ConsoleInterface consoleInterface;
    private readonly PluginManager pluginManager;
    private readonly HistoryManager historyManagemer;
    private string _pluginPath = "";
    private readonly IObserver _searchHistoryObserver;

    public App()
    {
        consoleInterface = new ConsoleInterface();
        fileSearcher = new FileSearcher();
        pluginManager = new PluginManager();
        historyManagemer = new HistoryManager(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs.json"));
        _pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
        _searchHistoryObserver = new SearchHistoryObserver(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs.json"));
        consoleInterface.EventOccurred += _searchHistoryObserver.Update;
    }

    public void Run()
    {        
        bool isRunning = true;
        pluginManager.LoadPlugins(_pluginPath);
        while (isRunning)
        {
            consoleInterface.Clear();
            consoleInterface.DisplayMainMenu();
            
            consoleInterface.Warning(pluginManager.GetWarning());
            int choice = consoleInterface.Option();
            switch (choice)
            {
                case 1:
                    SearchByExtension();
                    consoleInterface.Stop();
                    break;
                case 3:
                    // Add more menu options and corresponding actions here.
                    // Example: Search by file name, display search history, etc.
                    consoleInterface.DisplayHistoryResults(historyManagemer.LoadSearchHistory());
                    consoleInterface.Stop();

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