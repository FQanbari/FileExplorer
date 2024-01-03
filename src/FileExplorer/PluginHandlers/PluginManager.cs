using FileExplorer.Configuration;
using FileExplorer.ExtensionPlatfrom;
using System.Reflection;

namespace FileExplorer.PluginHandlers;

public class PluginManager : IPluginManager
{
    public delegate void PluginLoadHandler(string pluginName, bool success);
    public event PluginLoadHandler PluginLoaded;
    //private List<(IExtension extension, string name)> _plugins = new List<(IExtension, string)>();
    private List<(IExtension Extension, string Name, bool IsEnabled)> _plugins;
    private readonly List<(string FileName, string Reason)> _unloadedPlugins ;
    private readonly AppConfig _appConfig;

    public PluginManager(AppConfig appConfig)
    {
        _plugins = new List<(IExtension, string, bool IsEnabled)>();
        _unloadedPlugins = new List<(string, string)>();
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
                                _plugins.Add((plugin, pluginName,true)); 
                            }
                            else
                                _unloadedPlugins.Add((Path.GetFileName(pluginFile), "No types implementing IExtension found")); ;

                        }

                        OnPluginLoaded(pluginName, true); 
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
    public List<(IExtension Extension, string Name, bool IsEnabled)> ListPlugins()
    {
        return _plugins;
    }
    public void EnablePlugin(string pluginName)
    {
        var plugin = _plugins.FirstOrDefault(p => p.Name == pluginName);
        if (plugin.Extension != null)
        {
            _plugins.Remove(plugin);
            _plugins.Add((plugin.Extension, plugin.Name, true));
        }
    }

    public void DisablePlugin(string pluginName)
    {
        var plugin = _plugins.FirstOrDefault(p => p.Name == pluginName);
        if (plugin.Extension != null)
        {
            _plugins.Remove(plugin);
            _plugins.Add((plugin.Extension, plugin.Name, false));
        }
    }
    public List<(string FileName, string Reason)> GetUnloadedPlugins()
    {
        return _unloadedPlugins;
    }
    public List<(IExtension Extension, string Name, bool IsEnabled)> GetPluginsForExtension(string fileExtension)
    {
        return _plugins.Where(plugin => plugin.Extension.CanHandleFileExtension(fileExtension)).Where(plugin => plugin.IsEnabled).ToList();
    }
    public List<string> GetTypeAllPlugins()
    {
        return _plugins.Where(plugin => plugin.IsEnabled).GroupBy(x => x.Extension.TypeName).Select(x => x.Key).ToList();
    }
    public List<IExtension> GetPluginsByExtensionsInput(List<string> fileExtensions)
    {
        return _plugins.Where(plugin => fileExtensions
        .Any(extension => plugin.Extension.TypeName.Contains(extension, StringComparison.OrdinalIgnoreCase))).Where(plugin => plugin.IsEnabled).Select(plugin => plugin.Extension).ToList();
    }
    private void OnPluginLoaded(string pluginName, bool success)
    {
        PluginLoaded?.Invoke(pluginName, success);
    }
}
