using Xunit;

namespace Benjamin.Pizza.DocTest.Tests;

public class DocTestTests
{
    [Theory]
    [DocTestData(typeof(DocTest))]
    public async Task DoDocTests(DocTest test)
    {
        await test.Run();
    }
}
