using FileExplorer.ExtensionPlatfrom;
using FileExplorer.FileHandling;
using FileExplorer.Utilities;
using System.Linq;

namespace FileExplorer.UI;

public class ConsoleInterface : IConsoleInterface
{
    public event EventHandler HistoryViewed;

    public void DisplayMainMenu()
    {
        Helpers.Enter();
        Helpers.Info("1. Search for files");
        Helpers.Info("2. Manage Extensions");
        Helpers.Info("3. View search history\n");
        Helpers.Error("4. Exit");
        Helpers.Enter();
    }

    public int Option()
    {
        Console.Write("Please input an option: ");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            return choice;
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid menu choice.\n\n\n");
            return -1;
        }
    }


    public List<string> GetFileExtension(List<string> extensions)
    {
        var extenstionsStr = extensions.Select((ext, index) => $"[{index + 1}]{ext}");
        Console.Write("Select one of these file types: ");
        Console.Write($"{string.Join(" ", extenstionsStr)}: ");
        var input = Console.ReadLine();
        var result = input.Split(",").ToList()
            .Select(indexStr => int.Parse(indexStr) - 1) // Convert to zero-based index
            .Where(index => index >= 0 && index < extensions.Count) // Check bounds
            .Select(index => extensions[index])
            .ToList();
        return result;
    }

    public string GetRootDirectory()
    {
        Console.Write("Pick the root path: ");
        var input = Console.ReadLine();
        return input;
    }

    public void DisplaySearchResults(List<string> foundFiles)
    {
        Console.WriteLine("Search Results:");
        if (!foundFiles.Any())
            Console.WriteLine("Not Find ..");
        foreach (string filePath in foundFiles)
        {
            Console.WriteLine(filePath);
        }
    }

    public void DisplayErrorMessage(string message)
    {
        Helpers.Error($"Error: {message}");
    }

    public string GetQuery()
    {
        Console.Write("Query: ");
        var input = Console.ReadLine();
        return input;
    }

    public void Warning(string warning)
    {
        if (!string.IsNullOrWhiteSpace(warning))
            Helpers.Warning(warning);
    }
    public void DisplaySearchHistory(List<SearchHistoryEntry> historyEntries)
    {
        Console.WriteLine("Search History:");
        if (historyEntries == null || historyEntries.Count == 0)
        {
            Console.WriteLine("No search history available.");
            return;
        }

        foreach (var entry in historyEntries)
        {
            Console.WriteLine($"Timestamp: {entry.Timestamp}, Query: {entry.SearchQuery}");
            Console.WriteLine("Results:");
            foreach (var result in entry.SearchResults)
            {
                Console.WriteLine($"- {result}");
            }
            Console.WriteLine(); // Adds a blank line for better readability
        }
        OnHistoryViewed();
    }
    public void DisplayPlugins(List<(IExtension Extension, string Name)> plugins)
    {
        Console.WriteLine("Loaded Plugins:");
        var pluginsGroupedByType = plugins
           .GroupBy(p => p.Extension.TypeName)
           .OrderBy(g => g.Key);

        foreach (var group in pluginsGroupedByType)
        {
            Console.WriteLine($"{group.Key}:");
            foreach (var plugin in group)
            {
                Console.WriteLine($"   - {plugin.Name}");
            }
        }
    }
    public IExtension ChoosePlugin(List<(IExtension extenstion, string name)> plugins)
    {
        Console.WriteLine("Multiple plugins can handle this file type. Please choose one:");
        int i = 0;
        foreach (var plugin in plugins)
        {
            Console.WriteLine($"{i + 1}. {plugin.name}");
            i++;
        }

        int choice = Convert.ToInt32(Console.ReadLine()) - 1;
        return plugins[choice].extenstion;
    }
    public void DisplayMessage(string message)
    {
        Console.WriteLine(message);
    }
    public void ShowLoading(CancellationToken token)
    {
        string loadingText = "Searching";
        int loadingStep = 0;

        while (!token.IsCancellationRequested)
        {
            Console.CursorLeft = 0;
            Console.Write(loadingText + new string('.', loadingStep) + new string(' ', 10)); // Overwrite the line
            Thread.Sleep(500); // Control the speed of the animation

            loadingStep = (loadingStep + 1) % 4; // Change the number of dots
        }
    }

    public void Stop()
    {
        Console.ReadKey();
    }
    public void Clear()
    {
        Console.Clear();
    }
    protected virtual void OnHistoryViewed()
    {
        HistoryViewed?.Invoke(this, EventArgs.Empty);
    }
}
