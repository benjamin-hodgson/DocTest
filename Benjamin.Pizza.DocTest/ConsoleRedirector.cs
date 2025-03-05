namespace Benjamin.Pizza.DocTest;

/// <summary>
/// Captures the console for the duration of a test.
/// </summary>
public sealed class ConsoleRedirector : IDisposable
{
    private readonly StringWriter _outBuffer;
    private readonly StringWriter _errBuffer;
    private readonly TextWriter _oldConsoleOut;
    private readonly TextWriter _oldConsoleErr;

    /// <summary>
    /// The text that was written to <see cref="Console.Out"/>.
    /// </summary>
    public string CapturedConsoleOut => _outBuffer.ToString();

    /// <summary>
    /// The text that was written to <see cref="Console.Error"/>.
    /// </summary>
    public string CapturedConsoleError => _errBuffer.ToString();

    internal ConsoleRedirector()
    {
        _outBuffer = new StringWriter();
        _errBuffer = new StringWriter();
        _oldConsoleOut = Console.Out;
        _oldConsoleErr = Console.Error;

        Console.SetOut(_outBuffer);
        Console.SetError(_errBuffer);
    }

    /// <summary>
    /// Put the console back how it was.
    /// </summary>
    public void Dispose()
    {
        Console.SetOut(_oldConsoleOut);
        Console.SetError(_oldConsoleErr);

        // Safe to dispose as StringWriter does not
        // destroy its internal stringbuilder upon disposal
        _outBuffer.Dispose();
        _errBuffer.Dispose();
    }
}
