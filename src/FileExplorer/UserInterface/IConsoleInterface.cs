using FileExplorer.ExtensionPlatfrom;
using FileExplorer.SearchManagement;

namespace FileExplorer.UserInterface
{
    public interface IConsoleInterface
    {
        event EventHandler HistoryViewed;

        IExtension ChoosePlugin(List<(IExtension extenstion, string name)> plugins);
        void Clear();
        void DisplayErrorMessage(string message);
        void DisplayMainMenu();
        void DisplayMessage(string message);
        void DisplayPlugins(List<(IExtension Extension, string Name)> plugins);
        void DisplaySearchHistory(List<SearchHistoryEntry> historyEntries);
        void DisplaySearchResults(List<string> foundFiles);
        List<string> GetFileExtension(List<string> extensions);
        string GetQuery();
        string GetRootDirectory();
        int Option();
        void ShowLoading(CancellationToken token);
        void Stop();
        void Warning(string warning);
    }
}