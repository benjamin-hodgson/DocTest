using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis.CSharp.Scripting;
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
    private readonly Assembly _assembly;
    private readonly string _name;
    private readonly string _code;
    private readonly string? _preamble;

    /// <summary>Constructor</summary>
    public DocTest(Assembly assembly, string name, string code, string? preamble)
    {
        _assembly = assembly;
        _name = name;
        _code = code;
        _preamble = preamble;
    }

    /// <summary>ToString</summary>
    public override string ToString() => _name;

    /// <summary>Run the test</summary>
    public async Task Run()
    {
        var (output, error) = await RunDocTest();
        Assert.Equal("", error);
        Assert.Equal(GetExpected(), SplitLines(output));
    }

    private Task<(string output, string error)> RunDocTest()
    {
        var options = ScriptOptions.Default.AddReferences(_assembly).AddImports("System");

        return RedirectConsole(() => CSharpScript.RunAsync(_preamble + _code, options));
    }

    private IEnumerable<string> GetExpected()
    {
        var match = new Regex(@"// Output:\s*").Match(_code);
        return SplitLines(_code[(match.Index + match.Length)..])
            .Select(line => new Regex(@"^\s*(//( |$))?").Replace(line, ""));
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
