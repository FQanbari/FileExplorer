namespace FileExplorer.UserInterface;

public class InputHandler
{
    public static bool TryParseFileExtension(string input, out string fileExtension)
    {
        // Check if the input is a valid file extension (e.g., "txt" or "xml").
        // You can add more validation logic here as needed.
        fileExtension = input.Trim().ToLower();

        if (!string.IsNullOrWhiteSpace(fileExtension))
        {
            return true;
        }
        else
        {
            fileExtension = null;
            return false;
        }
    }

    public static bool TryParseRootDirectory(string input, out string rootDirectory)
    {
        // Check if the input is a valid directory path.
        // You can add more validation logic here as needed.
        rootDirectory = input.Trim();

        if (!string.IsNullOrWhiteSpace(rootDirectory) && System.IO.Directory.Exists(rootDirectory))
        {
            return true;
        }
        else
        {
            rootDirectory = null;
            return false;
        }
    }
}