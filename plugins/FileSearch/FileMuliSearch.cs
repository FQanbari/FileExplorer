using FileExplorer.ExtensionPlatfrom;

namespace FileSearch
{
    [FileExplorerExtenstion("TXT")]
    class FileMuliSearch : IExtension
    {
        public string TypeName => "TXT";

        public List<string> Execute(string rootDirectory, string searchQuery)
        {
            var directories = Directory.GetDirectories(rootDirectory, "*", SearchOption.AllDirectories);

            int threshold = 3;
            List<Task<List<string>>> tasks = new List<Task<List<string>>>();

            if (directories.Length > threshold)
            {
                for (int i = 0; i < directories.Length; i += threshold)
                {
                    var currentDirs = directories.Skip(i).Take(threshold).ToArray();
                    tasks.Add(Task.Run(() => SearchFiles(currentDirs,searchQuery)));
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
        static List<string> SearchFiles(string[] directories, string searchQuery)
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
                    try
                    {
                        var content = File.ReadAllText(file);
                        if (content.Contains(searchText))
                        {
                            foundFiles.Add(file);
                        }
                    }
                    catch (IOException)
                    {
                        // Handle or log the exception
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

    [FileExplorerExtenstion("CSV")]
    class SearchFilesInCsv : IExtension
    {
        public string TypeName => "CSV";

        public List<string> Execute(string rootDirectory, string searchQuery)
        {
            var directories = Directory.GetDirectories(rootDirectory, "*", SearchOption.AllDirectories);

            int threshold = 3;
            List<Task<List<string>>> tasks = new List<Task<List<string>>>();

            if (directories.Length > threshold)
            {
                for (int i = 0; i < directories.Length; i += threshold)
                {
                    var currentDirs = directories.Skip(i).Take(threshold).ToArray();
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
        static List<string> SearchFiles(string[] directories, string searchQuery)
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
                var csvFiles = Directory.GetFiles(directory, "*.csv");

                foreach (var file in csvFiles)
                {
                    try
                    {
                        var lines = File.ReadAllLines(file);

                        foreach (var line in lines)
                        {
                            var fields = line.Split(',');
                            if (fields.Any(field => field.Contains(searchText)))
                            {
                                foundFiles.Add(file);
                                break; // Found the text, no need to search further in this file
                            }
                        }
                        var content = File.ReadAllText(file);
                        if (content.Contains(searchText))
                        {
                            foundFiles.Add(file);
                        }
                    }
                    catch (IOException)
                    {
                        // Handle or log the exception
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
