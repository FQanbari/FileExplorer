namespace FileExplorer.FileSearch;

public static class Logger
{
    // Method to log informational messages
    public static void LogInfo(string message)
    {
        Console.WriteLine($"INFO: {DateTime.Now} - {message}");
    }

    // Method to log warning messages
    public static void LogWarning(string message)
    {
        Console.WriteLine($"WARNING: {DateTime.Now} - {message}");
    }

    // Method to log error messages
    public static void LogError(string message, Exception ex = null)
    {
        Console.WriteLine($"ERROR: {DateTime.Now} - {message}");
        if (ex != null)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }

    // Additional methods for other log levels (e.g., Debug, Fatal) can be added as required

    // Method to log debug messages (optional, based on build configuration)
    public static void LogDebug(string message)
    {
#if DEBUG
        Console.WriteLine($"DEBUG: {DateTime.Now} - {message}");
#endif
    }
}
