namespace FileExplorer.FileSearch;

public class HistoryManager
{
    private readonly List<SearchHistory> searchHistories;
    private readonly string historyFilePath;

    public HistoryManager(string historyFilePath)
    {
        this.historyFilePath = historyFilePath;
        this.searchHistories = LoadHistory();
    }

    // Method to add a new search record to the history
    public void RecordSearch(SearchParameters searchParameters, List<string> searchResults)
    {
        var newRecord = new SearchHistory
        {
            SearchParameters = searchParameters,
            Results = searchResults,
            Timestamp = DateTime.Now
        };

        searchHistories.Add(newRecord);
        SaveHistory();
    }

    // Method to retrieve the search history
    public IEnumerable<SearchHistory> GetSearchHistory()
    {
        return searchHistories;
    }

    // Loads the search history from a file
    private List<SearchHistory> LoadHistory()
    {
        // Implement logic to load history from the file
        // This could involve deserialization from a file
        return searchHistories;
    }

    // Saves the current search history to a file
    private void SaveHistory()
    {
        // Implement logic to save history to the file
        // This could involve serialization to a file
    }
}
