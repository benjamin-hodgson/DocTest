using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis.Scripting;

using Xunit;

namespace Benjamin.Pizza.DocTest;

/// <summary>A doctest</summary>
/// <example name="A test for the doctest">
/// <code doctest="true">
/// Console.WriteLine("Hello world");
/// // Output:
/// // Hello world
/// </code>
/// </example>
[SuppressMessage("naming", "CA1724", Justification = "DGAF")]  // "The type name conflicts in whole or in part with the namespace name"
public class DocTest
{
    private static readonly Regex _outputRegex = new(@"// Output:\s*", RegexOptions.Compiled);
    private static readonly Regex _commentRegex = new(@"^\s*(//( |$))?", RegexOptions.Compiled);
    private readonly Script _script;
    private readonly string _name;
    private readonly string _code;

    /// <summary>Constructor</summary>
    public DocTest(string name, string code, Script preamble)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(code);
        ArgumentNullException.ThrowIfNull(preamble);

        _script = preamble.ContinueWith(code);
        _code = code;
        _name = name;
    }

    /// <summary>ToString</summary>
    public override string ToString() => _name;

    /// <summary>Run the test</summary>
    public async Task Run()
    {
        var (output, error) = await RedirectConsole(() => _script.RunAsync());
        Assert.Equal("", error);
        Assert.Equal(GetExpected(), SplitLines(output));
    }

    private IEnumerable<string> GetExpected()
    {
        var match = _outputRegex.Match(_code);
        return SplitLines(_code[(match.Index + match.Length)..])
            .Select(line => _commentRegex.Replace(line, ""));
    }

    private static async Task<(string output, string error)> RedirectConsole(Func<Task> action)
    {
        using var outBuffer = new StringWriter();
        using var errBuffer = new StringWriter();
        var oldConsoleOut = Console.Out;
        var oldConsoleErr = Console.Error;
        Console.SetOut(outBuffer);
        Console.SetError(errBuffer);
        try
        {
            await action();
        }
        finally
        {
            Console.SetOut(oldConsoleOut);
            Console.SetError(oldConsoleErr);
        }
        return (outBuffer.ToString(), errBuffer.ToString());
    }

    private static string[] SplitLines(string str)
        => str.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
}
