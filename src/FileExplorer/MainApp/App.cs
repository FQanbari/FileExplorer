using FileExplorer.SearchManagement;
using FileExplorer.PluginManagement;
using FileExplorer.UserInterface;
using FileExplorer.EventManagement;

namespace FileExplorer.MainApp;
public class App
{
    private readonly IHistoryObservable _historyObservable;
    private readonly IHistoryObserver _historyObserver;
    private readonly FileSearcher fileSearcher;
    private readonly ConsoleInterface consoleInterface;
    private readonly PluginManager pluginManager;
    private string _pluginPath = "";



    public App()
    {
        consoleInterface = new ConsoleInterface();
        fileSearcher = new FileSearcher();
        pluginManager = new PluginManager();
        _pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
        _historyObservable = new HistoryObservable();
        _historyObserver = new HistoryManager();
        ((HistoryManager)_historyObserver).SubscribeToSearchObservable((IHistoryObservable)_historyObservable);        
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
                case 3:
                    // Add more menu options and corresponding actions here.
                    // Example: Search by file name, display search history, etc.
                    _historyObservable.PerformSearch("C# Events");
                    _historyObservable.PerformSearch("Observer Pattern");
                    ((HistoryManager)_historyObserver).DisplaySearchHistory();
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