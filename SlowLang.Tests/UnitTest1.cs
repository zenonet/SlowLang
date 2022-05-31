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
            .TrimEnd('\r', '\n')
            .Equals(
                expectedOutput
                    .TrimEnd('\n', '\n')
            )
        );
    }
    
    
    [Fact]
    public void HelloWorld()
    {
        TestScript("print(\"Hello World!\");", "Hello World!");
    }

    [Fact]
    public void HelloWorldWithVariable()
    {
        TestScript("hw = \"Hello World!\";" +
                                       "print(hw);",
            "Hello World!");
    }

    [Fact]
    public void GetInputAndReturn()
    {
        Initialize();
        Interpreter.Interpreter.InputStream = new StringReader("Hello SlowLang\n");
        TestScript("print(getInput())", "Hello SlowLang");
    }
}