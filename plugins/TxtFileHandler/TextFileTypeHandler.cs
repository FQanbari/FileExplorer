using FileExplorer.PluginInterface;

namespace TxtFileHandler;

[FileSearchPlugin("TXT")]
public class TextFileTypeHandler : IFileTypePlugin
{
    public string TypeName => "TXT";

    public List<string> Execute(string rootDirectory, string searchQuery)
    {
        var result = new List<string>();

        string[] allfiles = Directory.GetFiles(rootDirectory, "*.*", System.IO.SearchOption.AllDirectories);

        List<string> filteredFiles = GetTextFilesWithPattern(rootDirectory, searchQuery);

        // Process the filtered file paths
        Parallel.ForEach(filteredFiles, filePath =>
        {
            string fileText = ReadTextFromFile(filePath);
        });



        return result;
    }


    static List<string> GetTextFilesWithPattern(string directoryPath, string pattern)
    {
        try
        {
            // Get all text files in the directory and its subdirectories containing the specified pattern in their names
            var filteredFiles = Directory.GetFiles(directoryPath, "*.txt", System.IO.SearchOption.AllDirectories)
                .Where(file => file.ToLower().EndsWith(".txt"))
                .ToList();

            return filteredFiles;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return new List<string>();
        }
    }

    static string ReadTextFromFile(string filePath)
    {
        try
        {
            // Read all text from the file
            string fileText = File.ReadAllText(filePath);
            return fileText;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file '{filePath}': {ex.Message}");
            return string.Empty;
        }
    }
    static void SearchFiles_(string rootDirectory, string searchQuery)
    {
        try
        {
            // Check if the root directory exists
            if (Directory.Exists(rootDirectory))
            {
                // Determine the number of folders in the root directory
                int numberOfFolders = Directory.GetDirectories(rootDirectory, "*", System.IO.SearchOption.AllDirectories).Length;

                if (numberOfFolders > 10)
                {
                    // If there are more than 10 folders, use parallel processing with Tasks
                    SearchFilesParallel_(rootDirectory, searchQuery);
                }
                else
                {
                    // Use sequential processing
                    SearchFilesSequential_(rootDirectory, searchQuery);
                }
            }
            else
            {
                Console.WriteLine("Invalid directory path. Please make sure the directory exists.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void SearchFilesSequential_(string rootDirectory, string searchQuery)
    {
        // Search for files with the specified query in all subdirectories sequentially
        string[] files = Directory.GetFiles(rootDirectory, "*", System.IO.SearchOption.AllDirectories)
            .Where(file => File.ReadAllText(file).Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
            .ToArray();

        DisplaySearchResults_(files);
    }

    static void SearchFilesParallel_(string rootDirectory, string searchQuery)
    {
        // Search for files with the specified query in all subdirectories in parallel using Tasks and Threads
        string[] files = Directory.GetFiles(rootDirectory, "*", System.IO.SearchOption.AllDirectories);

        Parallel.ForEach(files, file =>
        {
            Task.Run(() =>
            {
                if (File.ReadAllText(file).Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"File found: {file}");
                }
            }).Wait(); // Ensure the Task completes before moving to the next file
        });
    }

    static void DisplaySearchResults_(string[] files)
    {
        if (files.Length > 0)
        {
            Console.WriteLine($"Found {files.Length} files containing the search query:");
            foreach (string file in files)
            {
                Console.WriteLine(file);
            }
        }
        else
        {
            Console.WriteLine("No files containing the search query found in the specified directory.");
        }
    }

}