using FileExplorer.FileSearch;
using FileExplorer.PluginInterface;
using System.Security.Permissions;

namespace TxtFileHandler;

public class TextFileTypeHandler : IFileTypePlugin
{
    public string TypeName => "TXT";

    public List<string> Execute(string directory, string searchQuery)
    {
        var result = new List<string>();
        string[] files = Directory.GetFiles(directory);

        result = GetTextFilesWithPattern(directory, searchQuery);


        return result;
    }
    static List<string> GetTextFilesWithPattern(string directoryPath, string pattern)
    {
        List<string> filteredFiles = new List<string>();

        try
        {
            // Get all text files in the current directory containing the specified pattern in their names
            var currentDirectoryFiles = Directory.GetFiles(directoryPath, $"*{pattern}*")
                .Where(file => file.ToLower().EndsWith(".txt"));

            // Add the current directory's files to the result list
            filteredFiles.AddRange(currentDirectoryFiles);

            // Recursively search subdirectories
            var subdirectories = Directory.GetDirectories(directoryPath);
            foreach (var subdirectory in subdirectories)
            {
                // Recursively call the method for each subdirectory
                filteredFiles.AddRange(GetTextFilesWithPattern(subdirectory, pattern));
            }
        }
        catch (Exception ex)
        {
        }

        return filteredFiles;
    }
}