using FileExplorer.PluginInterface;
using Microsoft.VisualBasic.FileIO;
using System.Security.Permissions;

namespace TxtFileHandler;

[FileSearchPlugin("TXT")]
public class TextFileTypeHandler : IFileTypePlugin
{
    public string TypeName => "TXT";

    public List<string> Execute(string rootDirectory, string searchQuery)
    {
        var result = new List<string>();

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // Run the task with CancellationToken
        Task loadingTask = Task.Run(() => Loading(false), cancellationTokenSource.Token);
        SearchFiles(rootDirectory, searchQuery, result);
        cancellationTokenSource.Cancel();

        // Wait for the Loading task to complete (if needed)
        loadingTask.Wait(1);
        Loading(true);
        return result;
    }

    static void SearchFiles(string directory, string searchQuery, List<string> result)
    {
        try
        {
            string[] files = Directory.GetFiles(directory, "*.txt");

            if (files.Length > 3)
            {
                var tasks = new List<Task>();
                for (int i = 0; i < files.Length; i += 3)
                {
                    var batchFiles = files.Skip(i).Take(3).ToArray();
                    tasks.Add(Task.Run(() => SearchFilesInBatch(batchFiles, searchQuery, result)));
                }

                Task.WaitAll(tasks.ToArray());
            }
            else
            {
                foreach (var file in files)
                {
                    SearchFile(file, searchQuery, result);
                }
            }

            // Recursively search in subdirectories
            string[] subDirectories = Directory.GetDirectories(directory);
            foreach (var subDir in subDirectories)
            {
                SearchFiles(subDir, searchQuery, result);
            }
        }
        catch (Exception ex)
        {
            // Console.WriteLine($"Error processing directory {directory}: {ex.Message}");
        }
    }

    static void SearchFilesInBatch(string[] files, string searchQuery, List<string> result)
    {
        foreach (var file in files)
        {
            SearchFile(file, searchQuery, result);
        }
    }

    static async Task SearchFile(string filePath, string searchQuery, List<string> result)
    {
        try
        {
            if (await FileContainsContent(filePath, searchQuery))
            {
                result.Add(filePath);
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions if needed
            // Console.WriteLine($"Error processing file {filePath}: {ex.Message}");
        }
    }

    static async Task<bool> FileContainsContent(string filePath, string content)
    {
        try
        {
            if (filePath.Contains(content, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            string fileContent = await File.ReadAllTextAsync(filePath);
            return fileContent.Contains(content);
        }
        catch (Exception)
        {
            return false;
        }
    }

    static void Loading(bool isBreak)
    {
        int left = Console.CursorLeft; // Get the initial left position
        int top = Console.CursorTop;   // Get the initial top position

        Console.SetCursorPosition(left, top);
        Console.Write("Loading");

        // Use a loop to simulate loading
        for (int i = 0; i < 10000; i++)
        {
            if (isBreak)
            {
                Console.SetCursorPosition(left , top);
                Console.WriteLine("");
                break;
            }
            if (i % 2 == 0)
            {
                Console.SetCursorPosition(left + 8, top);
                Console.Write("|");
            }
            else
            {
                Console.SetCursorPosition(left + 8, top);
                Console.Write("-");
            }
            Thread.Sleep(50); // Adjust the sleep duration based on your needs
        }

    }
}

