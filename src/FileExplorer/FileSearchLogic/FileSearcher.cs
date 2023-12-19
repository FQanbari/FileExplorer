using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.FileSearchLogic;

public class FileSearcher
{
    // Search for files with a specific extension in a directory and its subdirectories.
    public List<string> SearchFilesByExtension(string rootDirectory, string fileExtension)
    {
        List<string> foundFiles = new List<string>();

        try
        {
            // Recursively search for files with the specified extension.
            foundFiles = Directory.GetFiles(rootDirectory, $"*.{fileExtension}", SearchOption.AllDirectories).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return foundFiles;
    }

    public ConcurrentBag<string> SearchFiles(string rootPath, string query)
    {
        var foundFiles = new ConcurrentBag<string>();

        try
        {
            var allTxtFiles = Directory.EnumerateFiles(rootPath, "*.txt", SearchOption.AllDirectories);
            Parallel.ForEach(allTxtFiles, (file) =>
            {
                if (file.Contains(query) || File.ReadAllText(file).Contains(query))
                {
                    foundFiles.Add(file);
                }
            });
        }
        catch (Exception ex)
        {
            // Log the exception using Logger from Utilities
            // Logger.Log(ex.Message);
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return foundFiles;
    }
}
