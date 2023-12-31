using FileExplorer.ExtensionPlatfrom;
using FileExplorer.PluginManagement;
using Newtonsoft.Json;

namespace FileExplorer.SearchManagement;

public class FileSearcher
{
    public delegate void SearchCompletedHandler(string query, List<string> results);
    public event SearchCompletedHandler SearchCompleted;
    private List<SearchHistoryEntry> _searchHistory = new List<SearchHistoryEntry>();
    private string _historyFilePath;


    public FileSearcher()
    {
        _historyFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "searchHistory.json");
        _searchHistory = LoadSearchHistory();
    }
    public List<string> SearchFiles(string rootDirectory, List<IExtension> plugins, string searchQuery)
    {
        var result = new List<string>();
        try
        {
            PluginFactory pluginFactory = new PluginFactory(plugins);
            result = pluginFactory.ExecutePlugins(rootDirectory, searchQuery);
            OnSearchCompleted(searchQuery, result);
        }
        catch (Exception ex)
        {

        }
        return result;
    }
    public List<SearchHistoryEntry> GetSearchHistory()
    {
        return _searchHistory;
    }
    public void LogSearch(string query, List<string> results)
    {
        var entry = new SearchHistoryEntry
        {
            Timestamp = DateTime.Now,
            SearchQuery = query,
            SearchResults = results
        };
        _searchHistory.Add(entry);
        SaveSearchHistory();
    }

    private void SaveSearchHistory()
    {
        string json = JsonConvert.SerializeObject(_searchHistory, Formatting.Indented);
        File.WriteAllText(_historyFilePath, json);
    }
    
    private List<SearchHistoryEntry> LoadSearchHistory()
    {
        if (File.Exists(_historyFilePath))
        {
            string json = File.ReadAllText(_historyFilePath);
            return JsonConvert.DeserializeObject<List<SearchHistoryEntry>>(json) ?? new List<SearchHistoryEntry>();
        }
        return new List<SearchHistoryEntry>();
    }
    protected virtual void OnSearchCompleted(string query, List<string> results)
    {
        SearchCompleted?.Invoke(query, results);
    }
}
