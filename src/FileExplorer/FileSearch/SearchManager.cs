namespace FileExplorer.FileSearch;

public class SearchManager
{
    private Dictionary<string, IFileTypeHandler> fileTypeHandlers;

    public SearchManager()
    {
        this.fileTypeHandlers = new Dictionary<string, IFileTypeHandler>();
        // Initialize with default file type handler
        this.fileTypeHandlers.Add("txt", new TextFileTypeHandler());
        // Additional handlers can be added either here or through a method
    }

    // Method to add new file type handlers (used by PluginManager)
    public void AddFileTypeHandler(string fileType, IFileTypeHandler handler)
    {
        fileTypeHandlers[fileType] = handler;
    }

    // Main method to handle search
    public List<string> Search(string rootDirectory, string fileType, string query)
    {
        if (!InputValidator.IsValidDirectory(rootDirectory))
        {
            throw new ArgumentException("Invalid directory path.");
        }

        if (!fileTypeHandlers.ContainsKey(fileType))
        {
            throw new NotSupportedException($"File type '{fileType}' is not supported.");
        }

        var handler = fileTypeHandlers[fileType];

        // Decide whether to use parallel processing based on the number of subdirectories
        bool useParallel = ShouldUseParallelProcessing(rootDirectory);
        return handler.Search(rootDirectory, query, useParallel);
    }

    // Method to determine if parallel processing should be used
    private bool ShouldUseParallelProcessing(string rootDirectory)
    {
        // Implement logic to determine based on the number of subdirectories, etc.
        // For example, use parallel processing if more than a certain number of subdirectories are found
        return true;
    }
}
