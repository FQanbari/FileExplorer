using FileExplorer.FileSearch;
using System.Reflection;
using System.Text;

namespace FileExplorer.PluginManagement;

public class PluginLoader
{
    private static int _unLoadedPlugins;
    private static List<string> _extenstions;
    public PluginLoader()
    {
        _extenstions = new List<string>();
    }
    public void Waringn()
    {
        if (_unLoadedPlugins > 0)
            Console.WriteLine($"NOTE: There was a proble with loading {_unLoadedPlugins} extensions. View them in Manage Extenstions section.\n");
    }
    public string Extensionenstions()
    {
        StringBuilder extension = new StringBuilder();
        for (int i = 0; i < _extenstions.Count; i++)
        {
            extension.Append($"[{i + 1}]{_extenstions[i]} ");
        }

        return extension.ToString();
    }
    public IFileTypeHandler LoadPlugin(string pluginsPath)
    {
        try
        {
            //Assembly assembly = Assembly.LoadFile(Path.GetFullPath(pluginsPath));
            // Check if the plugins directory exists
            if (!Directory.Exists(pluginsPath))
            {
                Console.WriteLine("Plugins directory not found.");
                return null;
            }
            foreach (string dll in Directory.GetFiles(pluginsPath, "*.dll"))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(dll);
                    Type[] types = assembly.GetTypes();

                    foreach (Type type in assembly.GetTypes())
                    {
                        //if (type.IsClass && !type.IsAbstract && typeof(IFileTypeHandler).IsAssignableFrom(type))
                        {
                            // Found a class that implements IPlugin
                            IFileTypeHandler pluginInstance = (IFileTypeHandler)Activator.CreateInstance(type);
                            _extenstions.Add(type.FullName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _unLoadedPlugins++;
                }
            }

            return null; // No compatible handler found in the assembly.
        }
        catch (Exception ex)
        {
            _unLoadedPlugins++;
            return null;
        }
    }
}