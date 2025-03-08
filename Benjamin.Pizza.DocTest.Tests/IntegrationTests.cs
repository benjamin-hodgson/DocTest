using Benjamin.Pizza.DocTest.TestProject;

using Xunit;

namespace Benjamin.Pizza.DocTest.Tests;

[DocTest(typeof(ExampleSut))]
public sealed partial class IntegrationTests
{
    [Fact]
    public void TestTests()
    {
        var generatedMethods = typeof(IntegrationTests)
            .GetMethods()
            .Where(m => m.CustomAttributes.Any(a => a.AttributeType == typeof(FactAttribute)));

        // 4: this method, plus the three generated doctests.
        // todo: maybe try to assert that the generated tests actually ran?
        Assert.Equal(4, generatedMethods.Count());
    }
}
