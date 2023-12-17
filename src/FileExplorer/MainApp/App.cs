

using FileExplorer.FileSearchLogic;
using FileExplorer.MainApp;
using FileExplorer.PluginInterface;
using FileExplorer.UserInterface;
using System.Reflection;

public class App
{
    private readonly FileSearcher fileSearcher;
    private readonly ConsoleInterface consoleInterface;
    private readonly PluginLoader plugin;
    private string _pluginPath = "";



    public App()
    {
        fileSearcher = new FileSearcher();
        consoleInterface = new ConsoleInterface();
        plugin = new PluginLoader();
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string projectDirectory = Directory.GetParent(baseDirectory).Parent.Parent.FullName;
        _pluginPath = Path.Combine(baseDirectory, "plugins");
    }

    public void Run()
    {
        bool isRunning = true;

        while (isRunning)
        {
            // Display the main menu and get user input.
            consoleInterface.DisplayMainMenu();
            plugin.LoadPlugin(_pluginPath);
            plugin.Waringn();
            int choice = consoleInterface.Option();

            switch (choice)
            {
                case 1:
                    // Search for files by extension.
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
        // Get user input for file extension and root directory.
        string fileExtension = consoleInterface.GetFileExtension(plugin.Extensionenstions());
        string rootDirectory = consoleInterface.GetRootDirectory();
        string query = consoleInterface.GetQuery();

        // Perform file search and display results.
        var foundFiles = fileSearcher.SearchFilesByExtension(rootDirectory, fileExtension);
        consoleInterface.DisplaySearchResults(foundFiles);
    }
}