namespace FileExplorer.FileSearch;

public static class FileUtils
{
    // Method to read the content of a file
    public static string ReadFile(string filePath)
    {
        try
        {
            return File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            // Handle or log the exception appropriately
            throw;
        }
    }

    // Method to write content to a file, with an option to append or overwrite
    public static void WriteToFile(string filePath, string content, bool append = false)
    {
        try
        {
            if (append)
            {
                File.AppendAllText(filePath, content);
            }
            else
            {
                File.WriteAllText(filePath, content);
            }
        }
        catch (Exception ex)
        {
            // Handle or log the exception appropriately
            throw;
        }
    }

    // Method to check if a file exists
    public static bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    // Method to delete a file
    public static void DeleteFile(string filePath)
    {
        try
        {
            File.Delete(filePath);
        }
        catch (Exception ex)
        {
            // Handle or log the exception appropriately
            throw;
        }
    }

    // Additional utility methods as needed, such as copying files, moving files, etc.

    // Example: Method to copy a file
    public static void CopyFile(string sourcePath, string destinationPath, bool overwrite = false)
    {
        try
        {
            File.Copy(sourcePath, destinationPath, overwrite);
        }
        catch (Exception ex)
        {
            // Handle or log the exception appropriately
            throw;
        }
    }

    // More utility methods for directory operations can also be included
}
