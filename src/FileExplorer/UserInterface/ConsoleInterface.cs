namespace FileExplorer.UserInterface;

public class ConsoleInterface
{
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

    
    public string GetFileExtension(List<string> extensions)
    {
        Console.Write("Select one of these file types: ");
        for (int i = 0; i < extensions.Count; i++)
            Console.Write($"[{i + 1}]{extensions[i]} ");
        Console.Write(": ");
        return Console.ReadLine();
    }

    public string GetRootDirectory()
    {
        Console.Write("Pick the root path: ");
        return Console.ReadLine();
    }

    public void DisplaySearchResults(List<string> foundFiles)
    {
        Console.WriteLine("Search Results:");
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
        return Console.ReadLine();
    }
}
