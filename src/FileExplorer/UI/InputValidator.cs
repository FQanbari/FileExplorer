using FileExplorer.Utilities;

namespace FileExplorer.UI;

public static class InputValidator
{
    public static bool ValidateFileExtension(string input, List<string> validExtensions, out int validatedIndex)
    {
        validatedIndex = -1;

        if (!int.TryParse(input, out int inputIndex) || inputIndex <= 0 || inputIndex > validExtensions.Count)
        {
            Helpers.Error($"Invalid input. Please enter a number between 1 and {validExtensions.Count}.");
            return false;
        }

        validatedIndex = inputIndex - 1;
        return true;

    }

    public static bool ValidateDirectory(string input, out string validatedDirectory)
    {
        validatedDirectory = input.Trim();

        if (string.IsNullOrWhiteSpace(validatedDirectory) || !Directory.Exists(validatedDirectory))
        {
            Helpers.Error("Invalid directory path. Please enter a valid path.");
            return false;
        }

        return true;
    }

    public static bool ValidateNonEmptyString(string input, out string validatedString)
    {
        validatedString = input.Trim();

        if (string.IsNullOrWhiteSpace(validatedString))
        {
            Helpers.Error("Input cannot be empty. Please enter a valid input.");
            return false;
        }

        return true;
    }

    public static bool ValidateInteger(string input, out int validatedInteger)
    {
        if (!int.TryParse(input, out validatedInteger))
        {
            Console.WriteLine("Invalid input. Please enter a valid integer.");
            return false;
        }

        return true;
    }
}
