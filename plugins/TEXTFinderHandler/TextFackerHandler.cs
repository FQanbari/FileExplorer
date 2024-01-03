using FileExplorer.ExtensionPlatfrom;

namespace TEXTFinderHandler;

[FileExplorerExtenstion("TXT")]
public class TextFackerHandler : IExtension
{
    public string TypeName => "TXT";
    public int SearchThreshold { get; set; }
    static List<string> foundFiles = new List<string>();
    static object lockObject = new object();
    public List<string> Execute(string rootDirectory, string searchQuery)
    {
        Task<List<string>> mainSearchTask = Task.Run(() => SearchFiles(rootDirectory, searchQuery));
        List<string> foundFiles = mainSearchTask.Result;

        return foundFiles;
    }
    static Task<List<string>> SearchFiles(string directory, string searchTerm)
    {
        return Task.Run(() =>
        {
            List<string> foundFiles = new List<string>();

            try
            {
                string[] txtFiles = Directory.GetFiles(directory, "*.txt");

                Task fileSearchTask = Task.Run(() =>
                {
                    foreach (var currentFile in txtFiles)
                    {
                        string fileContent = File.ReadAllText(currentFile);
                        if (fileContent.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        {
                            lock (lockObject)
                            {
                                foundFiles.Add(currentFile);
                            }
                        }
                    }
                });

                fileSearchTask.Wait();
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"Access to the path '{directory}' is denied.");
            }

            try
            {
                string[] subdirectories = Directory.GetDirectories(directory);
                foreach (var currentSubdirectory in subdirectories)
                {
                    Thread subdirectoryThread = new Thread(() =>
                    {
                        List<string> filesInSubdirectory = SearchFiles(currentSubdirectory, searchTerm).Result;
                        lock (lockObject)
                        {
                            foundFiles.AddRange(filesInSubdirectory);
                        }
                    });
                    subdirectoryThread.Start();
                    subdirectoryThread.Join();
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"Access to a subdirectory in '{directory}' is denied.");
            }

            return foundFiles;
        });
    }


}