namespace FileExplorer.HistoryManagement;

public class HistoryManager
{
    private readonly string _filePath;
    private List<string> searchHistory = new List<string>();

    public HistoryManager(string filePath)
    {
        this._filePath = filePath;
    }
    public List<string> LoadSearchHistory()
    {
        try
        {
            // Load search history from the file
            searchHistory = File.ReadAllLines(_filePath).ToList();
        }
        catch (Exception ex)
        {
        }

        return searchHistory;
    }
}
