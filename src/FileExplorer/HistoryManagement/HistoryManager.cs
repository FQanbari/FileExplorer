using Newtonsoft.Json;

namespace FileExplorer.HistoryManagement;

public class HistoryManager
{
    private readonly string _filePath;
    private List<string> searchHistory = new List<string>();

    public HistoryManager(string filePath)
    {
        this._filePath = filePath;
    }
    public List<SearchHistoryEntry> LoadSearchHistory()
    {
        try
        {
            // Load search history from the file
            searchHistory = File.ReadAllLines(_filePath).ToList();
            if (File.Exists(_filePath))
            {
                string json = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<List<SearchHistoryEntry>>(json) ?? new List<SearchHistoryEntry>();
            }
            else
            {
                return new List<SearchHistoryEntry>();
            }
        }
        catch (Exception ex)
        {
        }

        return new List<SearchHistoryEntry>();
    }
}
public class SearchHistoryEntry
{
    public DateTime Timestamp { get; set; }
    public List<string> Query { get; set; }
}
