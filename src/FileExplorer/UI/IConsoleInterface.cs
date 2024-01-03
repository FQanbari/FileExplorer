using FileExplorer.ExtensionPlatfrom;
using FileExplorer.FileHandling;

namespace FileExplorer.UI;

public interface IConsoleInterface
{
    event EventHandler HistoryViewed;

    IExtension ChoosePlugin(List<(IExtension extension, string name, bool IsEnabled)> plugins);
    void Clear();
    void DisplayErrorMessage(string message);
    void DisplayMainMenu();
    void DisplayMessage(string message);
    void DisplayPlugins(List<(IExtension Extension, string Name, bool IsEnabled)> plugins);
    void DisplayPlugins(List<(IExtension Extension, string Name, bool IsEnabled)> loadedPlugins, List<(string FileName, string Reason)> unloadedPlugins);
    string ChoosePluginToToggle(List<(IExtension Extension, string Name, bool IsEnabled)> plugins);
    void DisplaySearchHistory(List<SearchHistoryEntry> historyEntries);
    void DisplaySearchResults(List<string> foundFiles);
    List<string> GetFileExtension(List<string> extensions, string defaultExtension);
    string GetQuery();
    string GetRootDirectory();
    int Option();
    void ShowLoading(CancellationToken token);
    void Stop();
    void Warning(string warning);
}