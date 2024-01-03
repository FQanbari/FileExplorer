﻿using FileExplorer.ExtensionPlatfrom;
using FileExplorer.FileHandling;

namespace FileExplorer.UI;

public interface IConsoleInterface
{
    event EventHandler HistoryViewed;

    IExtension ChoosePlugin(List<(IExtension extension, string name)> plugins);
    void Clear();
    void DisplayErrorMessage(string message);
    void DisplayMainMenu();
    void DisplayMessage(string message);
    void DisplayPlugins(List<(IExtension Extension, string Name)> plugins);
    void DisplayPlugins(List<(IExtension Extension, string Name)> loadedPlugins, List<(string FileName, string Reason)> unloadedPlugins);
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