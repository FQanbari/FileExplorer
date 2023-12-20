namespace FileExplorer.FileSearch;

public interface IPlugin
{
    // A unique identifier for the pluginManager
    string Id { get; }

    // A descriptive name for the pluginManager
    string Name { get; }

    // Initializes the pluginManager with necessary startup logic
    void Initialize();

    // Performs any cleanup or finalization necessary
    void Terminate();
}
