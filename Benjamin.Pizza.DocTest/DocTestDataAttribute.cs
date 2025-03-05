using System.Reflection;
using System.Xml.Linq;

using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

using Xunit;
using Xunit.Sdk;
using Xunit.v3;

namespace Benjamin.Pizza.DocTest;

/// <summary>
/// A <see cref="DataAttribute"/> for doctests.
/// </summary>
public sealed class DocTestDataAttribute : DataAttribute
{
    /// <summary>
    /// A type (any type) in the assembly whose documentation you want to test.
    /// </summary>
    public Type TypeInAssemblyToDoctest { get; }

    /// <summary>
    /// Code to inject into each doctest script.
    /// </summary>
    public string? Preamble { get; set; }

    /// <summary>Constructor.</summary>
    /// <param name="typeInAssemblyToDoctest">A type in the target assembly.</param>
    public DocTestDataAttribute(Type typeInAssemblyToDoctest)
    {
        TypeInAssemblyToDoctest = typeInAssemblyToDoctest;
    }

    /// <inheritdoc cref="DataAttribute.GetData"/>
    public override async ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
        MethodInfo testMethod,
        DisposalTracker disposalTracker)
    {
        await Task.CompletedTask;
        var assembly = TypeInAssemblyToDoctest.Assembly;

        var options = ScriptOptions.Default.AddReferences(assembly).AddImports("System");
        var script = CSharpScript.Create(Preamble ?? "", options);

        var path = Path.ChangeExtension(assembly.Location, "xml");
        var xml = XDocument.Parse(File.ReadAllText(path));
        return (
            from mem in xml.Descendants()
            where mem.Name == "member"
            from ex in mem.Descendants()
            where ex.Name == "example"
            let codes = ex
                .Elements()
                .Where(c => c.Name == "code" && c.Attribute("doctest")?.Value == "true")
                .Select((x, i) => (ix: i, code: x.Value))
            from c in codes
            let name = ex.Attribute("name")!.Value
                + (codes.Count() > 1 ? " > " + c.ix : "")
            select new DocTest(name, c.code, script)
        )
            .Distinct()
            .Select(x => ConvertDataRow(new[] { x }))
            .ToList();
    }

    /// <inheritdoc cref="DataAttribute.SupportsDiscoveryEnumeration"/>
    public override bool SupportsDiscoveryEnumeration()
        => true;
}
