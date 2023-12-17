namespace FileExplorer.FileSearch;

public interface IFileTypeHandler
{
    // Method to perform the search operation within a given directory for a specific query.
    // 'useParallelProcessing' indicates whether to use parallel processing in the search.
    List<string> Search(string directory, string query, bool useParallelProcessing);

    // Additional methods or properties relevant to handling file types can be declared here.
}
