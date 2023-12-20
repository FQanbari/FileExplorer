using System.Reflection;

namespace FileExplorer.PluginInterface;


public interface IFileTypePlugin 
{
    public string TypeName { get; }

    public List<string> Execute(string rootDirectory, string searchQuery);

    public bool CanHandleFileExtension(string fileExtension)
    {
        var attribute = GetType().GetCustomAttribute<FileSearchPluginAttribute>();
        return attribute != null && attribute.FileExtension.Equals(fileExtension, StringComparison.OrdinalIgnoreCase);
    }
}