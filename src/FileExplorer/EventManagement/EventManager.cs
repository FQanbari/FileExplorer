using FileExplorer.HistoryManagement;
using FileExplorer.SearchManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

// Command interface
public interface ICommand
{
    void Execute();
}

// Concrete command for saving search history to a file
public class SaveSearchHistoryCommand : ICommand
{
    private List<string> _data;
    private string filePath;

    public SaveSearchHistoryCommand(List<string> data, string filePath)
    {
        this._data = data;
        this.filePath = filePath;
    }

    public void Execute()
    {
        try
        {
            List<SearchHistoryEntry> history;

            if (File.Exists(filePath))
            {
                string existingJson = File.ReadAllText(filePath);
                history = JsonConvert.DeserializeObject<List<SearchHistoryEntry>>(existingJson) ?? new List<SearchHistoryEntry>();
            }
            else
            {
                history = new List<SearchHistoryEntry>();
            }

            history.Add(new SearchHistoryEntry { Timestamp = DateTime.Now, SearchResults = _data });

            string updatedJson = JsonConvert.SerializeObject(history, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, updatedJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving search history: {ex.Message}");
        }
    }
}

// Observer interface
public interface IObserver
{
    void Update(object sender, EventArgs e);
}

// Concrete observer for saving search history
public class SearchHistoryObserver : IObserver
{
    private string filePath;

    public SearchHistoryObserver(string filePath)
    {
        this.filePath = filePath;
    }

    public void Update(object sender, EventArgs e)
    {
        if (e is SearchEventArgs searchEventArgs)
        {
            // Create a command to save search history
            ICommand saveHistoryCommand = new SaveSearchHistoryCommand(searchEventArgs.SearchQuery, filePath);

            // Execute the command
            
            saveHistoryCommand.Execute();
        }
    }
}

// Subject interface
public interface ISubject
{
    event EventHandler<EventArgs> EventOccurred;
    //void TriggerEvent();
}

//// Concrete subject for loading plugins
//public class PluginLoader : ISubject
//{
//    public event EventHandler<EventArgs> EventOccurred;

//    public void TriggerEvent()
//    {
//        try
//        {
//            // Simulate loading plugins
//            // ...

//            // Notify observers that plugin loading was successful
//            OnEventOccurred(EventArgs.Empty);
//        }
//        catch (Exception ex)
//        {
//            // Notify observers about the error
//            OnEventOccurred(new ErrorEventArgs(ex.Message));
//        }
//    }

//    protected virtual void OnEventOccurred(EventArgs e)
//    {
//        EventOccurred?.Invoke(this, e);
//    }
//}

// Custom EventArgs for errors
public class ErrorEventArgs : EventArgs
{
    public string ErrorMessage { get; }

    public ErrorEventArgs(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}
public class SearchEventArgs : EventArgs
{
    public List<string> SearchQuery { get; }
    public string Path { get; }
    public string Choice { get; }
    public List<string> Extensions { get; }

    public SearchEventArgs(List<string>? extensions = null,List<string>? searchQuery = null)
    {
        Extensions = extensions;
        SearchQuery = searchQuery;
    }
}


