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
    // Search for files with a specific extension in a directory and its subdirectories.
    public List<string> SearchFilesByExtension(string rootDirectory, string fileExtension)
    {
        List<string> foundFiles = new List<string>();

        try
        {
            // Recursively search for files with the specified extension.
            foundFiles = Directory.GetFiles(rootDirectory, $"*.{fileExtension}", System.IO.SearchOption.AllDirectories).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return foundFiles;
    }

    public ConcurrentBag<string> SearchFiles_(string rootPath, string query,string fileextension)
    {
        var foundFiles = new ConcurrentBag<string>();

        try
        {
            var allTxtFiles = Directory.EnumerateFiles(rootPath, $"*{fileextension}", System.IO.SearchOption.AllDirectories);
            Parallel.ForEach(allTxtFiles, (file) =>
            {
                if (file.Contains(query) || File.ReadAllText(file).Contains(query))
                {
                    foundFiles.Add(file);
                }
            });
        }
        catch (Exception ex)
        {
            // Log the exception using Logger from Utilities
            // Logger.Log(ex.Message);
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return foundFiles;
    }
    public List<string> SearchFiles(string rootDirectory, List<IFileTypePlugin> plugins, string searchQuery)
    {
        var result = new List<string>();
        try
        {
            PluginFactory pluginFactory = new PluginFactory(plugins);
            result = pluginFactory.ExecutePlugins(rootDirectory, searchQuery);

            // Check if the root directory exists
            //if (Directory.Exists(rootDirectory))
            //{
            //    // Search for files with the specified extension and containing the search query in all subdirectories
            //    string[] files = Directory.GetFiles(rootDirectory, $"*.{fileType}", SearchOption.AllDirectories)
            //        .Where(file => File.ReadAllText(file).Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
            //        .ToArray();

            //    result = files.ToList();
            //}
        }
        catch (Exception ex)
        {

        }
        return result;
    }
}
