using System.IO;
using System.Text;
using Xunit;

namespace SlowLang.Tests;

public class UnitTest1
{
    private readonly StringBuilder stringBuilder = new ();
    public void Initialize()
    {
        TextWriter textWriter = new StringWriter(stringBuilder);

        Interpreter.Interpreter.OutputStream = textWriter;
    }
    
    [Fact]
    public void HelloWorld()
    {
        Initialize();
        SlowLang.Interpreter.Interpreter.RunScript("print(\"Hello World\")");
        
        Xunit.Assert.True(stringBuilder.ToString().TrimEnd('\n').TrimEnd('\r') == "Hello World");
    }
    
    [Fact]
    public void HelloWorldWithSemicolon()
    {
        Initialize();
        SlowLang.Interpreter.Interpreter.RunScript("print(\"Hello World\");");
        
        Xunit.Assert.True(stringBuilder.ToString().TrimEnd('\n').TrimEnd('\r') == "Hello World");
    }
}