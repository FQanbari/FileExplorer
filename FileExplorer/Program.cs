Console.WriteLine("=== BOOTCAMP SEARCH :: An extendible command-line search tool ===\n");

DisplayMenu();

var option = Console.ReadLine();

switch (option)
{
    case "1":
        DisplayFileSearch();
        break;
    default:
        break;
}


static void DisplayMenu()
{
    Console.WriteLine("1. Search for files");
    Console.WriteLine("2. Manage Extensions");
    Console.WriteLine("3. View search history\n");
    Console.WriteLine("4. Exit\n");
    Warning();
    Console.WriteLine("Please input an option:");
}

static void Warning()
{
    Console.WriteLine("NOTE: There was a problem with loading 2 extensions. View them in Manage Extensions section.");
    Console.WriteLine();
}

static void SearchFiles(string rootPath, string fileExtension, string query)
{
    DirectoryInfo dir = new DirectoryInfo(rootPath);
    FileInfo[] files;

    try
    {
        // Search for files with the specified extension in all directories
        files = dir.GetFiles("*" + fileExtension, SearchOption.AllDirectories);
    }
    catch (Exception e)
    {
        Console.WriteLine("Error: " + e.Message);
        return;
    }

    // Display each found file
    foreach (FileInfo file in files)
    {
        Console.WriteLine(file.FullName);
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