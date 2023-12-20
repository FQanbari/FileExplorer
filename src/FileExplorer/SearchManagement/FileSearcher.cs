using FileExplorer.FileSearch;
using FileExplorer.PluginInterface;
using FileExplorer.PluginManagement;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
