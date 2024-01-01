using FileExplorer.ExtensionPlatfrom;

namespace TEXTFinderHandler;

[FileExplorerExtenstion("TXT")]
public class TextFackerHandler : IExtension
{
    public string TypeName => "TXT";
    static List<string> foundFiles = new List<string>();
    static object lockObject = new object();
    public List<string> Execute(string rootDirectory, string searchQuery)
    {
        Task<List<string>> mainSearchTask = Task.Run(() => SearchFiles(rootDirectory, searchQuery));

        // Wait for the main search task to complete
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
                // Get all .txt files in the current directory
                string[] txtFiles = Directory.GetFiles(directory, "*.txt");

                // Use Task to run the file search in parallel
                Task fileSearchTask = Task.Run(() =>
                {
                    foreach (var currentFile in txtFiles)
                    {
                        // Check if the file content contains the search term
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

                // Wait for the file search task to complete
                fileSearchTask.Wait();
            }
            catch (UnauthorizedAccessException)
            {
                // Handle the case where access to the directory is denied
                Console.WriteLine($"Access to the path '{directory}' is denied.");
            }

            try
            {
                // Recursively search subdirectories using Thread
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
                // Handle the case where access to a subdirectory is denied
                Console.WriteLine($"Access to a subdirectory in '{directory}' is denied.");
            }

            return foundFiles;
        });
    }


}