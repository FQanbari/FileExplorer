namespace FileExplorer.PluginInterface;

public interface IFileTypePlugin
{
    string TypeName { get; } 
    List<string> Execute(string rootDirectory, string searchQuery);
}