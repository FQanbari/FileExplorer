namespace FileExplorer.PluginInterface;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class FileSearchPluginAttribute : Attribute
{
    public string FileExtension { get; }

    public FileSearchPluginAttribute(string fileExtension)
    {
        FileExtension = fileExtension;
    }
}
