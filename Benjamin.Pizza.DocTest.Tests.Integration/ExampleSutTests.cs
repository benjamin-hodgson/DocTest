using Benjamin.Pizza.DocTest.TestProject;

using Xunit;

namespace Benjamin.Pizza.DocTest.IntegrationTests;

[DocTest(typeof(ExampleSut))]
public sealed partial class ExampleSutTests
{
    [Fact]
    public void TestTests()
    {
        // todo: maybe try to test that the generated test actually ran?
        var generatedMethod = typeof(ExampleSutTests).GetMethod("My_example");
        Assert.NotNull(generatedMethod);
        Assert.Contains(generatedMethod.CustomAttributes, a => a.AttributeType == typeof(FactAttribute));
    }
}
