namespace FileExplorer.FileSearch;

public class TaskManager
{
    // Method to run an asynchronous task with a specified action
    public Task RunAsync(Action action)
    {
        return Task.Run(action);
    }

    // Method to run an asynchronous task with a return value
    public Task<T> RunAsync<T>(Func<T> function)
    {
        return Task.Run(function);
    }

    // Method to handle multiple tasks and wait for all of them to complete
    public void WaitForAll(params Task[] tasks)
    {
        Task.WaitAll(tasks);
    }

    // Method to handle multiple tasks and proceed when any of them completes
    public void WaitForAny(params Task[] tasks)
    {
        Task.WaitAny(tasks);
    }

    // Method to process a collection of items concurrently
    // 'action' is applied to each item in 'items'
    public Task ProcessConcurrentlyAsync<T>(IEnumerable<T> items, Action<T> action)
    {
        var tasks = items.Select(item => Task.Run(() => action(item)));
        return Task.WhenAll(tasks);
    }

    // Additional utility methods for error handling, task cancellation, etc.

    // Example: Method to handle exceptions from a task
    public void HandleTaskException(Task task)
    {
        if (task.Exception != null)
        {
            // Handle or log the exception
            // Example: Log the details of the exception
        }
    }

    // Method to cancel a running task, if cancellation is supported
    public void CancelTask(Task task, CancellationTokenSource cancellationTokenSource)
    {
        cancellationTokenSource.Cancel();
        // Additional logic for handling task cancellation
    }
}
