using FileExplorer.Configuration;
using FileExplorer.ExtensionPlatfrom;

namespace FileExplorer.PluginHandlers;

public interface IPluginManager
{
    event PluginManager.PluginLoadHandler PluginLoaded;

    List<IExtension> GetPluginsByExtensionsInput(List<string> fileExtensions);
    List<(IExtension Extension, string Name)> GetPluginsForExtension(string fileExtension);
    List<string> GetTypeAllPlugins();
    string GetWarning();
    List<(IExtension Extension, string Name)> ListPlugins();
    List<(string FileName, string Reason)> GetUnloadedPlugins();
    void LoadPlugins(string pluginDirectory, AppConfig appConfig);
}