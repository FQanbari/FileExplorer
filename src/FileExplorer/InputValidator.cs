









public static class InputValidator
{
    // Validates the root directory path provided by the user.
    public static bool IsValidDirectory(string directoryPath)
    {
        return Directory.Exists(directoryPath);
    }

    // Validates the file type input. You can expand this as needed.
    public static bool IsValidFileType(string fileType)
    {
        var allowedFileTypes = new HashSet<string> { "txt", "json", "xml", "pdf" }; // Expandable list
        return allowedFileTypes.Contains(fileType.ToLower());
    }

    // Validates other inputs as necessary, e.g., search queries, pluginManager names, etc.

    // Example: Validates search query (basic implementation)
    public static bool IsValidSearchQuery(string query)
    {
        // Simple example: check if not empty. You can add more complex validations.
        return !string.IsNullOrWhiteSpace(query);
    }

    // Add more validation methods as required by your application.
}
