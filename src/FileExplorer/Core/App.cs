using FileExplorer.ExtensionPlatfrom;
using FileExplorer.Utilities;
using FileExplorer.PluginHandlers;
using FileExplorer.FileHandling;
using FileExplorer.Configuration;
using FileExplorer.UI;

namespace FileExplorer.Core;
public class App
{
    private readonly IConsoleInterface _consoleInterface;
    private readonly IFileSearcher _fileSearcher;
    private readonly IPluginManager _pluginManager;
    private readonly AppConfig appConfig;
    private string _pluginPath = "";

    public App(IConsoleInterface consoleInterface, IFileSearcher fileSearcher, IPluginManager pluginManager, AppConfig appConfig)
    {
        _pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");

        _consoleInterface = consoleInterface;
        _fileSearcher = fileSearcher;
        _pluginManager = pluginManager;
        this.appConfig = appConfig;

        fileSearcher.SearchCompleted += FileSearcher_SearchCompleted;
        // Uncomment and implement this event handler
        // _pluginManager.PluginLoaded += PluginManager_PluginLoaded; 
        consoleInterface.HistoryViewed += ConsoleInterface_HistoryViewed;
    }


    public void Run()
    {
        bool isRunning = true;
        _pluginManager.LoadPlugins(_pluginPath, appConfig);
        while (isRunning)
        {
            //_consoleInterface.Clear();
            _consoleInterface.DisplayMainMenu();

            _consoleInterface.Warning(_pluginManager.GetWarning());
            int choice = _consoleInterface.Option();
            switch (choice)
            {
                case 1:
                    SearchByExtension();
                    break;
                case 2:
                    ManagePlugins();
                    break;
                case 3:
                    DisplaySearchHistory();
                    break;
                case 4:
                    isRunning = false;
                    break;
                default:
                    _consoleInterface.DisplayErrorMessage("Invalid choice. Please try again.\n\n\n");
                    break;
            }
            Helpers.PrintDividerLine();
        }
    }

    private void SearchByExtension()
    {
        List<string> chosenTypes = _consoleInterface.GetFileExtension(_pluginManager.GetTypeAllPlugins());

        List<IExtension> resultExtensions = new List<IExtension>();
        foreach (var choosType in chosenTypes)
        {
            var compatiblePlugins = _pluginManager.GetPluginsForExtension(choosType);
            IExtension selectedPlugin;

            if (compatiblePlugins.Count > 1)
            {
                selectedPlugin = _consoleInterface.ChoosePlugin(compatiblePlugins);
            }
            else
            {
                selectedPlugin = compatiblePlugins.Select(x => x.Extension).FirstOrDefault();
            }
            resultExtensions.Add(selectedPlugin);
        }

        string rootDirectory = _consoleInterface.GetRootDirectory();
        string query = _consoleInterface.GetQuery();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = cancellationTokenSource.Token;
        Thread loadingThread = new Thread(() => _consoleInterface.ShowLoading(token));
        loadingThread.Start();

        var foundFiles = _fileSearcher.SearchFiles(rootDirectory, resultExtensions, query);

        cancellationTokenSource.Cancel(); // Signal to stop the loading animation
        loadingThread.Join(); // Wait for the loading thread to finish

        _consoleInterface.DisplayMessage("\nSearch completed.");

        _fileSearcher.LogSearch(query, foundFiles.ToList());

        _consoleInterface.DisplaySearchResults(foundFiles.ToList());
    }
    private void DisplaySearchHistory()
    {
        var history = _fileSearcher.GetSearchHistory();
        _consoleInterface.DisplaySearchHistory(history);
    }
    private void FileSearcher_SearchCompleted(string query, List<string> results)
    {
        // Example: Log the completion of the search
        _consoleInterface.DisplayMessage($"Search for '{query}' completed. {results.Count} results found.");
    }
    private void PluginManager_PluginLoaded(string pluginName, bool success)
    {
        if (success)
        {
            _consoleInterface.DisplayMessage($"Plugin loaded successfully: {pluginName}");
        }
        else
        {
            _consoleInterface.DisplayErrorMessage($"Failed to load plugin: {pluginName}");
        }
    }
    private void ConsoleInterface_HistoryViewed(object sender, EventArgs e)
    {
        // Example action when history is viewed
        _consoleInterface.DisplayMessage("Search history was viewed.");
    }
    private void ManagePlugins()
    {
        var plugins = _pluginManager.ListPlugins();
        _consoleInterface.DisplayPlugins(plugins);
        // Add more plugin management functionalities here
    }


}