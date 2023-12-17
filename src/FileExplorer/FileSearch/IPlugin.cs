namespace FileExplorer.FileSearch;

public interface IPlugin
{
    // A unique identifier for the plugin
    string Id { get; }

    // A descriptive name for the plugin
    string Name { get; }

    // Initializes the plugin with necessary startup logic
    void Initialize();

    // Performs any cleanup or finalization necessary
    void Terminate();
}
