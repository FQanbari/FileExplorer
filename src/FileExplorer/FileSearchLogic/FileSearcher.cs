using System;
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

    // Additional methods for searching by file name, content, etc. can be added here.
}
