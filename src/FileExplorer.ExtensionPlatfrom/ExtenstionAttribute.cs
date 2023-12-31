using System.ComponentModel;

namespace FileExplorer.ExtensionPlatfrom;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class FileExplorerExtenstionAttribute : Attribute
{
    public string FileExtension { get; }

    public FileExplorerExtenstionAttribute(string fileExtension)
    {
        FileExtension = fileExtension;
    }
}
