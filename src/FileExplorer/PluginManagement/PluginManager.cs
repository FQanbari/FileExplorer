using FileExplorer.PluginInterface;
using System.Reflection;

namespace FileExplorer.PluginManagement;

public class PluginManager 
{
    private List<IFileTypePlugin> _plugins;
    private int _pluginsUnloaded;

    public PluginManager()
    {
        _plugins = new List<IFileTypePlugin>();
    }

    public void LoadPlugins(string pluginDirectory)
    {
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

                        if (plugin != null && plugin.CanHandleFileExtension(plugin.TypeName))
                            _plugins.Add(plugin);
                        else
                            _pluginsUnloaded++;

                    }
                }
                _pluginsUnloaded = pluginFiles.Length - _plugins.Count;
            }
        }
        catch (Exception ex)
        {
            _pluginsUnloaded++;
        }
        
    }
    
    public string GetWarning()
    {
        return _pluginsUnloaded > 0 ?
        ($"NOTE: There was a proble with loading {_pluginsUnloaded} extensions. View them in Manage Extenstions section.\n")
        : "";

    }
    public List<string> GetPluginNames()
    {
        return _plugins.Select(x => x.TypeName).ToList();
    }
    public List<IFileTypePlugin> GetPluginsByExtensionsInput(List<string> fileExtensions)
    {
        return _plugins.Where(plugin => fileExtensions.Any(extension => plugin.TypeName.Contains(extension, StringComparison.OrdinalIgnoreCase))).ToList();
    }
}
