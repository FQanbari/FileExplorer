using FileExplorer.ExtensionPlatfrom;

namespace FileSearch
{
    [FileExplorerExtenstion("CSV")]
    class SearchFilesInCsv : IExtension
    {
        public string TypeName => "CSV";

        public List<string> Execute(string rootDirectory, string searchQuery)
        {
            var allDirectories = new List<string> { rootDirectory };
            allDirectories.AddRange(Directory.GetDirectories(rootDirectory, "*", SearchOption.AllDirectories));

            int threshold = 3;
            List<Task<List<string>>> tasks = new List<Task<List<string>>>();

            if (allDirectories.Count > threshold)
            {
                for (int i = 0; i < allDirectories.Count; i += threshold)
                {
                    var currentDirs = allDirectories.Skip(i).Take(threshold).ToArray();
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
                results.AddRange(SearchInCsv(directory, searchQuery));
            }

            return results;
        }

        static IEnumerable<string> SearchInCsv(string directory, string searchText)
        {
            var foundFiles = new List<string>();

            try
            {
                var csvFiles = Directory.GetFiles(directory, "*.csv");

                foreach (var file in csvFiles)
                {
                    bool isTextFound = Path.GetFileName(file).Contains(searchText);

                    if (!isTextFound)
                    {
                        try
                        {
                            var lines = File.ReadAllLines(file);

                            foreach (var line in lines)
                            {
                                var fields = line.Split(',');
                                if (fields.Any(field => field.Contains(searchText)))
                                {
                                    isTextFound = true;
                                    break;
                                }
                            }
                        }
                        catch (IOException)
                        {
                            // Handle or log the exception
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
                // Handle or log the exception
            }

            return foundFiles;
        }
    }
}
