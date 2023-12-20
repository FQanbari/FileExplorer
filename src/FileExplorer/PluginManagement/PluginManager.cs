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

    public void _LoadPlugins(string pluginDirectory)
    {
        try
        {
            // Check if the pluginManager directory exists
            if (Directory.Exists(pluginDirectory))
            {
                // Load all DLL files in the pluginManager directory
                string[] pluginFiles = Directory.GetFiles(pluginDirectory, "*.dll");

                foreach (string pluginFile in pluginFiles)
                {
                    // Load the assembly
                    Assembly assembly = Assembly.LoadFrom(pluginFile);

                    // Get types implementing IFileSearchPlugin interface
                    var pluginTypes = assembly.GetTypes()
                        .Where(type => typeof(IFileTypePlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

                    foreach (var pluginType in pluginTypes)
                    {
                        // Create an instance of the pluginManager
                        IFileTypePlugin plugin = Activator.CreateInstance(pluginType) as IFileTypePlugin;

                        if (plugin != null)
                        {
                            plugins.Add(plugin);
                            Console.WriteLine($"Plugin loaded: {plugin.TypeName}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Plugin directory not found.");
            }
        }
        catch (Exception ex)
        {
            _pluginsUnloaded++;
            Console.WriteLine($"An error occurred while loading plugins: {ex.Message}");

        }
    }
    public List<IFileTypePlugin> LoadPlugins(string pluginDirectory)
    {
        List<IFileTypePlugin> loadedPlugins = new List<IFileTypePlugin>();

        try
        {
            // Check if the pluginManager directory exists
            if (Directory.Exists(pluginDirectory))
            {
                // Load all DLL files in the pluginManager directory
                string[] pluginFiles = Directory.GetFiles(pluginDirectory, "*.dll");

                foreach (string pluginFile in pluginFiles)
                {
                    // Load the assembly
                    Assembly assembly = Assembly.LoadFrom(pluginFile);

                    // Get types implementing IFileSearchPlugin interface
                    var pluginTypes = assembly.GetTypes()
                        .Where(type => typeof(IFileTypePlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

                    foreach (var pluginType in pluginTypes)
                    {
                        // Create an instance of the pluginManager
                        IFileTypePlugin plugin = Activator.CreateInstance(pluginType) as IFileTypePlugin;

                        if (plugin != null)
                        {
                            loadedPlugins.Add(plugin);
                            Console.WriteLine($"Plugin loaded: {plugin.TypeName}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Plugin directory not found.");
            }
        }
        catch (Exception ex)
        {
            _pluginsUnloaded++;
            Console.WriteLine($"An error occurred while loading plugins: {ex.Message}");
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
        {
            Console.WriteLine($"Executing pluginManager: {plugin.TypeName}");
            result.AddRange(plugin.Execute(rootDirectory, searchQuery));
        }

        return result;
    }
}
