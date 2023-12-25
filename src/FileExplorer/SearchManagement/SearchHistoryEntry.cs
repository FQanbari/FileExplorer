namespace FileExplorer.SearchManagement;

public class SearchHistoryEntry
{
    public DateTime Timestamp { get; set; }
    public string SearchQuery { get; set; }
    public List<string> SearchResults { get; set; }
}