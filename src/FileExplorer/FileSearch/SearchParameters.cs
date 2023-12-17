namespace FileExplorer.FileSearch;

public class SearchParameters
{
    // The root directory where the search should start
    public string RootDirectory { get; private set; }

    // The file type to search for (e.g., "txt", "pdf")
    public string FileType { get; private set; }

    // The search query or pattern
    public string Query { get; private set; }

    // Constructor to initialize the SearchParameters object
    public SearchParameters(string rootDirectory, string fileType, string query)
    {
        if (!InputValidator.IsValidDirectory(rootDirectory))
        {
            throw new ArgumentException("Invalid directory path provided.");
        }

        if (!InputValidator.IsValidFileType(fileType))
        {
            throw new ArgumentException("Unsupported file type provided.");
        }

        RootDirectory = rootDirectory;
        FileType = fileType;
        Query = query;
    }

    // Additional methods or properties as required
}
