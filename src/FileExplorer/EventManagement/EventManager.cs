namespace FileExplorer.EventManagement;

public class EventManager
{
}


// Subject interface
public interface IHistoryObservable
{
    event EventHandler<HistoryEventArgs> HistoryPerformed;
    void PerformSearch(string searchQuery);
}

// Concrete subject
public class HistoryObservable : IHistoryObservable
{
    public event EventHandler<HistoryEventArgs> HistoryPerformed;

    public void PerformSearch(string searchQuery)
    {
        // Perform the search operation...

        // Notify subscribers that a search has been performed
        OnSearchPerformed(searchQuery);
    }

    protected virtual void OnSearchPerformed(string searchQuery)
    {
        HistoryPerformed?.Invoke(this, new HistoryEventArgs(searchQuery));
    }
}

// Observer interface
public interface IHistoryObserver
{
    void Update(object sender, HistoryEventArgs e);
}

// Concrete observer
public class HistoryManager : IHistoryObserver
{
    private List<string> searchHistory = new List<string>();

    public void SubscribeToSearchObservable(IHistoryObservable searchObservable)
    {
        searchObservable.HistoryPerformed += Update;
    }

    public void Update(object sender, HistoryEventArgs e)
    {
        // Save the search history
        searchHistory.Add(e.SearchQuery);
        Console.WriteLine($"Search performed: {e.SearchQuery}");
    }

    public void DisplaySearchHistory()
    {
        Console.WriteLine("Search History:");
        foreach (var query in searchHistory)
        {
            Console.WriteLine(query);
        }
    }
}

public class HistoryEventArgs : EventArgs
{
    public string SearchQuery { get; }

    public HistoryEventArgs(string searchQuery)
    {
        SearchQuery = searchQuery;
    }
}
