using FileExplorer.Configuration;
using FileExplorer.ExtensionPlatfrom;
using System.Reflection;

namespace FileExplorer.PluginHandlers;

public class PluginManager : IPluginManager
{
    public delegate void PluginLoadHandler(string pluginName, bool success);
    public event PluginLoadHandler PluginLoaded;
    private List<(IExtension extension, string name)> _plugins = new List<(IExtension, string)>();
    private readonly List<(string FileName, string Reason)> _unloadedPlugins = new List<(string, string)>();
    private readonly AppConfig _appConfig;

    public PluginManager(AppConfig appConfig)
    {
        _plugins = new List<(IExtension, string)>();
        _appConfig = appConfig;
    }

    public void LoadPlugins(string pluginDirectory, AppConfig appConfig)
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

                        if (pluginTypes.Any() == false)
                        {
                            _unloadedPlugins.Add((Path.GetFileName(pluginFile), "No types implementing IExtension found"));
                        }
                        foreach (var pluginType in pluginTypes)
                        {
                            pluginName = pluginType.FullName;
                            IExtension plugin = Activator.CreateInstance(pluginType) as IExtension;

                            if (plugin != null && plugin.CanHandleFileExtension(plugin.TypeName))
                            {
                                if (_appConfig.PluginSearchThresholds.TryGetValue(pluginType.Name, out int threshold))
                                {
                                    plugin.SearchThreshold = threshold;
                                }
                                else
                                {
                                    plugin.SearchThreshold = _appConfig.DefaultSearchThreshold;
                                }
                                _plugins.Add((plugin, pluginName)); 
                            }
                            else
                                _unloadedPlugins.Add((Path.GetFileName(pluginFile), "No types implementing IExtension found")); ;

                        }

                        OnPluginLoaded(pluginName, true); // Assuming you extract pluginName somehow
                    }
                    catch (Exception ex)
                    {
                        _unloadedPlugins.Add((Path.GetFileName(pluginFile), ex.Message));
                        OnPluginLoaded(pluginFile, false);
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(pluginDirectory);
            }

        }
        catch (Exception ex)
        {

        }

    }

    public string GetWarning()
    {
        return _unloadedPlugins.Count > 0 ?
        $"NOTE: There was a problem with loading {_unloadedPlugins.Count} extensions. View them in Manage Extenstions section.\n"
        : "";

    }
    public List<(IExtension Extension, string Name)> ListPlugins()
    {
        return _plugins;
    }
    public List<(string FileName, string Reason)> GetUnloadedPlugins()
    {
        return _unloadedPlugins;
    }
    public List<(IExtension Extension, string Name)> GetPluginsForExtension(string fileExtension)
    {
        return _plugins.Where(plugin => plugin.extension.CanHandleFileExtension(fileExtension)).ToList();
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
