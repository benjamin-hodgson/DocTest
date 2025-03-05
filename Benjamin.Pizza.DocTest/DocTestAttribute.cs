namespace Benjamin.Pizza.DocTest;

/// <summary>
/// Generate doctests for the given assembly.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class DocTestAttribute : Attribute
{
    /// <summary>
    /// A type in the assembly for which to generate doctests.
    /// </summary>
    public Type TypeInAssemblyToDoctest { get; }

    /// <summary>
    /// Creates the attribute.
    /// </summary>
    /// <param name="typeInAssemblyToDoctest">A type in the assembly for which to generate doctests.</param>
    public DocTestAttribute(Type typeInAssemblyToDoctest)
    {
        TypeInAssemblyToDoctest = typeInAssemblyToDoctest;
    }
}
