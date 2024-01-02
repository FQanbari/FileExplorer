using FileExplorer.ExtensionPlatfrom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FileSearch;
[FileExplorerExtenstion("XML")]
class SearchFilesInXml : IExtension
{
    public string TypeName => "XML";
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
            results.AddRange(SearchInXml(directory, searchQuery));
        }

        return results;
    }

    static IEnumerable<string> SearchInXml(string directory, string searchText)
    {
        var foundFiles = new List<string>();

        try
        {
            var xmlFiles = Directory.GetFiles(directory, "*.xml");

            foreach (var file in xmlFiles)
            {
                bool isTextFound = Path.GetFileName(file).Contains(searchText);

                if (!isTextFound)
                {
                    try
                    {
                        var xmlContent = XElement.Load(file);
                        if (SearchXmlElements(xmlContent, searchText))
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

    static bool SearchXmlElements(XElement element, string searchText)
    {
        if (element.Value.Contains(searchText))
        {
            return true;
        }

        foreach (var child in element.Elements())
        {
            if (SearchXmlElements(child, searchText))
            {
                return true;
            }
        }

        return false;
    }
}
