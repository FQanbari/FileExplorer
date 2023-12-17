namespace FileExplorer.FileSearch;

public class TextFileTypeHandler : IFileTypeHandler
{
    // Implementation of the search method for text files
    public List<string> Search(string directory, string query, bool useParallelProcessing)
    {
        var result = new List<string>();

        // Retrieve all text files from the directory
        var files = Directory.EnumerateFiles(directory, "*.txt", SearchOption.AllDirectories);

        if (useParallelProcessing)
        {
            // Parallel search logic
            result = files.AsParallel()
                          .Where(file => FileContainsText(file, query))
                          .ToList();
        }
        else
        {
            // Sequential search logic
            foreach (var file in files)
            {
                if (FileContainsText(file, query))
                {
                    result.Add(file);
                }
            }
        }

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
