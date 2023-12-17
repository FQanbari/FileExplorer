Console.WriteLine("=== BOOTCAMP SEARCH :: An extendible command-line search tool ===\n");

// Initialize and run the application.
var app = new App();
app.Run();

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
static void SearchFiles(string rootPath, string fileExtension, string query)
{
    DirectoryInfo dir = new DirectoryInfo(rootPath);
    List<string> matchingFiles = new List<string>();
    try
    {
        var files = Directory.GetFiles(rootPath, "*" + fileExtension, SearchOption.AllDirectories);
        foreach (var file in files)
        {
            string content = File.ReadAllText(file);
            if (content.Contains(query))
            {
                matchingFiles.Add(file);
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred: " + ex.Message);
    }
    Console.WriteLine("Files containing the query:");
    foreach (var file in matchingFiles)
    {
        Console.WriteLine(file);
    }
}

    static void DisplayFileSearch()
{
    Console.Write("Select one of these file types:");
    string extension = Console.ReadLine();

    Console.Write("Pick the root path:");
    string path = Console.ReadLine();


    Console.Write("Query:");
    string query = Console.ReadLine();

    SearchFiles(path, extension, query);
}
