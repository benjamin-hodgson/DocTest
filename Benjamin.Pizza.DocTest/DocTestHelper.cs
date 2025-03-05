namespace Benjamin.Pizza.DocTest;

/// <summary>
/// Helper functions to be called from doctests.
/// </summary>
public static class DocTestHelper
{
    private static readonly string[] _newlines = { "\r\n", "\n" };

    /// <summary>
    /// Split the string at newlines.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <returns>An array of strings.</returns>
    public static string[] SplitLines(string str)
    {
        if (str == null)
        {
            throw new ArgumentNullException(nameof(str));
        }

        return str.Split(_newlines, StringSplitOptions.None);
    }

    /// <summary>
    /// Redirect the console for the duration of the test.
    /// </summary>
    /// <returns>An <see cref="IDisposable"/>.</returns>
    public static ConsoleRedirector RedirectConsole()
        => new();
}
