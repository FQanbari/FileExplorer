
using FileExplorer.ExtensionPlatfrom;
using Microsoft.VisualBasic.FileIO;
using System.Security.Permissions;

namespace TxtFileHandler;

[FileExplorerExtenstion("TXT")]
public class TextFileTypeHandler : IExtension
{
    public string TypeName => "TXT";
    public int SearchThreshold { get; set; }
    public List<string> Execute(string rootDirectory, string searchQuery)
    {
        var result = new List<string>();

        SearchFiles(rootDirectory, searchQuery, result);

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

}


[FileExplorerExtenstion("TXT")]
public class TextFileTypeCGHandler : IExtension
{
    public string TypeName => "TXT";
    public int SearchThreshold { get; set; }

    public List<string> Execute(string rootDirectory, string searchQuery)
    {
        var result = new List<string>();

        SearchFiles(rootDirectory, searchQuery, result);

        return result;
    }

    void SearchFiles(string directory, string searchQuery, List<string> result)
    {
        try
        {
            string[] files = Directory.GetFiles(directory, "*.txt");

            if (files.Length > SearchThreshold)
            {
                var tasks = new List<Task>();
                for (int i = 0; i < files.Length; i += SearchThreshold)
                {
                    var batchFiles = files.Skip(i).Take(SearchThreshold).ToArray();
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


}
