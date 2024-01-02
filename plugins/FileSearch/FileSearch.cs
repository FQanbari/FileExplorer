using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using FileExplorer.ExtensionPlatfrom;

namespace FileSearch
{

    [FileExplorerExtenstion("TXT")]
    class FileSearch : IExtension
    {
        public string TypeName => "TXT";
        public int SearchThreshold { get; set; }

        public async Task<List<string>> SearchFilesAsync(string rootDirectory, string query)
        {
            var directories = new List<string> { rootDirectory };
            directories.AddRange(Directory.GetDirectories(rootDirectory, "*", SearchOption.AllDirectories));
            var searchTasks = new List<Task<List<string>>>();

            foreach (var dir in directories)
            {
                var task = Task.Run(() => SearchInDirectory(dir, query));
                searchTasks.Add(task);
            }

            var results = await Task.WhenAll(searchTasks);
            var allMatches = results.SelectMany(r => r).ToList();
            return allMatches;
        }

        private List<string> SearchInDirectory(string directory, string query)
        {
            var matchedFiles = new List<string>();

   
            var txtFiles = Directory.GetFiles(directory, "*.txt");
            foreach (var file in txtFiles)
            {
                try
                {
                    var content = File.ReadAllText(file);
                    if (content.Contains(query))
                    {
                        matchedFiles.Add(file);
                    }
                }
                catch (IOException ex)
                {
                    
                }
            }

            return matchedFiles;
        }


        public List<string> Execute(string rootDirectory, string searchQuery)
        {
            Task<List<string>> mainSearchTask = Task.Run(() => SearchFilesAsync(rootDirectory, searchQuery));
            List<string> foundFiles = mainSearchTask.Result;
            return foundFiles;
        }
    }
}
