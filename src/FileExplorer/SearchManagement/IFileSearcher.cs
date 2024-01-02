using FileExplorer.ExtensionPlatfrom;

namespace FileExplorer.SearchManagement
{
    public interface IFileSearcher
    {
        event FileSearcher.SearchCompletedHandler SearchCompleted;

        List<SearchHistoryEntry> GetSearchHistory();
        void LogSearch(string query, List<string> results);
        List<string> SearchFiles(string rootDirectory, List<IExtension> plugins, string searchQuery);
    }
}