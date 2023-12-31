using FileExplorer.ExtensionPlatfrom;
using System.Reflection;

namespace FileExplorer.PluginManagement;

public class PluginManager
{
    public delegate void PluginLoadHandler(string pluginName, bool success);
    public event PluginLoadHandler PluginLoaded;
    private List<(IExtension extension, string name)> _plugins = new List<(IExtension, string)>();
    private int _pluginsUnloaded;

    public PluginManager()
    {
        _plugins = new List<(IExtension, string)>();
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
                    try
                    {
                        string pluginName = "";
                        Assembly assembly = Assembly.LoadFrom(pluginFile);

                        var pluginTypes = assembly.GetTypes()
                            .Where(type => typeof(IExtension).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

                        foreach (var pluginType in pluginTypes)
                        {
                            pluginName = pluginType.FullName;
                            IExtension plugin = Activator.CreateInstance(pluginType) as IExtension;

                            if (plugin != null && plugin.CanHandleFileExtension(plugin.TypeName))
                                _plugins.Add((plugin, pluginName));
                            else
                                _pluginsUnloaded++;


                        }
                        OnPluginLoaded(pluginName, true); // Assuming you extract pluginName somehow
                    }
                    catch (Exception ex)
                    {
                        OnPluginLoaded(pluginFile, false);
                    }
                }
                _pluginsUnloaded = pluginFiles.Length - _plugins.Count;
            }

        }
        catch (Exception ex)
        {

        }

    }

    public string GetWarning()
    {
        return _pluginsUnloaded > 0 ?
        ($"NOTE: There was a problem with loading {_pluginsUnloaded} extensions. View them in Manage Extenstions section.\n")
        : "";

    }
    public List<(IExtension Extension, string Name)> ListPlugins()
    {
        return _plugins;
    }
    public List<(IExtension Extension, string Name)> GetPluginsForExtension(string fileExtension)
    {
        return  _plugins.Where(plugin => plugin.extension.CanHandleFileExtension(fileExtension)).ToList();
    }
    public List<string> GetTypeAllPlugins()
    {
        return _plugins.GroupBy(x => x.extension.TypeName).Select(x => x.Key).ToList();
    }
    public List<IExtension> GetPluginsByExtensionsInput(List<string> fileExtensions)
    {
        return _plugins.Where(plugin => fileExtensions
        .Any(extension => plugin.extension.TypeName.Contains(extension, StringComparison.OrdinalIgnoreCase))).Select(plugin => plugin.extension).ToList();
    }
    private void OnPluginLoaded(string pluginName, bool success)
    {
        PluginLoaded?.Invoke(pluginName, success);
    }
}
