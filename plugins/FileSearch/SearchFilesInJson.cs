using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileExplorer.ExtensionPlatfrom;
using Newtonsoft.Json.Linq; 

namespace FileSearch;

[FileExplorerExtenstion("JSON")]
class SearchFilesInJson : IExtension
{
    public string TypeName => "JSON";
    public int SearchThreshold { get; set; }

    public List<string> Execute(string rootDirectory, string searchQuery)
    {
        var allDirectories = new List<string> { rootDirectory };
        allDirectories.AddRange(Directory.GetDirectories(rootDirectory, "*", SearchOption.AllDirectories));

        List<Task<List<string>>> tasks = new List<Task<List<string>>>();

        if (allDirectories.Count > SearchThreshold)
        {
            for (int i = 0; i < allDirectories.Count; i += SearchThreshold)
            {
                var currentDirs = allDirectories.Skip(i).Take(SearchThreshold).ToArray();
                tasks.Add(Task.Run(() => SearchFiles(currentDirs, searchQuery)));
            }
        }
        else
        {
            tasks.Add(Task.Run(() => SearchFiles(allDirectories.ToArray(), searchQuery)));
        }

        var allResults = new List<string>();
        Task.WhenAll(tasks).ContinueWith(t =>
        {
            foreach (var task in tasks)
            {
                allResults.AddRange(task.Result);
            }
        }).Wait();

        return allResults;
    }

    static List<string> SearchFiles(string[] directories, string searchQuery)
    {
        var results = new List<string>();

        foreach (var directory in directories)
        {
            results.AddRange(SearchInJson(directory, searchQuery));
        }

        return results;
    }

    static IEnumerable<string> SearchInJson(string directory, string searchText)
    {
        var foundFiles = new List<string>();

        try
        {
            var jsonFiles = Directory.GetFiles(directory, "*.json");

            foreach (var file in jsonFiles)
            {
                bool isTextFound = Path.GetFileName(file).Contains(searchText);

                if (!isTextFound)
                {
                    try
                    {
                        var jsonContent = File.ReadAllText(file);
                        var jObject = JObject.Parse(jsonContent);

                        if (SearchJObject(jObject, searchText))
                        {
                            isTextFound = true;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                if (isTextFound)
                {
                    foundFiles.Add(file);
                }
            }
        }
        catch (Exception)
        {
        }

        return foundFiles;
    }

    static bool SearchJObject(JToken jToken, string searchText)
    {
        if (jToken.Type == JTokenType.Object)
        {
            foreach (var child in jToken.Children<JProperty>())
            {
                if (SearchJObject(child.Value, searchText))
                {
                    return true;
                }
            }
        }
        else if (jToken.Type == JTokenType.Array)
        {
            foreach (var child in jToken.Children())
            {
                if (SearchJObject(child, searchText))
                {
                    return true;
                }
            }
        }
        else if (jToken.Type == JTokenType.Property)
        {
            if (((JProperty)jToken).Value.ToString().Contains(searchText))
            {
                return true;
            }
        }
        else
        {
            if (jToken.ToString().Contains(searchText))
            {
                return true;
            }
        }

        return false;
    }
}
