using Xunit;

namespace Benjamin.Pizza.DocTest.Tests;

internal class DocTestTests
{
    [Theory]
    [DocTestData(typeof(DocTest))]
    public async Task DoDocTests(DocTest test)
    {
        await test.Run();
    }
}
