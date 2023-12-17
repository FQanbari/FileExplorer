namespace FileExplorer.PluginInterface;

public interface IFileTypePlugin
{
    // Gets the supported file extension(s) for this handler.
    string[] SupportedExtensions { get; }

    // Handles the processing of a file with the specified content.
    void ProcessFile(string filePath);

    // Optional: Provide a description of the file type handler.
    string GetHandlerDescription();
}