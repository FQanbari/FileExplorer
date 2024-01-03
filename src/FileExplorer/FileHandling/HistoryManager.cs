using Newtonsoft.Json;

namespace FileExplorer.FileHandling;

public class HistoryManager
{
    private readonly string _filePath;
    private List<string> _searchHistory ;

    public HistoryManager(string filePath)
    {
        _filePath = filePath;
        _searchHistory = new List<string>();
    }
    public List<SearchHistoryEntry> LoadSearchHistory()
    {
        try
        {
            _searchHistory = File.ReadAllLines(_filePath).ToList();
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
