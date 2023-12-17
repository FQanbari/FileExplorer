namespace FileExplorer.Utilities;

public static class Helpers
{
    public static void PrintDividerLine()
    {
        Console.WriteLine(new string('-', Console.WindowWidth));
    }

    public static bool ConfirmAction(string message)
    {
        Console.Write($"{message} (y/n): ");
        string response = Console.ReadLine().ToLower();
        return response == "y" || response == "yes";
    }
}