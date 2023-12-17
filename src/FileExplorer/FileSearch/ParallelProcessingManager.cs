namespace FileExplorer.FileSearch;

public class ParallelProcessingManager
{
    // Method to execute a search operation in parallel
    public List<string> ExecuteParallelSearch(IEnumerable<string> files, string query, Func<string, string, bool> searchCriteria)
    {
        // Using Parallel LINQ (PLINQ) for parallel processing
        var searchResults = files.AsParallel()
                                 .Where(file => searchCriteria(file, query))
                                 .ToList();

        return searchResults;
    }

    // Additional methods or properties to manage parallel tasks, handle exceptions, etc.

    // Example: a method to determine the degree of parallelism based on system resources or other criteria
    public int GetDegreeOfParallelism()
    {
        // Logic to determine the optimal degree of parallelism
        // For example, based on the number of available CPU cores
        return Environment.ProcessorCount;
    }

    // Method to handle exceptions that might occur during parallel processing
    private void HandleParallelException(AggregateException aggEx)
    {
        foreach (var ex in aggEx.InnerExceptions)
        {
            // Handle exceptions - log them, inform the user, etc.
        }
    }

    // Additional utility methods for parallel processing can be added as needed
}
