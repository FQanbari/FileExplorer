using FileExplorer.PluginInterface;
using System.Reflection;

namespace FileExplorer.PluginManagement;

public class PluginManager
{
    private List<IFileTypePlugin> plugins;
    private int _pluginsUnloaded;

    public PluginManager()
    {
        plugins = new List<IFileTypePlugin>();
    }

    public List<IFileTypePlugin> LoadPlugins(string pluginDirectory)
    {
        List<IFileTypePlugin> loadedPlugins = new List<IFileTypePlugin>();

        try
        {
            if (Directory.Exists(pluginDirectory))
            {
                string[] pluginFiles = Directory.GetFiles(pluginDirectory, "*.dll");

                foreach (string pluginFile in pluginFiles)
                {
                    Assembly assembly = Assembly.LoadFrom(pluginFile);

                    var pluginTypes = assembly.GetTypes()
                        .Where(type => typeof(IFileTypePlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

                    foreach (var pluginType in pluginTypes)
                    {
                        IFileTypePlugin plugin = Activator.CreateInstance(pluginType) as IFileTypePlugin;

                        if (plugin != null)
                            loadedPlugins.Add(plugin);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _pluginsUnloaded++;
        }

        return loadedPlugins;
    }

    public void Warning()
    {
        if (_pluginsUnloaded > 0)
            Console.WriteLine($"NOTE: There was a proble with loading {_pluginsUnloaded} extensions. View them in Manage Extenstions section.\n");
    }
}

public class PluginFactory
{
    private readonly List<IFileTypePlugin> plugins;

    public PluginFactory(List<IFileTypePlugin> plugins)
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
