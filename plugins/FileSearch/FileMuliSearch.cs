using FileExplorer.ExtensionPlatfrom;

namespace FileSearch
{
    [FileExplorerExtenstion("TXT")]
    class FileMuliSearch : IExtension
    {
        public string TypeName => "TXT";

        public int SearchThreshold { get; set; }

        public List<string> Execute(string rootDirectory, string searchQuery)
        {
            var directories = new List<string> { rootDirectory };
            directories.AddRange(Directory.GetDirectories(rootDirectory, "*", SearchOption.AllDirectories));

            List<Task<List<string>>> tasks = new List<Task<List<string>>>();

            if (directories.Count > SearchThreshold)
            {
                for (int i = 0; i < directories.Count; i += SearchThreshold)
                {
                    var currentDirs = directories.Skip(i).Take(SearchThreshold).ToList();
                    tasks.Add(Task.Run(() => SearchFiles(currentDirs, searchQuery)));
                }
            }
            else
            {
                tasks.Add(Task.Run(() => SearchFiles(directories, searchQuery)));
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

        static List<string> SearchFiles(List<string> directories, string searchQuery)
        {
            var results = new List<string>();

            foreach (var directory in directories)
            {
                results.AddRange(SearchFilesInDirectory(directory, searchQuery));
            }

            return results;
        }

        static IEnumerable<string> SearchFilesInDirectory(string directory, string searchText)
        {
            var foundFiles = new List<string>();

            try
            {
                var txtFiles = Directory.GetFiles(directory, "*.txt");

                foreach (var file in txtFiles)
                {
                    bool isTextFound = Path.GetFileName(file).Contains(searchText);

                    if (!isTextFound)
                    {
                        try
                        {
                            var content = File.ReadAllText(file);
                            if (content.Contains(searchText))
                            {
                                isTextFound = true;
                            }
                        }
                        catch (IOException)
                        {
                        }
                    }

                    if (isTextFound)
                    {
                        foundFiles.Add(file);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
            }

            return foundFiles;
        }
    }
}
