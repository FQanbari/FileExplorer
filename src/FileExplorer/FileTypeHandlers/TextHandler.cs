using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.FileTypeHandlers;

public class TextHandler
{
    private ConcurrentBag<string> _contents;

    public TextHandler()
    {
        _contents = new ConcurrentBag<string>();
    }

    public void ProcessFiles(string[] filePaths)
    {
        Parallel.ForEach(filePaths, (filePath) =>
        {
            try
            {
                string content = File.ReadAllText(filePath);
                _contents.Add(content); // Storing the content of each file
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file {filePath}: {ex.Message}");
            }
        });
    }

    public void DisplayContents()
    {
        foreach (var content in _contents)
        {
            Console.WriteLine(content);
        }
    }
}
