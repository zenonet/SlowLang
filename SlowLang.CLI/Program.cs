namespace SlowLang.CLI;

static class Program
{
    public static void Main(string[] args)
    {
        SlowLang.Interpreter.Interpreter.RunScript(args[0]);
    }
}