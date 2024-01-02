using FileExplorer.SearchManagement;
using FileExplorer.PluginManagement;
using FileExplorer.UserInterface;
using FileExplorer.ExtensionPlatfrom;
using FileExplorer.Utilities;

namespace FileExplorer.MainApp;
public class App
{

    private readonly FileSearcher fileSearcher;
    private readonly ConsoleInterface consoleInterface;
    private readonly PluginManager pluginManager;
    private string _pluginPath = "";


    public App()
    {
        _pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
        consoleInterface = new ConsoleInterface();
        fileSearcher = new FileSearcher();
        pluginManager = new PluginManager();

        fileSearcher.SearchCompleted += FileSearcher_SearchCompleted;
        // Uncomment and implement this event handler
        // pluginManager.PluginLoaded += PluginManager_PluginLoaded; 
        consoleInterface.HistoryViewed += ConsoleInterface_HistoryViewed;
    }


    public void Run()
    {
        bool isRunning = true;
        pluginManager.LoadPlugins(_pluginPath);
        while (isRunning)
        {
            //consoleInterface.Clear();
            consoleInterface.DisplayMainMenu();

            consoleInterface.Warning(pluginManager.GetWarning());
            int choice = consoleInterface.Option();
            switch (choice)
            {
                case 1:
                    SearchByExtension();
                    //consoleInterface.Stop();
                    break;
                case 2:
                    ManagePlugins();
                    break;
                case 3:
                    DisplaySearchHistory();
                    //consoleInterface.Stop();
                    break;
                case 4:
                    isRunning = false;
                    break;
                default:
                    consoleInterface.DisplayErrorMessage("Invalid choice. Please try again.\n\n\n");
                    break;
            }
            Helpers.PrintDividerLine();
        }
    }

    private void SearchByExtension()
    {
        List<string> chosenTypes = consoleInterface.GetFileExtension(pluginManager.GetTypeAllPlugins());

        List<IExtension> resultExtensions = new List<IExtension>();
        foreach (var choosType in chosenTypes)
        {
            var compatiblePlugins = pluginManager.GetPluginsForExtension(choosType);
            IExtension selectedPlugin;

            if (compatiblePlugins.Count > 1)
            {
                selectedPlugin = consoleInterface.ChoosePlugin(compatiblePlugins);
            }
            else
            {
                selectedPlugin = compatiblePlugins.Select(x => x.Extension).FirstOrDefault();
            }
            resultExtensions.Add(selectedPlugin);
        }

        string rootDirectory = consoleInterface.GetRootDirectory();
        string query = consoleInterface.GetQuery();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = cancellationTokenSource.Token;
        Thread loadingThread = new Thread(() => consoleInterface.ShowLoading(token));
        loadingThread.Start();

        var foundFiles = fileSearcher.SearchFiles(rootDirectory, resultExtensions, query);

        cancellationTokenSource.Cancel(); // Signal to stop the loading animation
        loadingThread.Join(); // Wait for the loading thread to finish

        consoleInterface.DisplayMessage("\nSearch completed.");

        fileSearcher.LogSearch(query, foundFiles.ToList());

        consoleInterface.DisplaySearchResults(foundFiles.ToList());
    }
    private void DisplaySearchHistory()
    {
        var history = fileSearcher.GetSearchHistory();
        consoleInterface.DisplaySearchHistory(history);
    }
    private void FileSearcher_SearchCompleted(string query, List<string> results)
    {
        // Example: Log the completion of the search
        consoleInterface.DisplayMessage($"Search for '{query}' completed. {results.Count} results found.");
    }
    private void PluginManager_PluginLoaded(string pluginName, bool success)
    {
        if (success)
        {
            consoleInterface.DisplayMessage($"Plugin loaded successfully: {pluginName}");
        }
        else
        {
            consoleInterface.DisplayErrorMessage($"Failed to load plugin: {pluginName}");
        }
    }
    private void ConsoleInterface_HistoryViewed(object sender, EventArgs e)
    {
        // Example action when history is viewed
        consoleInterface.DisplayMessage("Search history was viewed.");
    }
    private void ManagePlugins()
    {
        var plugins = pluginManager.ListPlugins();
        consoleInterface.DisplayPlugins(plugins);
        // Add more plugin management functionalities here
    }


}