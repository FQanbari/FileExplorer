using FileExplorer.MainApp;

Console.WriteLine("=== BOOTCAMP SEARCH :: An extendible command-line search tool ===\n");


var app = new App();
app.Run();

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
// Get the root directory from the user
Console.Write("Enter the root directory path: ");
string rootDirectory = Console.ReadLine();

// Get the query from the user
Console.Write("Enter the search query: ");
string searchQuery = Console.ReadLine();

// Perform the search using Tasks and Threads
SearchFiles_(rootDirectory, searchQuery);

static void SearchFiles_(string rootDirectory, string searchQuery)
{
    try
    {
        // Check if the root directory exists
        if (Directory.Exists(rootDirectory))
        {
            // Determine the number of folders in the root directory
            int numberOfFolders = Directory.GetDirectories(rootDirectory, "*", SearchOption.AllDirectories).Length;

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