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
        Console.WriteLine("1. Search for files");
        Console.WriteLine("2. Manage Extensions");
        Console.WriteLine("3. View search history\n");
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


    public List<string> GetFileExtension(List<string> extensions, string defaultExtension = "txt")
    {
        var extensionsStr = extensions.Select((ext, index) => $"[{index + 1}]{ext}");
        Console.Write("Select one of these file types (press Enter for default): ");
        Console.Write($"{string.Join(" ", extensionsStr)} :");

        var input = Console.ReadLine();

        // Check if the input is empty and return the default extension
        if (string.IsNullOrWhiteSpace(input))
        {
            return new List<string> { defaultExtension };
        }

        var inputParts = input.Split(",");
        var validIndices = new List<int>();
        bool allInputsValid = true;

        foreach (var part in inputParts)
        {
            if (InputValidator.ValidateFileExtension(part, extensions, out int index))
            {
                validIndices.Add(index);
            }
            else
            {
                allInputsValid = false;
                break;
            }
        }

        if (allInputsValid)
        {
            return validIndices.Select(index => extensions[index]).ToList();
        }
        else
        {
            Helpers.Error("Invalid choice. Please enter valid numbers separated by commas like 1,2");
            return GetFileExtension(extensions, defaultExtension); // Recursively call the method again for valid input
        }
    }


    public string GetRootDirectory()
    {
        string input = "";
        string validatedDirectory = "";
        do
        {
            Console.Write("Pick the root path: ");
            input = Console.ReadLine();
        } while (!InputValidator.ValidateDirectory(input, out validatedDirectory));

        return validatedDirectory;
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
        string input = "";
        string validatedString = "";
        do
        {
            Console.Write("Query: ");
            input = Console.ReadLine();
        } while (!InputValidator.ValidateNonEmptyString(input, out validatedString));
        
        return validatedString;
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
    public void DisplayPlugins(List<(IExtension Extension, string Name)> loadedPlugins, List<(string FileName, string Reason)> unloadedPlugins)
    {
        Console.WriteLine("Loaded Plugins:");
        if (loadedPlugins.Any())
        {
            foreach (var plugin in loadedPlugins)
            {
                Console.WriteLine($"- {plugin.Name}");
            }
        }
        else
        {
            Console.WriteLine("No loaded plugins.");
        }

        Helpers.Error("\nUnloaded Plugins:");
        if (unloadedPlugins.Any())
        {
            foreach (var plugin in unloadedPlugins)
            {
                Helpers.Error($"- {plugin.FileName}: {plugin.Reason}");
            }
        }
        else
        {
            Console.WriteLine("No unloaded plugins.");
        }
    }
    public IExtension ChoosePlugin(List<(IExtension extension, string name)> plugins)
    {
        int choice = 0;
        if (plugins == null || plugins.Count == 0)
        {
            Helpers.Info("No plugins available.");
            return null; // Or handle this case as appropriate for your application
        }

        Console.WriteLine("Multiple plugins can handle this file type. Please choose one:");
        for (int i = 0; i < plugins.Count; i++)
        {
            Console.WriteLine($"[{i + 1}]. {plugins[i].name}");
        }

        while (true)
        {
            Console.Write("Enter your choice (number): ");
            string input = Console.ReadLine();

            if (InputValidator.ValidateInteger(input, out choice) && choice > 0 && choice <= plugins.Count)
            {
                return plugins[choice - 1].extension;
            }
            else
            {
                Helpers.Error("Invalid choice. Please enter a valid number from the list.");
            }
        }
        //choice = Convert.ToInt32(Console.ReadLine()) - 1;
        //return plugins[choice].extension;
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
