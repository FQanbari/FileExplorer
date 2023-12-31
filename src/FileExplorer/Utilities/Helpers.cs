using System.Drawing;

namespace FileExplorer.Utilities;

public static class Helpers
{
    public static void PrintDividerLine()
    {
        Console.WriteLine();
        Console.WriteLine(new string('-', Console.WindowWidth));
        Console.WriteLine();
    }

    public static bool ConfirmAction(string message)
    {
        Console.Write($"{message} (y/n): ");
        string response = Console.ReadLine().ToLower();
        return response == "y" || response == "yes";
    }
    public static void DisplayMessage(string message)
    {
        Console.WriteLine(message);
    }
    public static void Info(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        DisplayMessage(message);
        Console.ResetColor();
    }
    public static void Warning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        DisplayMessage(message);
        Console.ResetColor();
    }
    public static void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        DisplayMessage(message);
        Console.ResetColor();
    }
    public static void Enter()
    {
        Console.WriteLine();
    }
}