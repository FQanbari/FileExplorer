

using FileExplorer.HistoryManagement;
using FileExplorer.SearchManagement;
using System.Linq;

namespace FileExplorer.UserInterface;

public class ConsoleInterface 
{
    public event EventHandler HistoryViewed;

    public void DisplayMainMenu()
    {
        Console.WriteLine("1. Search for files");
        Console.WriteLine("2. Manage Extensions");
        Console.WriteLine("3. View search history\n");
        Console.WriteLine("4. Exit\n");
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
        Console.Write($"{string.Join(" ",extenstionsStr)}: ");
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
        Console.WriteLine($"Error: {message}");
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
            Console.WriteLine(warning);
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
    public void DisplayMessage(string message)
    {
        Console.WriteLine(message);
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
