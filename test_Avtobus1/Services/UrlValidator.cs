using System.Text.RegularExpressions;

namespace test_Avtobus1.Services;

public class UrlValidator
{
    private readonly Regex DomainOnlyRegex =
        new Regex(@"^[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)+(/.*)?$", RegexOptions.Compiled);

    public bool IsValid(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        input = input.Trim();

        // Case 1: Full URL with scheme
        if (input.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            input.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return Uri.TryCreate(input, UriKind.Absolute, out var uri)
                   && !string.IsNullOrWhiteSpace(uri.Host);
        }

        // Case 2: Domain only (example.com or example.co.uk/path)
        return DomainOnlyRegex.IsMatch(input);
    }
}