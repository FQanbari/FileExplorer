using FileExplorer.ExtensionPlatfrom;

namespace TEXTFinderHandler;

[FileExplorerExtenstion("TXT")]
public class TextFackerHandler : IExtension
{
    public string TypeName => "TXT";

    public List<string> Execute(string rootDirectory, string searchQuery)
    {
        return new List<string> { "sdkfjk" };
    }
}