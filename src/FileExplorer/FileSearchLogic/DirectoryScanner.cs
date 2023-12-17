namespace FileExplorer.FileSearchLogic;

public class DirectoryScanner
{
    // Recursively scan directories and subdirectories to find files.
    public List<string> ScanDirectories(string rootDirectory)
    {
        List<string> foundFiles = new List<string>();

        try
        {
            // Get all files in the current directory.
            string[] files = Directory.GetFiles(rootDirectory);

            // Add the found files to the list.
            foundFiles.AddRange(files);

            // Get all subdirectories in the current directory.
            string[] subdirectories = Directory.GetDirectories(rootDirectory);

            // Recursively scan subdirectories.
            foreach (string subdirectory in subdirectories)
            {
                List<string> subdirectoryFiles = ScanDirectories(subdirectory);
                foundFiles.AddRange(subdirectoryFiles);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return foundFiles;
    }
}