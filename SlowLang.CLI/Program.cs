
namespace SlowLang.CLI;

static class Program
{
    public static void Main(string[] args)
    {
        Interpreter.Interpreter.RunScript(File.ReadAllText(args[0]));
    }
}