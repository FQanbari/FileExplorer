using FileExplorer.FileSearch;
using FileExplorer.PluginInterface;
using System.Security.Permissions;

namespace TxtFileHandler;

public class TextFileTypeHandler : IFileTypePlugin
{
    public string TypeName => "TXT";

    public List<string> _Execute(string rootDirectory, string searchQuery)
    {
        var result = new List<string>();
        try
        {
            // Check if the root directory exists
            if (Directory.Exists(rootDirectory))
            {
                // Search for files with the specified extension and containing the search query in all subdirectories
                string[] files = Directory.GetFiles(rootDirectory, $"*.{TypeName.ToLower()}", SearchOption.AllDirectories)
    .Where(file =>
    {
        try
        {
            // Check if the file name contains the search query
            string fileName = Path.GetFileName(file);
            if (fileName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
            {
                return true; // Include the file in the results
            }

            // Check if the file content contains the search query
            string fileContent = File.ReadAllText(file);
            return fileContent.Contains(searchQuery, StringComparison.OrdinalIgnoreCase);
        }
        catch (UnauthorizedAccessException)
        {
            // Handle the case where access is denied (e.g., skip the file)
            return false;
        }
    })
    .ToArray();


                result = files.ToList();
                if (files.Length > 0)
                {
                    Console.WriteLine($"Found {files.Length} files with the extension '.{TypeName}' containing the search query:");
                    foreach (string file in files)
                    {
                        Console.WriteLine(file);
                    }
                }
                else
                {
                    Console.WriteLine($"No files with the extension '.{TypeName}' containing the search query found in the specified directory.");
                }
            }
            else
            {
                Console.WriteLine("Invalid directory path. Please make sure the directory exists.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return result;
    }
    public List<string> Execute(string directory, string searchQuery)
    {
        var result = new List<string>();

        // Retrieve all text files from the directory
        //var files = Directory.EnumerateFiles(directory, "*.txt", SearchOption.AllDirectories);

        //if (true)
        //{
        //    // Parallel search logic
        //    result = files.AsParallel()
        //                  .Where(file => FileContainsText(file, query))
        //                  .ToList();
        //}
        //else
        //{
        //    // Sequential search logic
        //    foreach (var file in files)
        //    {
        //        if (FileContainsText(file, query))
        //        {
        //            result.Add(file);
        //        }
        //    }
        //}
        string[] files = Directory.GetFiles(directory, "*.txt", SearchOption.AllDirectories)
    .Where(file =>
    {
        try
        {
            // Check if the application has access to the directory
            new FileIOPermission(FileIOPermissionAccess.Read, file).Demand();

            // Check if the file name contains the search query
            string fileName = Path.GetFileName(file);
            if (fileName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
            {
                return true; // Include the file in the results
            }

            // Check if the file content contains the search query
            string fileContent = File.ReadAllText(file);
            return fileContent.Contains(searchQuery, StringComparison.OrdinalIgnoreCase);
        }
        catch (UnauthorizedAccessException)
        {
            // Handle the case where access is denied (e.g., skip the file)
            return false;
        }
    })
    .ToArray();


        return result;
    }

    // Helper method to check if a file contains the specified text
    private bool FileContainsText(string filePath, string searchText)
    {
        // Read file content and check for the query text
        // Implement exception handling as needed
        string content = File.ReadAllText(filePath);
        return content.Contains(searchText, StringComparison.OrdinalIgnoreCase);
    }

}