//using FileExplorer.ExtensionPlatfrom;
//using iTextSharp.text.pdf.parser;
//using UglyToad.PdfPig;
//namespace FileSearch;

//[FileExplorerExtenstion("PDF")]
//class SearchFilesInPdf : IExtension
//{
//    public string TypeName => "PDF";

//    public List<string> Execute(string rootDirectory, string searchQuery)
//    {
//        var allDirectories = new List<string> { rootDirectory };
//        allDirectories.AddRange(Directory.GetDirectories(rootDirectory, "*", SearchOption.AllDirectories));

//        int threshold = 3;
//        List<Task<List<string>>> tasks = new List<Task<List<string>>>();

//        if (allDirectories.Count > threshold)
//        {
//            for (int i = 0; i < allDirectories.Count; i += threshold)
//            {
//                var currentDirs = allDirectories.Skip(i).Take(threshold).ToArray();
//                tasks.Add(Task.Run(() => SearchFiles(currentDirs, searchQuery)));
//            }
//        }
//        else
//        {
//            tasks.Add(Task.Run(() => SearchFiles(allDirectories.ToArray(), searchQuery)));
//        }

//        var allResults = new List<string>();
//        Task.WhenAll(tasks).ContinueWith(t =>
//        {
//            foreach (var task in tasks)
//            {
//                allResults.AddRange(task.Result);
//            }
//        }).Wait();

//        return allResults;
//    }

//    static List<string> SearchFiles(string[] directories, string searchQuery)
//    {
//        var results = new List<string>();

//        foreach (var directory in directories)
//        {
//            results.AddRange(SearchInPdf(directory, searchQuery));
//        }

//        return results;
//    }
//    static IEnumerable<string> SearchInPdf(string directory, string searchText)
//    {
//        var foundFiles = new List<string>();

//        try
//        {
//            var pdfFiles = Directory.GetFiles(directory, "*.pdf");

//            foreach (var file in pdfFiles)
//            {
//                bool isTextFound = Path.GetFileName(file).Contains(searchText);

//                if (!isTextFound)
//                {
//                    try
//                    {
//                        using (PdfDocument document = PdfDocument.Open(file))
//                        {
//                            foreach (var page in document.GetPages())
//                            {
//                                var text = page.Text;
//                                if (text.Contains(searchText))
//                                {
//                                    isTextFound = true;
//                                    break;
//                                }
//                            }
//                        }
//                    }
//                    catch (Exception)
//                    {
//                    }
//                }

//                if (isTextFound)
//                {
//                    foundFiles.Add(file);
//                }
//            }
//        }
//        catch (Exception)
//        {
//        }

//        return foundFiles;
//    }
//    static IEnumerable<string> SearchInPdf(string directory, string searchText)
//    {
//        var foundFiles = new List<string>();

//        try
//        {
//            var pdfFiles = Directory.GetFiles(directory, "*.pdf");

//            foreach (var file in pdfFiles)
//            {
//                bool isTextFound = System.IO.Path.GetFileName(file).Contains(searchText);

//                if (!isTextFound)
//                {
//                    try
//                    {
//                        using (PdfReader reader = new PdfReader(file))
//                        {
//                            for (int i = 1; i <= reader.NumberOfPages; i++)
//                            {
//                                string textFromPage = PdfTextExtractor.GetTextFromPage(reader, i);
//                                if (textFromPage.Contains(searchText))
//                                {
//                                    isTextFound = true;
//                                    break;
//                                }
//                            }
//                        }
//                    }
//                    catch (Exception)
//                    {
//                    }
//                }

//                if (isTextFound)
//                {
//                    foundFiles.Add(file);
//                }
//            }
//        }
//        catch (Exception)
//        {
//        }

//        return foundFiles;
//    }
//}