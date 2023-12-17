namespace FileExplorer.FileSearch;

public class SearchHistory
{
    public SearchParameters SearchParameters { get; set; }
    public List<string> Results { get; set; }
    public DateTime Timestamp { get; set; }
}
