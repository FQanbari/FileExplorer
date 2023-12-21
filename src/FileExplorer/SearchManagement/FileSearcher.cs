using FileExplorer.PluginInterface;
using FileExplorer.PluginManagement;

namespace FileExplorer.SearchManagement;

public class FileSearcher
{
    public List<string> SearchFiles(string rootDirectory, List<IFileTypePlugin> plugins, string searchQuery)
    {
        var result = new List<string>();
        try
        {
            PluginFactory pluginFactory = new PluginFactory(plugins);
            result = pluginFactory.ExecutePlugins(rootDirectory, searchQuery);
        }
        catch (Exception ex)
        {

        }
        return result;
    }
}
