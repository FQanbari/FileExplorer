using FileExplorer.Configuration;
using FileExplorer.ExtensionPlatfrom;

namespace FileExplorer.PluginHandlers;

public interface IPluginManager
{
    event PluginManager.PluginLoadHandler PluginLoaded;

    List<IExtension> GetPluginsByExtensionsInput(List<string> fileExtensions);
    List<(IExtension Extension, string Name, bool IsEnabled)> GetPluginsForExtension(string fileExtension);
    List<string> GetTypeAllPlugins();
    string GetWarning();
    public List<(IExtension Extension, string Name, bool IsEnabled)> ListPlugins();
    List<(string FileName, string Reason)> GetUnloadedPlugins();
    void LoadPlugins(string pluginDirectory, AppConfig appConfig);
    void EnablePlugin(string pluginName);
    void DisablePlugin(string pluginName);
}