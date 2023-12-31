using System.Reflection;

namespace FileExplorer.ExtensionPlatfrom;

public interface IExtension
{
    public string TypeName { get; }

    public List<string> Execute(string rootDirectory, string searchQuery);

    public bool CanHandleFileExtension(string fileExtension)
    {
        var attribute = GetType().GetCustomAttribute<FileExplorerExtenstionAttribute>();
        return attribute != null && attribute.FileExtension.Equals(fileExtension, StringComparison.OrdinalIgnoreCase);
    }
}