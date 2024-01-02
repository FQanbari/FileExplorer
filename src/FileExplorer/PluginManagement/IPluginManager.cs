using FileExplorer.ExtensionPlatfrom;

namespace FileExplorer.PluginManagement
{
    public interface IPluginManager
    {
        event PluginManager.PluginLoadHandler PluginLoaded;

        List<IExtension> GetPluginsByExtensionsInput(List<string> fileExtensions);
        List<(IExtension Extension, string Name)> GetPluginsForExtension(string fileExtension);
        List<string> GetTypeAllPlugins();
        string GetWarning();
        List<(IExtension Extension, string Name)> ListPlugins();
        void LoadPlugins(string pluginDirectory);
    }
}