using System.IO;
using System.Text;
using Xunit;

namespace SlowLang.Tests;

public class UnitTest1
{
    private readonly StringBuilder stringBuilder = new ();
    private void Initialize()
    {
        TextWriter textWriter = new StringWriter(stringBuilder);

        Interpreter.Interpreter.OutputStream = textWriter;
    }

    private void TestScript(string code, string expectedOutput)
    {
        Initialize();
        Interpreter.Interpreter.RunScript(code);
        Assert.True(stringBuilder.ToString()
            .TrimEnd('\n')
            .TrimEnd('\r')
            .Equals(
                expectedOutput
                    .TrimEnd('\n')
                    .TrimEnd('\r')
                    )
        );
    }
    
    
    [Fact]
    public void HelloWorld()
    {
        TestScript("print(\"Hello World!\")", "Hello World!");
    }
    
    [Fact]
    public void HelloWorldWithSemicolon()
    {
        TestScript("print(\"Hello World!\");", "Hello World!");
    }

    [Fact]
    public void HelloWorldWithVariable()
    {
        Assert.True(false);
        TestScript("var hw = \"Hello World!\";" +
                        "print(hw);",
            "Hello World!");
    }
}