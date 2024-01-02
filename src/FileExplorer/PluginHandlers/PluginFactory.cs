using FileExplorer.ExtensionPlatfrom;

namespace FileExplorer.PluginHandlers;

public class PluginFactory
{
    private readonly List<IExtension> plugins;

    public PluginFactory(List<IExtension> plugins)
    {
        this.plugins = plugins ?? throw new ArgumentNullException(nameof(plugins));
    }

    public List<string> ExecutePlugins(string rootDirectory, string searchQuery)
    {
        var result = new List<string>();
        foreach (var plugin in plugins)
            result.AddRange(plugin.Execute(rootDirectory, searchQuery));

        return result;
    }
}
