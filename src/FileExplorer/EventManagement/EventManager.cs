using System;
using System.Collections.Generic;
using System.IO;

// Command interface
public interface ICommand
{
    void Execute();
}

// Concrete command for saving search history to a file
public class SaveSearchHistoryCommand : ICommand
{
    private string _data;
    private string filePath;

    public SaveSearchHistoryCommand(string data, string filePath)
    {
        this._data = data;
        this.filePath = filePath;
    }

    public void Execute()
    {
        try
        {
            if (!File.Exists(filePath))
                File.Create(filePath);
            // Save the search history to a file
            File.AppendAllText(filePath, $"{_data}\n");
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
    public string SearchQuery { get; }

    public SearchEventArgs(string searchQuery)
    {
        SearchQuery = searchQuery;
    }
}


