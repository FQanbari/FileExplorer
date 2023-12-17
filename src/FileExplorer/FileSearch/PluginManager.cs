using System.Reflection;

namespace FileExplorer.FileSearch;

public class PluginManager
{
    private readonly Dictionary<string, IFileTypeHandler> fileTypeHandlers;

    public PluginManager(Dictionary<string, IFileTypeHandler> fileTypeHandlers)
    {
        this.fileTypeHandlers = fileTypeHandlers;
    }

    // Method to load plugins from a specified directory
    public void LoadPlugins(string pluginsDirectory)
    {
        if (!Directory.Exists(pluginsDirectory))
        {
            throw new DirectoryNotFoundException("Plugins directory not found.");
        }

        // Load plugin assemblies
        var pluginFiles = Directory.GetFiles(pluginsDirectory, "*.dll");
        foreach (var file in pluginFiles)
        {
            try
            {
                var assembly = Assembly.LoadFrom(file);
                RegisterPluginsFromAssembly(assembly);
            }
            catch (Exception ex)
            {
                // Handle exceptions related to plugin loading
            }
        }
    }

    // Method to register plugins from an assembly
    private void RegisterPluginsFromAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsClass && !type.IsAbstract && typeof(IFileTypeHandler).IsAssignableFrom(type))
            {
                var plugin = (IFileTypeHandler)Activator.CreateInstance(type);
                fileTypeHandlers[type.Name] = plugin;
            }
        }
    }

    // Additional methods or properties as required, such as unloading plugins, etc.
}
